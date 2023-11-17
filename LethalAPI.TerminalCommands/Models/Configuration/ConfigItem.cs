using System;
using System.Reflection;
using LethalAPI.TerminalCommands.Attributes;

namespace LethalAPI.TerminalCommands.Models.Configuration
{
	public class ConfigItem
	{
		public string Name { get; }

		public string Description { get; }

		public PropertyInfo Property { get; }

		public PersistType PersistType { get; }

		public object Instance { get; }

		public Type Type => Property.PropertyType;

		public AccessControlAttribute AccessControl { get; }

		public ConfigItem(string name, string description, PropertyInfo property, PersistType persistType, object instance, AccessControlAttribute accessControl = null)
		{
			Name = name;
			PersistType = persistType;
			Instance = instance;
			Description = description;
			AccessControl = accessControl;
			Property = property;
		}

		public bool CheckAccess()
		{
			if (AccessControl != null)
			{
				return AccessControl.CheckAllowed();
			}
			return true;
		}

		public string GetValue()
		{
			return Property.GetValue(Instance)?.ToString();
		}

		public bool TrySetValue(string value)
		{
			if (StringConverter.TryConvert(value, Type, out var result))
			{
				Property.SetValue(Instance, result);
				return true;
			}

			return false;
		}
	}
}
