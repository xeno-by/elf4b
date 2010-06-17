#define DUMP_WHIT_DYNAMIC

namespace ObjectMeet.Tiller.Entities.Whit
{
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Runtime.CompilerServices;
	using DataVault.Api;
	using Internal;
	using Traits;

	public static class VaultWhit
	{
		private static readonly ModuleBuilder _moduleBuilder;
		private static readonly Dictionary<Type, ICreature> _creatures;

		static VaultWhit()
		{
			_creatures = new Dictionary<Type, ICreature>();
#if DUMP_WHIT_DYNAMIC
			var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("WhitLiteDynamic"), AssemblyBuilderAccess.RunAndSave, @"c:\");
			assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof (GeneratedCodeAttribute).GetConstructor(new Type[] {typeof (string), typeof (string)}), new object[] {MethodBase.GetCurrentMethod().DeclaringType.FullName, DateTime.Now.ToString()}));
			_moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule", "DynamicModule.dll", true);
			var saved = false;
			EventHandler saver = (sender, e) =>
				{
					if (saved) return;
					try
					{
						assemblyBuilder.Save("WhitLiteDynamic.dll");
						saved = true;
					}
					catch
					{
						Debugger.Break();
					}
				};

			AppDomain.CurrentDomain.ProcessExit += saver;
			AppDomain.CurrentDomain.DomainUnload += saver;

#else			
			_moduleBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("WhitLiteDynamic"), AssemblyBuilderAccess.Run).DefineDynamicModule("DynamicModule");
#endif
		}

		public static D EmitDelegate<D>(Action<ILGenerator> emitter)
		{
			Debug.Assert(typeof (MulticastDelegate).IsAssignableFrom(typeof (D)), "Delegate type expected");

			var info = typeof (D).GetMethod("Invoke");
			var dynamicMethod = new DynamicMethod(typeof (D).Name, MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard,
			                                      info.ReturnType, info.GetParameters().Select(x => x.ParameterType).ToArray(),
			                                      typeof (VaultWhit).Module, true);

			emitter(dynamicMethod.GetILGenerator());
			return (D) (object) dynamicMethod.CreateDelegate(typeof (D));
		}


		private static object New(Type type, IBranch branch)
		{
			ICreature result;
			if (_creatures.TryGetValue(type, out result)) return result.CreateYourself(branch);

			// TODO: implement a DAG resolution for the right property mapping
			// for the time being it's recommended to use an interface instead of abstract class
			// Debug.Assert(type.IsInterface, "This version handles interfaces only :(");
			DefaultImplementationAttribute defaultImplementationAttribute;
			if (type.IsInterface && type.HasAnnotation(out defaultImplementationAttribute))
			{
				if (_creatures.TryGetValue(defaultImplementationAttribute.Implementator, out result)) return result.CreateYourself(branch);
				if (defaultImplementationAttribute.Implementator.IsInterface) throw new BusinessRuleViolationException(4, 31);
				if (!type.IsAssignableFrom(defaultImplementationAttribute.Implementator)) throw new BusinessRuleViolationException(4, 30);
				type = defaultImplementationAttribute.Implementator;
			}

			if (type.IsSealed) throw new BusinessRuleViolationException(4, 32);

			lock (_creatures)
				if (!_creatures.TryGetValue(type, out result))
				{
					var typeBuilt = BuildType(type);
					result = (ICreature) Activator.CreateInstance(typeBuilt);
					_creatures.Add(type, result);
				}
			return result.CreateYourself(branch);
		}


		public static T New<T>(IBranch branch) where T : class
		{
			return (T) New(typeof (T), branch);
		}

		public static T New<T>() where T : class
		{
			return (T) New(typeof (T), null);
		}

		private static Type BuildType(Type type)
		{
			var originalType = type;

			DefaultImplementationAttribute defaultImplementationAttribute;
			if (type.IsInterface && type.HasAnnotation(out defaultImplementationAttribute))
			{
				type = defaultImplementationAttribute.Implementator;
				ICreature result;
				if (_creatures.TryGetValue(type, out result)) return result.GetType(); // TODO: should be checked in New method
				if (type.IsInterface) throw new BusinessRuleViolationException(4, 31);
				if (!originalType.IsAssignableFrom(type)) throw new BusinessRuleViolationException(4, 30);
			}

			if (type.GetInterfaces().Union(new[] {type}).Where(x => x.IsAnnotated<WhitIgnorableAttribute>()).Count() > 0) throw new BusinessRuleViolationException(4, 2);
			if (type.IsNotPublic && type.Assembly.FullName != _moduleBuilder.Assembly.FullName && !type.Assembly.IsAnnotated<InternalsVisibleToAttribute>(a => a.AssemblyName == _moduleBuilder.Assembly.GetName().Name))
				throw new MemberAccessException(string.Format("Type {0} is not visible for the assembly being generated. Add the following to its assembly: [assembly: InternalsVisibleTo(\"{1}\")]", type.FullName, _moduleBuilder.Assembly.GetName().Name));

			// TODO: maybe check visibility of the rest interfaces?

			var typeName = string.Format("Generated4Tiller.{0}", type.FullName.Replace('.', '_'));
			var context = new TypeBuildingContext
			              	{
			              		SourceType = type,
			              		TypeBuilder = _moduleBuilder.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed, type.IsInterface ? typeof (object) : type, new[] {typeof (ICreature)}),
			              		ShadowBuilder = _moduleBuilder.DefineType(typeName + "@Shadow", TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed, type.IsInterface ? typeof (object) : type, new[] {typeof (ICreature)}),
			              	};
			context.DefaultCtor = context.TypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
			context.ShadowDefaultCtor = context.ShadowBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
			context.IntersectWithShadowBuilder = context.TypeBuilder.DefineMethod("@Intersect", MethodAttributes.Private, null, new Type[] {context.ShadowBuilder});
			context.IntersectingField = context.TypeBuilder.DefineField("@intersecting", typeof (bool), FieldAttributes.Private);
			context.IntersectWithShadowBuilder.GetILGenerator().ldarg(0).ldc_i4(1).stfld(context.IntersectingField);

			if (type.IsInterface)
			{
				context.TypeBuilder.AddInterfaceImplementation(type);
				context.ShadowBuilder.AddInterfaceImplementation(type);
				context.MethodsLeftAbstract = new HashSet<MethodInfo>(type.GetInterfaces().Union(new[] {type}).SelectMany(x => x.GetMethods()));
			}
			else
			{
				context.MethodsLeftAbstract = new HashSet<MethodInfo>(type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.IsAbstract));
			}

			ImplementProperties(context);
			ImplementICreature(context);

			context.DefaultCtor.GetILGenerator().ret();
			context.ShadowDefaultCtor.GetILGenerator().ret();
			context.IntersectWithShadowBuilder.GetILGenerator().ldarg(0).ldc_i4(0).stfld(context.IntersectingField).ret();

			foreach (var method in context.MethodsLeftAbstract)
			{
				// generate dummy implementation for abstact methods being left, just to keep type consistency
// ReSharper disable AccessToModifiedClosure
				context.TypeBuilder.OverrideMethod(method, il => il.lddefault(method.ReturnType).ret());
				context.ShadowBuilder.OverrideMethod(method, il => il.lddefault(method.ReturnType).ret());
// ReSharper restore AccessToModifiedClosure
			}
			try
			{
				context.ShadowBuilder.CreateType();
			}
			catch (TypeLoadException oops)
			{
				if (typeof (TypeLoadException).GetField("ResourceId", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(oops).Equals(8271))
					throw new MemberAccessException(string.Format("Some of types is not visible for the assembly being generated. Add the following to all target assemblies: [assembly: InternalsVisibleTo(\"{0}\")]", _moduleBuilder.Assembly.GetName().Name));
				throw;
			}

			return context.TypeBuilder.CreateType();
		}


		private static void ImplementProperties(TypeBuildingContext context)
		{
			// here we have no idea how to generate indexers, thus placeholders will be generated below
			foreach (var map in context.SourceType.IsClass
			                    	? from prop in context.SourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			                    	  // if property by some reasons is partial abstract then we ignore the whole property and placeholders will be generated below for abstract part
			                    	  where prop.IsAbstract(true) && !prop.IsIndexer()
			                    	  group prop by new PropertySignature {Name = prop.Name, PropertyType = prop.PropertyType}
			                    	  into propmap select propmap
			                    	: from iface in context.SourceType.GetInterfaces().Union(new[] {context.SourceType}).Distinct()
			                    	  from prop in iface.GetProperties()
			                    	  where !prop.IsIndexer()
			                    	  group prop by new PropertySignature {Name = prop.Name, PropertyType = prop.PropertyType}
			                    	  into propmap select propmap)
			{
				var meta = new MetaProperty
				           	{
				           		BuildingContext = context,
				           		WhitIgnorable = false,
				           		Name = map.Key.Name,
				           		DefaultValue = null,
				           		PropertyType = map.Key.PropertyType,
				           		Siblings = map,
				           		PropertyBuilder = context.TypeBuilder.DefineProperty(map.Key.Name, PropertyAttributes.HasDefault, map.Key.PropertyType, new Type[0]),
				           		ShadowPropertyBuilder = context.ShadowBuilder.DefineProperty(map.Key.Name, PropertyAttributes.HasDefault, map.Key.PropertyType, new Type[0]),
				           	};

				UpdateMetaAndCheckConstraints(meta);
				EmitInheritCustomAttributesOnProp(meta);

				if (!meta.WhitIgnorable) EmitPropertyByTypeDiscriminator(meta);
				else EmitProperty_WayAuto(meta);

				EmitShadowProperties(meta);
				MarkMethodsOverriden(meta);
			}
		}

		private static void EmitShadowProperties(MetaProperty meta)
		{
			//if(meta.IsPrimaryKey) return;
			var flag = meta.BuildingContext.ShadowBuilder.DefineField(string.Format("_is{0}Changed", meta.Name), typeof (bool), FieldAttributes.Private);
			var field = meta.BuildingContext.ShadowBuilder.DefineField(string.Format("_{0}", meta.Name), meta.PropertyType, FieldAttributes.Private);
			var isChangedProp = meta.BuildingContext.ShadowBuilder.DefineProperty(string.Format("Is{0}Changed", meta.Name), PropertyAttributes.None, typeof (bool), new Type[0]);
			var isChanged = meta.BuildingContext.ShadowBuilder.DefineMethod("get_" + isChangedProp.Name, MethodAttributes.Public, typeof (bool), new Type[0]);
			isChangedProp.SetGetMethod(isChanged);

			Func<ILGenerator, ILGenerator> prefix = null;
			if (meta.PropertyType.IsAbstract)
			{
				prefix = il => il
				               	.ldarg(1)
				               	.@if(false, x => x.ldc_i4(4).ldc_i4(23).@throw(typeof (BusinessRuleViolationException), typeof (int), typeof (int)))
					;
			}

			isChanged.GetILGenerator().ldarg(0).ldfld(flag).ret();
			meta.ShadowGetterBuilder.GetILGenerator().ldarg(0).ldfld(field).ret();
			meta.ShadowSetterBuilder.GetILGenerator()
				.Apply(prefix)
				.ldarg(0)
				.ldfld(field)
				.ldarg(1)
				.ceq()
				.@if(false, x => x
				                 	.ldarg(0)
				                 	.ldc_i4(1)
				                 	.stfld(flag)
				                 	.ldarg(0)
				                 	.ldarg(1)
				                 	.stfld(field)
				)
				.ret();

			meta.BuildingContext.IntersectWithShadowBuilder.GetILGenerator()
				.ldarg(1)
				.call(isChanged)
				.@if(true, x => x
				                	.ldarg(0)
				                	.ldarg(1)
				                	.callvirt(meta.ShadowGetterBuilder)
				                	.callvirt(meta.SetterBuilder)
				);
		}

		private static void MarkMethodsOverriden(MetaProperty meta)
		{
			foreach (var info in meta.Siblings)
			{
				if (info.CanRead)
				{
					meta.BuildingContext.TypeBuilder.DefineMethodOverride(meta.GetterBuilder, info.GetGetMethod(true));
					meta.BuildingContext.ShadowBuilder.DefineMethodOverride(meta.ShadowGetterBuilder, info.GetGetMethod(true));
					meta.BuildingContext.MethodsLeftAbstract.Remove(info.GetGetMethod(true));
				}
				if (info.CanWrite)
				{
					meta.BuildingContext.TypeBuilder.DefineMethodOverride(meta.SetterBuilder, info.GetSetMethod(true));
					meta.BuildingContext.ShadowBuilder.DefineMethodOverride(meta.ShadowSetterBuilder, info.GetSetMethod(true));
					meta.BuildingContext.MethodsLeftAbstract.Remove(info.GetSetMethod(true));
				}
			}
		}

		private static void EmitPropertyByTypeDiscriminator(MetaProperty meta)
		{
			if (meta.PropertyType.HasGenericTypeDefinition(typeof (IQueryable<>)))
			{
				EmitProperty_IQueryable(meta);
				return;
			}
			if (meta.PropertyType.HasGenericTypeDefinition(typeof (IEnumerable<>)) && meta.PropertyType != typeof (string))
			{
				EmitProperty_IEnumerable(meta);
				return;
			}
			if (meta.IsPrimaryKey)
			{
				if (!(meta.PropertyType.IsPrimitive || typeof (string) == meta.PropertyType || typeof (Guid) == meta.PropertyType)) throw new BusinessRuleViolationException(4, 6);
				EmitProperty_WayPrimaryKey(meta);
				return;
			}

			if (meta.PropertyType.IsAbstract)
			{
				EmitProperty_AbstractType(meta);
				return;
			}

			EmitProperty_WayDefault(meta);
		}

		private static void EmitProperty_AbstractType(MetaProperty meta)
		{
			meta.FieldBuilder = meta.BuildingContext.TypeBuilder.DefineField("_" + (meta.BuildingContext.Counter++), meta.PropertyType, FieldAttributes.Private);
			meta.GetterBuilder.GetILGenerator()
				.ldarg(0)
				.ldfld(meta.FieldBuilder)
				.@if(false, il => il
				                  	.ldarg(0)
				                  	.ldarg(0)
				                  	.callvirt(FromLambda.Getter<ICreature, IBranch>(c => c.Model))
				                  	.ldstr(meta.Name)
				                  	.convert(typeof (string), typeof (VPath))
				                  	.callvirt(FromLambda.Method<IBranch, VPath, IBranch>((b, v) => b.GetOrCreateBranch(v)))
				                  	.call(FromLambda.Method<IBranch, object>(m => New<object>(m)).GetGenericMethodDefinition().MakeGenericMethod(meta.PropertyType))
				                  	.stfld(meta.FieldBuilder)
				)
				.ldarg(0)
				.ldfld(meta.FieldBuilder)
				.ret();

			Func<ILGenerator, ILGenerator> onChangingIL = null, onChangedIL = null;

			if (meta.BuildingContext.SourceType.IsClass)
			{
				MethodInfo onChanging, onChanged;
				if (meta.BuildingContext.SourceType.HasMethod(out onChanging, MethodAttributes.Family, typeof (bool), string.Format("On{0}Changing", meta.Name), meta.PropertyType))
				{
					onChangingIL = il => il
					                     	.ldarg(0)
					                     	.ldfld(meta.BuildingContext.IntersectingField)
					                     	.@if(false, x => x
					                     	                 	.ldarg(0)
					                     	                 	.ldarg(1)
					                     	                 	.call(onChanging)
					                     	                 	.@if(true, y => y.ret()
					                     	                 	)
					                     	);
				}
				if (meta.BuildingContext.SourceType.HasMethod(out onChanged, MethodAttributes.Family, typeof (void), string.Format("On{0}Changed", meta.Name)))
					throw new BusinessRuleViolationException(4, 22);
			}

			meta.SetterBuilder.GetILGenerator().Apply(onChangingIL).ldc_i4(4).ldc_i4(21).@throw(typeof (BusinessRuleViolationException), typeof (int), typeof (int));
		}

		private static void EmitProperty_WayAuto(MetaProperty meta)
		{
			meta.FieldBuilder = meta.BuildingContext.TypeBuilder.DefineField("_" + (meta.BuildingContext.Counter++), meta.PropertyType, FieldAttributes.Private);
			meta.GetterBuilder.GetILGenerator().ldarg(0).ldfld(meta.FieldBuilder).ret();

			Func<ILGenerator, ILGenerator> onChangingIL = null, onChangedIL = null;

			if (meta.BuildingContext.SourceType.IsClass)
			{
				MethodInfo onChanging, onChanged;
				if (meta.BuildingContext.SourceType.HasMethod(out onChanging, MethodAttributes.Family, typeof (bool), string.Format("On{0}Changing", meta.Name), meta.PropertyType))
				{
					onChangingIL = il => il
					                     	.ldarg(0)
					                     	.ldfld(meta.BuildingContext.IntersectingField)
					                     	.@if(false, x => x
					                     	                 	.ldarg(0)
					                     	                 	.ldarg(1)
					                     	                 	.call(onChanging)
					                     	                 	.@if(true, y => y.ret()
					                     	                 	)
					                     	);
				}
				if (meta.BuildingContext.SourceType.HasMethod(out onChanged, MethodAttributes.Family, typeof (void), string.Format("On{0}Changed", meta.Name)))
				{
					onChangedIL = il => il
					                    	.ldarg(0)
					                    	.ldfld(meta.BuildingContext.IntersectingField)
					                    	.@if(false, x => x
					                    	                 	.ldarg(0)
					                    	                 	.call(onChanged)
					                    	)
						;
				}
			}

			meta.SetterBuilder.GetILGenerator().Apply(onChangingIL).ldarg(0).ldarg(1).stfld(meta.FieldBuilder).Apply(onChangedIL).ret();
		}

		private static void EmitProperty_IEnumerable(MetaProperty meta)
		{
			meta.ElementType = meta.PropertyType.GetGenericArguments()[0];
			meta.FieldBuilder = meta.BuildingContext.TypeBuilder.DefineField("_" + (meta.BuildingContext.Counter++), typeof (List<>).MakeGenericType(meta.ElementType), FieldAttributes.Private);
			// BR: no setters for IEnumerable<?>
			meta.SetterBuilder.GetILGenerator()
				.ldc_i4(4)
				.ldc_i4(4)
				.@throw(typeof (BusinessRuleViolationException), typeof (int), typeof (int));

			throw new NotImplementedException("IEnumerable<?> will be implemented in build 20, use IQueryable<?> instead");
		}

		private static void EmitProperty_WayDefault(MetaProperty meta)
		{
			var defaultValue = meta.DefaultValue == null ? null : meta.DefaultValue.ToString();

			meta.GetterBuilder.GetILGenerator()
				.ldarg(0)
				.callvirt(FromLambda.Getter<ICreature, IBranch>(c => c.Model))
				// .callvirt<ICreature, IBranch>( c => c.Model )
				.ldstr(meta.Name)
				.convert(typeof (string), typeof (VPath))
				.ldstr(defaultValue)
				.call(FromLambda.Method<IBranch, VPath, string, IValue>((b, n, v) => b.GetOrCreateValue(n, v)))
				.callvirt(FromLambda.Getter<IValue, string>(v => v.ContentString))
				.convert(typeof (string), meta.PropertyType)
				.ret();

			Func<ILGenerator, ILGenerator> onChangingIL = null, onChangedIL = null;

			if (meta.BuildingContext.SourceType.IsClass)
			{
				MethodInfo onChanging, onChanged;
				if (meta.BuildingContext.SourceType.HasMethod(out onChanging, MethodAttributes.Family, typeof (bool), string.Format("On{0}Changing", meta.Name), meta.PropertyType))
				{
					onChangingIL = il => il
					                     	.ldarg(0)
					                     	.ldfld(meta.BuildingContext.IntersectingField)
					                     	.@if(false, x => x
					                     	                 	.ldarg(0)
					                     	                 	.ldarg(1)
					                     	                 	.call(onChanging)
					                     	                 	.@if(true, y => y.ret()
					                     	                 	)
					                     	);
				}
				if (meta.BuildingContext.SourceType.HasMethod(out onChanged, MethodAttributes.Family, typeof (void), string.Format("On{0}Changed", meta.Name)))
				{
					onChangedIL = il => il
					                    	.ldarg(0)
					                    	.ldfld(meta.BuildingContext.IntersectingField)
					                    	.@if(false, x => x
					                    	                 	.ldarg(0)
					                    	                 	.call(onChanged)
					                    	)
						;
				}
			}

			meta.SetterBuilder.GetILGenerator()
				.Apply(onChangingIL) // warning .ret may be over here: don't use .try
				.ldarg(0)
				.callvirt(FromLambda.Getter<ICreature, IBranch>(c => c.Model))
				.ldstr(meta.Name)
				.convert(typeof (string), typeof (VPath))
				.ldstr(defaultValue)
				.call(FromLambda.Method<IBranch, VPath, string, IValue>((b, n, v) => b.GetOrCreateValue(n, v)))
				.ldarg(1)
				.convert(meta.PropertyType, typeof (string))
				.call(FromLambda.Method<IValue, string>((v, s) => v.UpdateContent(s)))
				.pop()
				.Apply(onChangedIL)
				.ret();
		}

		private static void EmitProperty_WayPrimaryKey(MetaProperty meta)
		{
			// BR: no setters for primary keys
			// assume all requirements for PK are met and validated earley
			meta.SetterBuilder.GetILGenerator()
				.ldc_i4(4)
				.ldc_i4(1)
				.@throw(typeof (BusinessRuleViolationException), typeof (int), typeof (int));

			meta.GetterBuilder.GetILGenerator()
				.ldarg(0)
				.callvirt(typeof (ICreature).GetProperty("Model").GetGetMethod())
				.callvirt(typeof (IElement).GetProperty("Name").GetGetMethod())
				.convert(typeof (string), meta.PropertyType)
				.ret();
		}

		private static void EmitProperty_IQueryable(MetaProperty meta)
		{
			meta.ElementType = meta.PropertyType.GetGenericArguments()[0];
			// BR: no setters for IQueryable<?>
			meta.SetterBuilder.GetILGenerator()
				.ldc_i4(4)
				.ldc_i4(3)
				.@throw(typeof (BusinessRuleViolationException), typeof (int), typeof (int));

			var itemType = meta.ElementType;

			DefaultImplementationAttribute defaultImplementationAttribute;
			if (itemType.HasAnnotation(out defaultImplementationAttribute)) itemType = defaultImplementationAttribute.Implementator;

			var parents = from prop in itemType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			              where prop.IsAnnotated<MetaInfoAttribute>(x => x.IsParentProperty) && !prop.IsIndexer() &&
			                    prop.CanWrite && prop.PropertyType.IsAssignableFrom(meta.BuildingContext.SourceType)
			              select prop;

			if (parents.Count() > 0)
			{
				if (parents.Count() > 1) throw new BusinessRuleViolationException(4, 5);
				EmitProperty_IQueryable_WithParentInit(meta, parents.First().GetSetMethod(true), itemType);
				return;
			}

			LocalBuilder parameterExpression;
			LocalBuilder parameters;
			LocalBuilder expressions;

			// produces: return
			//	from branch in Model.GetOrCreateBranch(name).GetBranches().AsQueryable()
			//	select Whit.New<TElement>(branch);
			// ---> may be "optimized" to
			// ---> ICreature creature = Whit.New<TElement>(null) ...
			// ---> ... select creature.CreateYourSelf(branch);

			meta.GetterBuilder.GetILGenerator()
				.DefineLocal(typeof (ParameterExpression), out parameterExpression)
				.DefineLocal(typeof (ParameterExpression[]), out parameters)
				.DefineLocal(typeof (Expression[]), out expressions)
				.ldarg(0)
				.callvirt(FromLambda.Getter<ICreature, IBranch>(c => c.Model))
				.ldstr(meta.Name)
				.convert(typeof (string), typeof (VPath))
				.callvirt(FromLambda.Method<IBranch, VPath, IBranch>((b, v) => b.GetOrCreateBranch(v)))
				.callvirt(FromLambda.Method<IBranch, IEnumerable<IBranch>>(b => b.GetBranches()))
				.call(FromLambda.Method<IEnumerable<IBranch>, IQueryable<IBranch>>(x => x.AsQueryable()))
				.ld_type_info(typeof (IBranch))
				.ldstr("branch")
				.call(FromLambda.Method<Type, string, ParameterExpression>((t, n) => Expression.Parameter(t, n)))
				.stloc(parameterExpression)
				.ldnull() // Whit.New<?> is static
				.ld_method_info(typeof (VaultWhit).GetMethod("New", new[] {typeof (IBranch)}).MakeGenericMethod(meta.ElementType))
				.ldc_i4(1)
				.newarr(typeof (Expression))
				.stloc(expressions)
				.ldloc(expressions)
				.ldc_i4(0)
				.ldloc(parameterExpression)
				.stelem_ref()
				.ldloc(expressions)
				.call(FromLambda.Method<Expression, MethodInfo, Expression[], MethodCallExpression>((e, m, es) => Expression.Call(e, m, es)))
				.ldc_i4(1)
				.newarr(typeof (ParameterExpression))
				.stloc(parameters)
				.ldloc(parameters)
				.ldc_i4(0)
				.ldloc(parameterExpression)
				.stelem_ref()
				.ldloc(parameters)
				.call(FromLambda.Method<Expression, ParameterExpression[], Expression<int>>((e, p) => Expression.Lambda<int>(e, p)).GetGenericMethodDefinition().MakeGenericMethod(typeof (Func<,>).MakeGenericType(typeof (IBranch), meta.ElementType)))
				.call(FromLambda.Method<IQueryable<int>, Expression<Func<int, int>>, IQueryable<int>>((q, e) => q.Select(e)).GetGenericMethodDefinition().MakeGenericMethod(typeof (IBranch), meta.ElementType))
				.ret();
		}

		private static void EmitProperty_IQueryable_WithParentInit(MetaProperty meta, MethodInfo parentSetter, Type itemType)
		{
			var createChild = meta.BuildingContext.TypeBuilder.DefineMethod(string.Format("@Create{0}Child", meta.Name), MethodAttributes.Private, meta.ElementType, new[] {typeof (IBranch)});
			LocalBuilder child;
			createChild.GetILGenerator()
				.DefineLocal(itemType, out child)
				.ldarg(1)
				.call(FromLambda.Method<IBranch, object>(b => New<object>(b)).GetGenericMethodDefinition().MakeGenericMethod(itemType))
				.stloc(child)
				.ldloc(child)
				.ldarg(0)
				.callvirt(parentSetter)
				.ldloc(child)
				.ret()
				;

			LocalBuilder parameterExpression;
			LocalBuilder parameters;
			LocalBuilder expressions;

			// produces: return
			//	from branch in Model.GetOrCreateBranch(name).GetBranches().AsQueryable()
			//	select this.CreateChild(branch);

			meta.GetterBuilder.GetILGenerator()
				.DefineLocal(typeof (ParameterExpression), out parameterExpression)
				.DefineLocal(typeof (ParameterExpression[]), out parameters)
				.DefineLocal(typeof (Expression[]), out expressions)
				.ldarg(0)
				.callvirt(FromLambda.Getter<ICreature, IBranch>(c => c.Model))
				.ldstr(meta.Name)
				.convert(typeof (string), typeof (VPath))
				.callvirt(FromLambda.Method<IBranch, VPath, IBranch>((b, v) => b.GetOrCreateBranch(v)))
				.callvirt(FromLambda.Method<IBranch, IEnumerable<IBranch>>(b => b.GetBranches()))
				.call(FromLambda.Method<IEnumerable<IBranch>, IQueryable<IBranch>>(x => x.AsQueryable()))
				.ld_type_info(typeof (IBranch))
				.ldstr("branch")
				.call(FromLambda.Method<Type, string, ParameterExpression>((t, n) => Expression.Parameter(t, n)))
				.stloc(parameterExpression)
				.ldarg(0)
				.box(meta.BuildingContext.TypeBuilder)
				.ld_type_info(meta.BuildingContext.TypeBuilder)
				.call(FromLambda.Method<object, Type, ConstantExpression>((o, t) => Expression.Constant(o, t)))
				.ld_method_info(createChild)
				.ldc_i4(1)
				.newarr(typeof (Expression))
				.stloc(expressions)
				.ldloc(expressions)
				.ldc_i4(0)
				.ldloc(parameterExpression)
				.stelem_ref()
				.ldloc(expressions)
				.call(FromLambda.Method<Expression, MethodInfo, Expression[], MethodCallExpression>((e, m, es) => Expression.Call(e, m, es)))
				.ldc_i4(1)
				.newarr(typeof (ParameterExpression))
				.stloc(parameters)
				.ldloc(parameters)
				.ldc_i4(0)
				.ldloc(parameterExpression)
				.stelem_ref()
				.ldloc(parameters)
				.call(FromLambda.Method<Expression, ParameterExpression[], Expression<int>>((e, p) => Expression.Lambda<int>(e, p)).GetGenericMethodDefinition().MakeGenericMethod(typeof (Func<,>).MakeGenericType(typeof (IBranch), meta.ElementType)))
				.call(FromLambda.Method<IQueryable<int>, Expression<Func<int, int>>, IQueryable<int>>((q, e) => q.Select(e)).GetGenericMethodDefinition().MakeGenericMethod(typeof (IBranch), meta.ElementType))
				.ret();
		}

		private static void UpdateMetaAndCheckConstraints(MetaProperty meta)
		{
			foreach (var propertyInfo in meta.Siblings)
			{
				// BR: if one of props is "whit ignorable" we should ignore the whole group
				// note "ignoring" means the default implementation of auto property
				meta.WhitIgnorable |= propertyInfo.IsAnnotated<WhitIgnorableAttribute>();
				if (meta.WhitIgnorable) break;

				MetaInfoAttribute metaInfoAttribute;
				if (!propertyInfo.HasAnnotation(out metaInfoAttribute)) continue;

				// BR: if one of props has alias we should use it as a name of the whole group
				// note the very last defined alias will be used
				if (!string.IsNullOrEmpty(metaInfoAttribute.Alias))
					meta.Name = metaInfoAttribute.Alias;

				// BR: if one of props has default value initialized we should use it as a default value of the whole group
				// note the very last defined default value will be used
				if (metaInfoAttribute.IsDefaultValueInitialized)
					meta.DefaultValue = metaInfoAttribute.DefaultValue;

				meta.IsPrimaryKey = metaInfoAttribute.IsPrimaryKey;
				// TODO: check out if IsPrimaryKey is drawn once and on the right property
			}

			if (meta.DefaultValue == null) UpdateDefaultValueForKnownTypes(meta);

			meta.GetterBuilder = meta.BuildingContext.TypeBuilder.DefineMethod("get_" + meta.Name, MethodAttributes.Final | MethodAttributes.Public | MethodAttributes.Virtual, meta.PropertyType, new Type[0]);
			meta.SetterBuilder = meta.BuildingContext.TypeBuilder.DefineMethod("set_" + meta.Name, MethodAttributes.Final | MethodAttributes.Public | MethodAttributes.Virtual, null, new[] {meta.PropertyType});
			meta.PropertyBuilder.SetGetMethod(meta.GetterBuilder);
			meta.PropertyBuilder.SetSetMethod(meta.SetterBuilder);

			meta.ShadowGetterBuilder = meta.BuildingContext.ShadowBuilder.DefineMethod("get_" + meta.Name, MethodAttributes.Final | MethodAttributes.Public | MethodAttributes.Virtual, meta.PropertyType, new Type[0]);
			meta.ShadowSetterBuilder = meta.BuildingContext.ShadowBuilder.DefineMethod("set_" + meta.Name, MethodAttributes.Final | MethodAttributes.Public | MethodAttributes.Virtual, null, new[] {meta.PropertyType});
			meta.ShadowPropertyBuilder.SetGetMethod(meta.ShadowGetterBuilder);
			meta.ShadowPropertyBuilder.SetSetMethod(meta.ShadowSetterBuilder);
		}

		private static void UpdateDefaultValueForKnownTypes(MetaProperty meta)
		{
			if (meta.PropertyType.IsEnum)
			{
				meta.DefaultValue = "0"; // assume 0 is good as a default value for enums
				return;
			}

			if (Known<int>(meta, 0)) return;
			if (Known<bool>(meta, false)) return;
			if (Known<long>(meta, 0L)) return;
			if (Known<decimal>(meta, 0)) return;
			if (Known<Guid>(meta, Guid.Empty)) return;
			if (Known<Version>(meta, new Version(0, 0, 0, 0))) return;
			if (Known<DateTime>(meta, DateTime.MinValue)) return; // maybe DateTime.Now ?

			// TODO: add exotic somewhen: sbyte, byte, char ...
		}

		private static bool Known<T>(MetaProperty meta, object value)
		{
			if (value == null) throw new ArgumentNullException("value");
			if (meta.PropertyType == typeof (T))
			{
				try
				{
					meta.DefaultValue = (T) value;
				}
				catch (InvalidCastException)
				{
					throw new ArgumentOutOfRangeException("value", value, string.Format("Unable to cast {0} to {1} for property {3}: {2}", value.GetType().Name, typeof (T).Name, meta.PropertyType.Name, meta.Name));
				}
				return true;
			}
			return false;
		}

		private static void EmitInheritCustomAttributesOnProp(MetaProperty meta)
		{
			foreach (var data in from prop in meta.Siblings
			                     from a in CustomAttributeData.GetCustomAttributes(prop)
			                     let type = a.Constructor.DeclaringType
			                     // avoid generation of "generation" attributes :)
			                     where !typeof (WhitAnnotation).IsAssignableFrom(type)
			                     group a by type
			                     into attmap select attmap)
			{
				AttributeUsageAttribute attributeUsageAttribute;
				if (data.Key.HasAnnotation(out attributeUsageAttribute) && attributeUsageAttribute.AllowMultiple)
					foreach (var ad in data)
						meta.PropertyBuilder.SetCustomAttribute(ad.ToCustomAttributeBuilder());
				else
					// BR: just one attribute will be applied even if we have more :)
					// note the very last defined attribute will be used
					meta.PropertyBuilder.SetCustomAttribute(data.Last().ToCustomAttributeBuilder());
			}
		}

		private static CustomAttributeBuilder ToCustomAttributeBuilder(this CustomAttributeData data)
		{
			return new CustomAttributeBuilder(
				data.Constructor,
				data.ConstructorArguments.Select(x => x.Value).ToArray(),
				data.NamedArguments.Where(x => x.MemberInfo is PropertyInfo).Select(x => x.MemberInfo as PropertyInfo).ToArray(),
				data.NamedArguments.Where(x => x.MemberInfo is PropertyInfo).Select(x => x.TypedValue.Value).ToArray(),
				data.NamedArguments.Where(x => x.MemberInfo is FieldInfo).Select(x => x.MemberInfo as FieldInfo).ToArray(),
				data.NamedArguments.Where(x => x.MemberInfo is FieldInfo).Select(x => x.TypedValue.Value).ToArray()
				);
		}

		private static void ImplementICreature(TypeBuildingContext context)
		{
			var createYourselfMeth = FromLambda.Method<ICreature, IBranch, ICreature>((c, b) => c.CreateYourself(b));
			var intersectMeth = FromLambda.Method<ICreature, ICreature>((c, s) => c.Intersect(s));

			var _model = context.TypeBuilder.DefineField("@_model", typeof (IBranch), FieldAttributes.Private);
			var map = new Dictionary<MethodInfo, MethodBuilder>();
			var mapShadow = new Dictionary<MethodInfo, MethodBuilder>();

			context.TypeBuilder.OverrideMethod(FromLambda.Getter<ICreature, bool>(c => c.IsShadow), il => il.ldc_i4(0).ret(), map);
			context.TypeBuilder.OverrideMethod(FromLambda.Getter<ICreature, IBranch>(c => c.Model), il => il.ldarg(0).ldfld(_model).ret(), map);
			context.TypeBuilder.OverrideMethod(FromLambda.Setter<ICreature, IBranch>(c => c.Model),
			                                   il => il
			                                         	.ldarg(1)
			                                         	.@if(false, x => x
			                                         	                 	.ldc_i4(4)
			                                         	                 	.ldc_i4(20)
			                                         	                 	.@throw(typeof (BusinessRuleViolationException), typeof (int), typeof (int))
			                                         	)
			                                         	.ldarg(0)
			                                         	.ldarg(1)
			                                         	.stfld(_model)
			                                         	.ret(), map);

			context.TypeBuilder.OverrideMethod(createYourselfMeth,
			                                   il => il
			                                         	.ldarg(1)
			                                         	.@if(false,
			                                         	     @if => @if
			                                         	            	.newobj(context.ShadowDefaultCtor),
			                                         	     @else => @else
			                                         	              	.newobj(context.DefaultCtor)
			                                         	              	.dup()
			                                         	              	.ldarg(1)
			                                         	              	.callvirt(FromLambda.Setter<ICreature, IBranch>(c => c.Model))
			                                         	)
			                                         	.ret(), map);
			context.TypeBuilder.OverrideMethod(intersectMeth,
			                                   il => il
			                                         	.ldarg(1)
			                                         	.isinst(context.ShadowBuilder)
			                                         	.@if(true, x => x
			                                         	                	.ldarg(0)
			                                         	                	.ldarg(1)
			                                         	                	.castclass(context.ShadowBuilder)
			                                         	                	.call(context.IntersectWithShadowBuilder)
			                                         	)
			                                         	.ret(), map);

			context.ShadowBuilder.OverrideMethod(FromLambda.Getter<ICreature, bool>(c => c.IsShadow), il => il.ldc_i4(1).ret(), mapShadow);
			context.ShadowBuilder.OverrideMethod(FromLambda.Getter<ICreature, IBranch>(c => c.Model), il => il.ldnull().ret(), mapShadow);
			context.ShadowBuilder.OverrideMethod(FromLambda.Setter<ICreature, IBranch>(c => c.Model), il => il.ldc_i4(4).ldc_i4(10).@throw(typeof (BusinessRuleViolationException), typeof (int), typeof (int)), mapShadow);
			context.ShadowBuilder.OverrideMethod(createYourselfMeth,
			                                     il => il
			                                           	.ldarg(1)
			                                           	.@if(false,
			                                           	     @if => @if
			                                           	            	.newobj(context.ShadowDefaultCtor),
			                                           	     @else => @else
			                                           	              	.newobj(context.DefaultCtor)
			                                           	              	.dup()
			                                           	              	.ldarg(1)
			                                           	              	.callvirt(FromLambda.Setter<ICreature, IBranch>(c => c.Model))
			                                           	)
			                                           	.ret(), mapShadow);
			;
			context.ShadowBuilder.OverrideMethod(intersectMeth, il => il.ldc_i4(4).ldc_i4(11).@throw(typeof (BusinessRuleViolationException), typeof (int), typeof (int)), mapShadow);

			if (context.SourceType.IsClass)
			{
				foreach (var method in context.SourceType.GetMethods().Where(x => x.IsAbstract && context.MethodsLeftAbstract.Contains(x)))
				{
					var creatured = typeof (ICreature).GetMethod(method.Name);
					if (creatured != null && method.IsInvariantTo(creatured))
					{
						context.TypeBuilder.OverrideMethod(method, x => x.ld_args(creatured.GetParameters().Length + 1 /* because isn't static */).callvirt(map[creatured] /* .callvirt will decide whether use .call or .callvirt*/).ret());
						context.ShadowBuilder.OverrideMethod(method, x => x.ld_args(creatured.GetParameters().Length + 1).callvirt(mapShadow[creatured]).ret());
					}
					else
					{
						// ReSharper disable AccessToModifiedClosure
						context.TypeBuilder.OverrideMethod(method, x => x.lddefault(method.ReturnType).ret());
						context.ShadowBuilder.OverrideMethod(method, x => x.lddefault(method.ReturnType).ret());
						// ReSharper restore AccessToModifiedClosure
					}
					context.MethodsLeftAbstract.Remove(method);
				}
			}
			else
				foreach (var iamtoo in from iface in context.SourceType.GetAllInterfaces()
				                       where iface.IsAnnotated<IamTooCreature>()
				                       select iface)
				{
					context.TypeBuilder.AddInterfaceImplementation(iamtoo);
					context.ShadowBuilder.AddInterfaceImplementation(iamtoo);
					foreach (var method in iamtoo.GetMethods())
					{
						var creatured = typeof (ICreature).GetMethod(method.Name);
						if (creatured != null && method.IsInvariantTo(creatured))
						{
							context.TypeBuilder.OverrideMethod(method, x => x.ld_args(map[creatured].GetParameters().Length + 1 /* because isn't static */).callvirt(map[creatured] /* .callvirt will decide whether use .call or .callvirt*/).ret());
							context.TypeBuilder.OverrideMethod(method, x => x.ld_args(mapShadow[creatured].GetParameters().Length + 1).callvirt(mapShadow[creatured]).ret());
						}
						else
						{
// ReSharper disable AccessToModifiedClosure
							context.TypeBuilder.OverrideMethod(method, x => x.lddefault(method.ReturnType).ret());
							context.ShadowBuilder.OverrideMethod(method, x => x.lddefault(method.ReturnType).ret());
// ReSharper restore AccessToModifiedClosure
						}
					}
				}
		}
	}
}