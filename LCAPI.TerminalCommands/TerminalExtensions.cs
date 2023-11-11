using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace LCAPI.TerminalCommands
{
	/// <summary>
	/// Provides a collection of extension methods for the <seealso cref="Terminal"/> class
	/// </summary>
	public static class TerminalExtensions
	{
		/// <summary>
		/// Plays a local video file in the terminal. Playback of this video will end when the user leaves the terminal, or executes another command.
		/// </summary>
		/// <remarks>
		/// Also worth noting this doesn't support audio, for audio, look at <seealso cref="StartOfRound.speakerAudioSource"/>.PlayOneShot()
		/// </remarks>
		/// <param name="terminal">Terminal instance</param>
		/// <param name="filePath">Path to the video to play</param>
		public static void PlayVideoFile(this Terminal terminal, string filePath)
		{
			var uri = "file:///" + filePath.Replace('\\', '/');
			terminal.PlayVideoLink(uri);
		}

		/// <summary>
		/// Plays a remote video file. Playback of this video will end when the user leaves the terminal, or executes another command.
		/// </summary>
		/// <remarks>
		/// Also worth noting this doesn't support audio, for audio, look at <seealso cref="StartOfRound.speakerAudioSource"/>.PlayOneShot()
		/// </remarks>
		/// <param name="terminal">Terminal instance</param>
		/// <param name="url">URI to the video file to play</param>
		public static void PlayVideoLink(this Terminal terminal, Uri url)
		{
			terminal.StartCoroutine(PlayVideoLink(url.AbsoluteUri, terminal));
		}

		/// <summary>
		/// Plays the specified video link. This link must be in URI format, and by itself does not support local files. See <seealso cref="PlayVideoFile(Terminal, string)"/> for local file support.
		/// </summary>
		/// <remarks>
		/// Also worth noting this doesn't support audio, for audio, look at <seealso cref="StartOfRound.speakerAudioSource"/>.PlayOneShot()
		/// </remarks>
		/// <param name="terminal">Terminal instance</param>
		/// <param name="url">URI to the video file to play</param>
		public static void PlayVideoLink(this Terminal terminal, string url)
		{
			terminal.StartCoroutine(PlayVideoLink(url, terminal));
		}

		/// <summary>
		/// Plays the video file on the next fixed update, to skip the built-in video clip video player that only supports instances of <seealso cref="VideoClip"/>, and not URIs
		/// </summary>
		/// <param name="url">URI to send to teh video player</param>
		/// <param name="terminal">Terminal instance</param>
		private static IEnumerator PlayVideoLink(string url, Terminal terminal)
		{
			yield return new WaitForFixedUpdate();

			terminal.terminalImage.enabled = true;
			terminal.terminalImage.texture = terminal.videoTexture;
			terminal.displayingPersistentImage = null;

			terminal.videoPlayer.clip = null;
			terminal.videoPlayer.source = VideoSource.Url;
			terminal.videoPlayer.url = url;

			terminal.videoPlayer.enabled = true;
		}
	}
}
