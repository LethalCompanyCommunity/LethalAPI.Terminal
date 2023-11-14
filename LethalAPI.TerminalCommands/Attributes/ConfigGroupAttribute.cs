using System;

namespace LethalAPI.TerminalCommands.Attributes
{
	/// <summary>
	/// Marks all <seealso cref="TerminalConfigAttribute"/> settings within a class as part of a group
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ConfigGroupAttribute : Attribute
	{
		/// <summary>
		/// The name of the config group
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The description/help message for the group
		/// </summary>
		public string Description { get; }


		/// <summary>
		/// Sets all terminal config values within a class as part of a group
		/// </summary>
		/// <param name="name">The name of the config group. Can specify multiple layers by separating them with a period.</param>
		/// <param name="description">Group config description that is shown in the terminal</param>
		public ConfigGroupAttribute(string name, string description)
		{
			Name = name;
			Description = description;
		}
	}
}
