using System;

namespace LCAPI.TerminalCommands.Attributes
{
	/// <summary>
	/// Marks a property as configurable in the config terminal command
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class TerminalConfigAttribute : Attribute
	{
		/// <summary>
		/// The name of the setting, or <see langword="null"/> if it should be inferred from the property
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Config description/help message
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Marks a property as configurable in the config terminal command
		/// </summary>
		/// <param name="description">Setting description</param>
		/// <param name="name">Optional name for the config value</param>
		public TerminalConfigAttribute(string description, string name = null)
		{
			Name = name;
			Description = description;
		}
	}
}
