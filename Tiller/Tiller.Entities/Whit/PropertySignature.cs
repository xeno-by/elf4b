namespace ObjectMeet.Tiller.Entities.Whit
{
	using System;

	internal class PropertySignature
	{
		public string Name { get; set; }
		public Type PropertyType { get; set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (PropertySignature)) return false;
			return Equals((PropertySignature) obj);
		}

		public bool Equals(PropertySignature obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj.Name, Name) && Equals(obj.PropertyType, PropertyType);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (PropertyType != null ? PropertyType.GetHashCode() : 0);
			}
		}
	}
}