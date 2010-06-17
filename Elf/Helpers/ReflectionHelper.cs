using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Elf.Core.ClrIntegration;
using System.Linq;
using Elf.Core.Runtime.Impl;
using Elf.Core.TypeSystem;

namespace Elf.Helpers
{
    public static class ReflectionHelper
    {
        private static Dictionary<ICustomAttributeProvider, bool> _isRtimplCache =
            new Dictionary<ICustomAttributeProvider, bool>();

        private static bool GetIsRtimplFromCache(ICustomAttributeProvider cap)
        {
            lock (_isRtimplCache)
            {
                if (!_isRtimplCache.ContainsKey(cap))
                {
                    _isRtimplCache.Add(cap, cap.IsDefined(typeof(RtimplAttribute), false));
                }

                return _isRtimplCache[cap];
            }
        }

        private static Dictionary<ICustomAttributeProvider, String> _rtimplOfCache =
            new Dictionary<ICustomAttributeProvider, String>();

        private static String GetRtimplFromCache(ICustomAttributeProvider cap)
        {
            lock (_rtimplOfCache)
            {
                if (!_rtimplOfCache.ContainsKey(cap))
                {
                    var ria = cap.GetCustomAttributes(typeof(RtimplAttribute), false)
                        .Cast<RtimplAttribute>().SingleOrDefault();
                    _rtimplOfCache.Add(cap, ria == null ? null : ria.Name);
                }

                return _rtimplOfCache[cap];
            }
        }

        public static bool IsRtimpl(this ICustomAttributeProvider cap)
        {
            return GetIsRtimplFromCache(cap);
        }

        public static String RtimplOf(this ICustomAttributeProvider cap)
        {
            return GetRtimplFromCache(cap);
        }

        public static bool IsVarargs(this MethodBase mi)
        {
            var @params = mi.GetParameters();
            return @params.Length != 0 &&
                @params[@params.Length - 1].IsDefined(typeof(ParamArrayAttribute), true);
        }

        public static bool IsExtension(this MethodBase mi)
        {
            return mi.IsDefined(typeof(ExtensionAttribute), false);
        }

        public static Type GetScopeResolver(this ICustomAttributeProvider cap)
        {
            return cap.IsDefined(typeof(CustomScopeResolverAttribute), false) ?
                cap.GetCustomAttributes(typeof(CustomScopeResolverAttribute), false)
                .Cast<CustomScopeResolverAttribute>().Single().Type :
                typeof(DefaultScopeResolver);
        }

        public static Type GetInvocationResolver(this ICustomAttributeProvider cap)
        {
            return cap.IsDefined(typeof(CustomInvocationResolverAttribute), false) ?
                cap.GetCustomAttributes(typeof(CustomInvocationResolverAttribute), false)
                .Cast<CustomInvocationResolverAttribute>().Single().Type :
                typeof(DefaultInvocationResolver);
        }

        public static bool IsElfSerializable(this ICustomAttributeProvider cap)
        {
            return cap.IsDefined(typeof(ElfSerializableAttribute), false);
        }

        public static String ElfSerializableAs(this ICustomAttributeProvider cap)
        {
            return cap.GetCustomAttributes(typeof(ElfSerializableAttribute), false)
                .Cast<ElfSerializableAttribute>().Single().TypeToken;
        }

        public static Func<String, IElfObject> ElfDeserializer(this Type t)
        {
            var parse = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Where(
                mi => mi.Name == "Parse" &&
                mi.GetParameters().Length == 1 && 
                mi.GetParameters()[0].ParameterType == typeof(String) &&
                mi.ReturnType.MetadataToken == t.MetadataToken &&
                mi.ReturnType.Module == t.Module).SingleOrDefault();

            if (parse != null)
            {
                return s => (IElfObject)parse.Invoke(null, s.AsArray());
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(t);
                if (converter.CanConvertFrom(typeof(String)))
                {
                    return s => (IElfObject)t.FromInvariantString(s);
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool IsRthelper(this ICustomAttributeProvider cap)
        {
            return cap.IsDefined(typeof(RthelperAttribute), false);
        }
    }
}
