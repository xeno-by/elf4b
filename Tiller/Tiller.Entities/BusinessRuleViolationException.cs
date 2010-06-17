namespace ObjectMeet.Tiller.Entities
{
	using System;
	using System.Collections.Generic;

	public class BusinessRuleViolationException : ApplicationException
	{
		private static readonly IDictionary<string, string> _brs;

		static BusinessRuleViolationException()
		{
			_brs = new Dictionary<string, string>();
			// 4 - Generating and Shadowing
			_brs[""] = "";
			_brs["4.1"] = "Direct setter for Primary Key property is forbidden. Use the corresponding service instead";
			_brs["4.2"] = "Creature declaration neither implements interface nor extends class marked by WhitIgnorableAttribute";
			_brs["4.3"] = "Direct setter for IQueryable<?> properties is forbidden. Use the corresponding service instead";
			_brs["4.4"] = "Direct setter for IEnumerable<?> properties is forbidden. Use the corresponding service instead";
			_brs["4.5"] = "There is only one property having IsParentProperty=true for each parent type in creature definition";
			_brs["4.6"] = "Primary Key is a primitive value, or string, or Guid";
			_brs["4.10"] = "Shadow has no model";
			_brs["4.11"] = "Shadow doesn't dominate in intersection";
			_brs["4.20"] = "Creature doesn't accept null as a model";
			_brs["4.21"] = "Creature has no setter in case of 1..1 relationship to another creature or the corresponding OnXXXChanging is called to allow to do some setup, but should return true";
			_brs["4.22"] = "Creature has no the corresponding OnXXXChanged method in case of 1..1 relationship to another creature (see rule #4.21)";
			_brs["4.23"] = "The '1..1' relationship means non null values on both sides";
			_brs["4.30"] = "Default implementator implements interface having the corresponding attribute";
			_brs["4.31"] = "Default implementator is a class";
			_brs["4.32"] = "Creature is not a final class";

			// 8 - Loading and convertation
			_brs["8.10"] = "Converter has access to IScenarioService implementation";

			// 16 - Client Application behavior
			_brs["16.1"] = "The client application should call the corresponding 'Save As' command for the very first revision of document file instead of 'Save'";
		}

		private static string BuildMessageByBrid(string brid)
		{
			if (string.IsNullOrEmpty(brid) || !_brs.ContainsKey(brid)) return string.Format("Unknown business rule #{0} violation is detected", brid);
			return string.Format("Violation of business rule #{0} is detected. Rule text:{1}\"{2}.\"", brid, Environment.NewLine, _brs[brid]);
		}

		private BusinessRuleViolationException(Version brid)
			: base(BuildMessageByBrid(brid.ToString()))
		{
			BusinessRuleId = brid.ToString();
			BusinessRuleText = BuildMessageByBrid(BusinessRuleId);
		}

		public string BusinessRuleId { get; private set; }

		public string BusinessRuleText { get; private set; }

		public BusinessRuleViolationException(int majorLevel, int minorLevel, int level3, int level4) :
			this(new Version(majorLevel, minorLevel, level3, level4))
		{
		}

		public BusinessRuleViolationException(int majorLevel, int minorLevel, int level3)
			: this(new Version(majorLevel, minorLevel, level3))
		{
		}

		public BusinessRuleViolationException(int majorLevel, int minorLevel)
			: this(new Version(majorLevel, minorLevel))
		{
		}
	}

	public class ActionCancelledException : ApplicationException
	{
	}
}