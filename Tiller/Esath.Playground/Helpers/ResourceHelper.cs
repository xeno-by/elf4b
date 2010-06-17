using System;
using System.IO;
using System.Reflection;
using DataVault.Core.Api;
using DataVault.Core.Helpers;

namespace Esath.Playground.Helpers
{
    public static class ResourceHelper
    {
        public static void ExtractResource(this String fullName)
        {
            using (var res = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName))
            {
                using (var onDisk = File.OpenWrite(fullName))
                {
                    var ms = (MemoryStream)res.CacheInMemory();
                    ms.WriteTo(onDisk);
                }
            }
        }
        public static IVault ExtractAndOpenVault(this String fullName)
        {
            using (var res = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName))
            {
                using (var onDisk = File.OpenWrite(fullName))
                {
                    var ms = (MemoryStream)res.CacheInMemory();
                    ms.WriteTo(onDisk);
                }

                return VaultApi.OpenZip(fullName);
            }
        }
    }
}
