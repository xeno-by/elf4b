namespace ObjectMeet.Tiller.Entities.Service
{
	using System;
	using System.Linq;
	using System.Reflection;
	using Api;

	public class ModelConvertationService
	{

		public IQueryable<IModelConverter> Converters
		{
			get
			{
				return (from type in MethodBase.GetCurrentMethod().DeclaringType.Assembly.GetTypes()
				        where type.IsClass && !type.IsAbstract && typeof (IModelConverter).IsAssignableFrom(type)
				        select Activator.CreateInstance(type) as IModelConverter).AsQueryable();
			}
		}
	}
}