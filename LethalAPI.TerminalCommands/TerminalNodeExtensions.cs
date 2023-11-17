using LethalAPI.TerminalCommands.Models.Enums;
using UnityEngine;
using UnityEngine.Video;

namespace LethalAPI.TerminalCommands
{
	public static class TerminalNodeExtensions
	{
		public static TerminalNode WithDisplayText(this TerminalNode node, object displayText)
		{
			node.displayText = displayText.ToString();
			return node;
		}

		public static TerminalNode WithDisplayTexture(this TerminalNode node, UnityEngine.Texture texture)
		{
			node.displayTexture = texture;
			return node;
		}

		public static TerminalNode WithVideoClip(this TerminalNode node, VideoClip video)
		{
			node.displayVideo = video;
			return node;
		}

		public static TerminalNode WithAcceptsAnything(this TerminalNode node, bool acceptsAnything)
		{
			node.acceptAnything = acceptsAnything;
			return node;
		}

		public static TerminalNode WithBuyItemIndex(this TerminalNode node, int index)
		{
			node.buyItemIndex = index;
			return node;
		}

		public static TerminalNode WithMoonIndex(this TerminalNode node, int index)
		{
			node.buyRerouteToMoon = index;
			return node;
		}

		public static TerminalNode ClearText(this TerminalNode node)
		{
			node.clearPreviousText = true;
			return node;
		}

		public static TerminalNode WithItemCost(this TerminalNode node, int cost)
		{
			node.itemCost = cost;
			return node;
		}

		public static TerminalNode LoadImageSlowly(this TerminalNode node)
		{
			node.loadImageSlowly = true;
			return node;
		}

		public static TerminalNode PersistImage(this TerminalNode node)
		{
			node.persistentImage = true;
			return node;
		}

		public static TerminalNode WithAudio(this TerminalNode node, AudioClip audio)
		{
			node.playClip = audio;
			return node;
		}

		public static TerminalNode WithTerminalEvent(this TerminalNode node, string eventName)
		{
			node.terminalEvent = eventName;
			return node;
		}

		public static TerminalNode ReturnItemFromStorage(this TerminalNode node, int unlockID)
		{
			node.shipUnlockableID = unlockID;
			node.returnFromStorage = true;
			return node;
		}

		public static TerminalNode PlayEffect(this TerminalNode node, TerminalAudioClip clip)
		{
			node.playSyncedClip = (int)clip;
			return node;
		}
	}
}
