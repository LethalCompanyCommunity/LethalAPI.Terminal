using LethalAPI.TerminalCommands.Models.Enums;
using UnityEngine;
using UnityEngine.Video;

namespace LethalAPI.TerminalCommands
{
	/// <summary>
	/// Provides a collection of Builder extension methods for <seealso cref="TerminalNode"/>
	/// </summary>
	public static class TerminalNodeExtensions
	{
		/// <summary>
		/// Sets the display/response text of a terminal node
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="displayText">New display text</param>
		/// <returns>Original terminal node</returns>
		public static TerminalNode WithDisplayText(this TerminalNode node, object displayText)
		{
			node.displayText = displayText.ToString();
			return node;
		}

		/// <summary>
		/// Sets the display texture, visible in the background of the terminal
		/// </summary>
		/// <param name="node">Terminal node to modify</param>.P
		/// <param name="texture">Texture to make visible in the background of the terminal</param>
		/// <returns>Original terminal node</returns>
		public static TerminalNode WithDisplayTexture(this TerminalNode node, UnityEngine.Texture texture)
		{
			node.displayTexture = texture;
			return node;
		}

		/// <summary>
		/// Displays a <see cref="VideoClip"/> in the background of the terminal. This is the same way the 'View Monitor' command works
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="video">Video clip to display in the background</param>
		/// <returns>Original terminal node</returns>
		public static TerminalNode WithVideoClip(this TerminalNode node, VideoClip video)
		{
			node.displayVideo = video;
			return node;
		}

		/// <summary>
		/// Enables accept anything
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="acceptsAnything">accepts anything value</param>
		/// <returns>Original terminal node</returns>
		public static TerminalNode WithAcceptsAnything(this TerminalNode node, bool acceptsAnything) // TODO: Further research on what this value does
		{
			node.acceptAnything = acceptsAnything;
			return node;
		}

		/// <summary>
		/// Sets the buy item index, this sets what item the user is trying to buy
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="index">Buy item index</param>
		/// <returns>Original terminal node</returns>
		public static TerminalNode WithBuyItemIndex(this TerminalNode node, int index)
		{
			node.buyItemIndex = index;
			return node;
		}

		/// <summary>
		/// Sets the route to moon index. This is how the vanilla route command works
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="index">Index of the moon to route to</param>
		/// <returns>Original terminal node</returns>
		public static TerminalNode WithMoonIndex(this TerminalNode node, int index)
		{
			node.buyRerouteToMoon = index;
			return node;
		}

		/// <summary>
		/// Enables clearing the screen text before writing the response
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <returns>The original terminal node</returns>
		public static TerminalNode ClearText(this TerminalNode node)
		{
			node.clearPreviousText = true;
			return node;
		}

		/// <summary>
		/// Sets a price for this terminal node, used to set a price for buying an item or routing to a moon.
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="cost">The new cost in credits for the user to confirm/deny</param>
		/// <returns>The original terminal node</returns>
		public static TerminalNode WithItemCost(this TerminalNode node, int cost)
		{
			node.itemCost = cost;
			return node;
		}

		/// <summary>
		/// Enables slow image loading. This is is the effect that slowly loads the background image/video top to bottom
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <returns>The original terminal node</returns>
		public static TerminalNode LoadImageSlowly(this TerminalNode node)
		{
			node.loadImageSlowly = true;
			return node;
		}

		/// <summary>
		/// Enables image persisting, allowing the background image to persist after the user runs another command
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <returns>The original terminal node</returns>
		public static TerminalNode PersistImage(this TerminalNode node)
		{
			node.persistentImage = true;
			return node;
		}

		/// <summary>
		/// Sets an audio clip to play from the terminal audio source
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="audio">Audio clip to play</param>
		/// <returns></returns>
		public static TerminalNode WithAudio(this TerminalNode node, AudioClip audio)
		{
			node.playClip = audio;
			return node;
		}

		/// <summary>
		/// Sets a terminal event to run. Currently the game only provides the `switch` event for changing the camera
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="eventName">event to trigger</param>
		/// <returns>The original terminal node</returns>
		public static TerminalNode WithTerminalEvent(this TerminalNode node, string eventName)
		{
			node.terminalEvent = eventName;
			return node;
		}

		/// <summary>
		/// Returns an unlock ship item from storage
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="unlockID">Ship unlock ID</param>
		/// <returns>The original terminal node</returns>
		public static TerminalNode ReturnItemFromStorage(this TerminalNode node, int unlockID)
		{
			node.shipUnlockableID = unlockID;
			node.returnFromStorage = true;
			return node;
		}

		/// <summary>
		/// Plays a network-synced audio clip in the terminal
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <param name="clip">Network synced audio clip to play</param>
		/// <returns>The original terminal node</returns>
		public static TerminalNode PlayEffect(this TerminalNode node, SyncedTerminalClip clip)
		{
			node.playSyncedClip = (int)clip;
			return node;
		}

		/// <summary>
		/// Plays the command error sound effect in the terminal, network synced
		/// </summary>
		/// <param name="node">Terminal node to modify</param>
		/// <returns>The original terminal node</returns>
		public static TerminalNode PlayError(this TerminalNode node)
		{
			node.playSyncedClip = (int)SyncedTerminalClip.Error;
			return node;
		}
	}
}