using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LethalAPI.TerminalCommands.Attributes;

namespace LethalAPI.TerminalCommands.Models.Configuration
{
	public class ConfigNode
	{
		public string Name { get; }

		public string Description { get; }

		public AccessControlAttribute AccessControl { get; }

		public List<ConfigNode> Children { get; } = new List<ConfigNode>();

		public List<ConfigItem> Options { get; } = new List<ConfigItem>();

		public ConfigNode(string name, string description, AccessControlAttribute accessControl = null)
		{
			Name = name;
			Description = description;
			AccessControl = accessControl;
		}

		public bool CheckAccess()
		{
			if (AccessControl != null)
			{
				return AccessControl.CheckAllowed();
			}
			return true;
		}

		public ConfigNode GetOrCreateChild(string name, string description, AccessControlAttribute accessControl = null)
		{
			lock (Children)
			{
				var existing = Children.FirstOrDefault(x => x.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase));

				if (existing != null)
				{
					return existing;
				}

				existing = new ConfigNode(name, description, accessControl);

				Children.Add(existing);

				return existing;
			}
		}

		public void CreateConfigItem(string name, string description, PropertyInfo property, object instance, PersistType persist)
		{
			var newNode = new ConfigItem(name, description, property, persist, instance);

			lock (Options)
			{
				Options.Add(newNode);
			}
		}

		public bool TryGetChild(string name, out ConfigNode node)
		{
			lock (Children)
			{
				node = Children.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
				return node != null;
			}
		}

		public bool TryGetValue(string name, out ConfigItem node)
		{
			lock (Children)
			{
				node = Options.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
				return node != null;
			}
		}
	}
}
