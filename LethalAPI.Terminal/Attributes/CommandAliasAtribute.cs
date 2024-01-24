using System;

namespace LethalAPI.LibTerminal.Attributes
{
    /// <summary>
    /// Specifies aliases for a terminal command
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAliasAttribute : Attribute
    {
        /// <summary>
        /// Case-insensitive command aliases
        /// </summary>
        public string[] Aliases { get; }

        /// <summary>
        /// Specifies aliases for a terminal command
        /// </summary>
        /// <param name="aliases">The case-insensitive command aliases</param>
        public CommandAliasAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}
