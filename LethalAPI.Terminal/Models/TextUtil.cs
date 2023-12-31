namespace LethalAPI.LibTerminal.Models
{
	/// <summary>
	/// Provides some misc string manipulation tools
	/// </summary>
	public static class TextUtil
	{
		/// <summary>
		/// Applies a minimum trailing character count to the end of a string, padding the end with the specified character until it reaches the specified number of trailing characters
		/// </summary>
		/// <param name="text">Text to apply minimum padding to</param>
		/// <param name="character">The character to check the text for trailing instances of, and to append to the text as needed</param>
		/// <param name="minPadding">Minimum number trailing instances of <paramref name="character"/></param>
		/// <returns>Resulting string with the minimum padding applied</returns>
		/// <remarks>
		/// This method is used to apply a minimum newline count to the end of command output, to enforce newlines at the end of command response, and a gap between the response and the next command input
		/// </remarks>
		public static string SetEndPadding(this string text, char character, int minPadding)
		{
			var result = text;
			var newlineCount = 0;

			for (int i = text.Length - 1; i >= 0; i--)
			{
				if (text[i] == character)
				{
					newlineCount++;
					continue;
				}
				break;
			}

			var newlinesRequired = minPadding - newlineCount;

			if (newlinesRequired > 0)
			{
				result += new string(character, newlinesRequired);
			}

			return result;
		}

		/// <summary>
		/// Applies a minimum padding character count to the start of a string, padding the start with the specified character until it reaches the specified number of characters
		/// </summary>
		/// <param name="text">Text to apply minimum padding to</param>
		/// <param name="character">The character to check the text for trailing instances of, and to append to the text as needed</param>
		/// <param name="minPadding">Minimum number trailing instances of <paramref name="character"/></param>
		/// <returns>Resulting string with the minimum padding applied</returns>
		public static string SetStartPadding(this string text, char character, int minPadding)
		{
			var result = text;
			var newlineCount = 0;

			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == character)
				{
					newlineCount++;
					continue;
				}
				break;
			}

			var newlinesRequired = minPadding - newlineCount;

			if (newlinesRequired > 0)
			{
				result = new string(character, newlinesRequired) + result;
			}

			return result;
		}
	}
}
