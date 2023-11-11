using System.Collections.Generic;

namespace LCAPI.TerminalCommands.Models
{
	/// <summary>
	/// Compares terminal commands to determine preference order. Orders by priority, then parameter count.
	/// </summary>
	public class CommandComparer : IComparer<TerminalCommand>
	{
		public int Compare(TerminalCommand x, TerminalCommand y)
		{
			if (x.Priority > y.Priority)
			{
				return 1;
			}
			else if (x.Priority < y.Priority)
			{
				return -1;
			}

			return x.ArgumentCount.CompareTo(y.ArgumentCount);
		}
	}
}
