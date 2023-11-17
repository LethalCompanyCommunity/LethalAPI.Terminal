using System;
using System.Collections.Generic;
using System.Reflection;
using LethalAPI.TerminalCommands.Attributes;

namespace LethalAPI.TerminalCommands.Models.Configuration
{
	public class ConfigTree
	{
		public ConfigNode Root { get; } = new ConfigNode("root", "Root config node");

		public void RegisterFrom(Type type, object instance)
		{
			// Descend to config node

			var node = Root;

			var parentType = type;

			while (true)
			{
				var attribute = parentType.GetCustomAttributes<ConfigGroupAttribute>();
				node = DescendNodes(node, attribute);

				if (parentType?.DeclaringType?.IsClass ?? false)
				{
					parentType = parentType.DeclaringType;
					continue;
				}

				break;
			}

			// Register config properties

			foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				var configAttribute = property.GetCustomAttribute<TerminalConfigAttribute>();

				if (configAttribute != null)
				{
					continue;
				}

				node.CreateConfigItem(configAttribute.Name, configAttribute.Description, property, instance, PersistType.None);
			}
		}

		private ConfigNode DescendNodes(ConfigNode parent, IEnumerable<ConfigGroupAttribute> groups)
		{
			var node = parent;
			foreach (var group in groups)
			{
				node = node.GetOrCreateChild(group.Name, group.Description);
			}
			return node;
		}
	}
}
