using System;

namespace LethalAPI.LibTerminal.Attributes
{
    /// <summary>
    /// The base attribute type for decorating terminal command methods with access restrictions.
    /// </summary>
    /// <remarks>
    /// Can be decorated on a command method, or on a class to be applied to all commands declared within the class
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class AccessControlAttribute : Attribute
    {
        /// <summary>
        /// Checks if the local player has the required permission level
        /// </summary>
        /// <returns><see langword="true"/> means the player does have permission, <see langword="false"/> otherwise</returns>
        public abstract bool CheckAllowed();
    }
}