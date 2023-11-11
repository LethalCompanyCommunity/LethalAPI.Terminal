using System;
using System.Linq;
using GameNetcodeStuff;

namespace LCAPI.TerminalCommands.Models
{
	/// <summary>
	/// Provides services for parsing user-entered strings into types, including custom game types.
	/// </summary>
	public static class StringConverter
	{
		/// <summary>
		/// Underlying string converter used to parse primitive values
		/// </summary>
		public static System.ComponentModel.StringConverter Converter { get; } = new System.ComponentModel.StringConverter();

		/// <summary>
		/// Attempts to convert the specified string to the specified type
		/// </summary>
		/// <param name="value">String value to parse</param>
		/// <param name="type">The type to parse the string as</param>
		/// <param name="result">Resulting object instance, or <see langword="null"/></param>
		/// <returns><see langword="true"/> if the string could be parsed as the specified type</returns>
		public static bool TryConvert(string value, Type type, out object result)
		{
			// Custom type converters here

			if (type == typeof(PlayerControllerB))
			{
				return TryParsePlayerB(value, out result);
			}

			try
			{
				// Fallback primitive type converter
				if (Converter.CanConvertTo(type))
				{
					result = Converter.ConvertTo(value, type);
					return true;
				}
			}
			catch (ArgumentException)
			{
				// Incorrect string format, return false
			}

			result = null;
			return false;
		}

		private static bool TryParsePlayerB(string value, out object result)
		{
			if (StartOfRound.Instance == null) // Game not started
			{
				result = null;
				return false;
			}

			PlayerControllerB player = null;
			if (ulong.TryParse(value, out var steamID))
			{
				player = StartOfRound.Instance.allPlayerScripts
										.FirstOrDefault(x => x.playerSteamId == steamID);
			}

			if (player == null)
			{
				player = StartOfRound.Instance.allPlayerScripts
										.FirstOrDefault(x => x.playerUsername.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1);
			}

			result = player;
			return player != null;
		}
	}
}
