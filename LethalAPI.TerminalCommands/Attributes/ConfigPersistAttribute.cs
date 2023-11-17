using System;
using LethalAPI.TerminalCommands.Models.Configuration;

namespace LethalAPI.TerminalCommands.Attributes
{
    /// <summary>
    /// Marks a config to persist using BepinEx's config bindings
    /// </summary>
    public class ConfigPersistAttribute : Attribute
	{
		/// <summary>
		/// The type of config persist
		/// </summary>
		public PersistType PersistType { get; }

		public string ConfigPath { get; }

		/// <summary>
		/// Marks a config to persist using BepinEx's config bindings
		/// </summary>
		/// <param name="persistType">Where the config should be persisted</param>
		/// <param name="configPath">The path the config value should be saved, or <see langword="null"/> if it should be infered</param>
		public ConfigPersistAttribute(PersistType persistType, string configPath = null)
		{
			PersistType = persistType;
			ConfigPath = configPath;
		}
	}
}
