using System;
using System.Reflection;
using LethalAPI.TerminalCommands.Interfaces;
using LethalAPI.TerminalCommands.Models;
using UnityEngine;

namespace LethalAPI.TerminalCommands.Interactions
{
	/// <summary>
	/// Provides a Confirm/Deny terminal interaction 
	/// </summary>
	/// <remarks>
	/// You can return this interaction from a Terminal Command to implement a Confirm/Deny action pattern
	/// </remarks>
	public class ConfirmInteraction : ITerminalInteraction
	{
		/// <summary>
		/// Command response/interaction prompt to display to the user
		/// </summary>
		public TerminalNode Prompt { get; private set; }

		/// <summary>
		/// Services to provide to the interaction handlers
		/// </summary>
		public ServiceCollection Services { get; } = new ServiceCollection();

		/// <summary>
		/// Delegate executed when the action is confirmed
		/// </summary>
		public Delegate ConfirmHandler { get; set; }

		/// <summary>
		/// Delegate execution when the action is denied, or an ambiguous response is received
		/// </summary>
		public Delegate DenyHandler { get; set; }

		/// <summary>
		/// Creates en empty confirmation interaction
		/// </summary>
		public ConfirmInteraction()
		{
		}

		/// <summary>
		/// Creates a confirmation interaction with the specified prompt
		/// </summary>
		/// <param name="prompt">Prompt to display to the user</param>
		/// <remarks>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction
		/// </remarks>
		public ConfirmInteraction(TerminalNode prompt)
		{
			Prompt = prompt;
			PostprocessPrompt();
		}

		/// <summary>
		/// Creates a confirmation interaction with the specified prompt
		/// </summary>
		/// <param name="promptBuilder">Builder method to create a response <seealso cref="TerminalNode"/>, displayed as the response/prompt for the confirm/deny dialog</param>
		/// <remarks>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction
		/// </remarks>
		public ConfirmInteraction(Action<TerminalNode> promptBuilder)
		{
			var prompt = ScriptableObject.CreateInstance<TerminalNode>();
			promptBuilder(prompt);

			WithPrompt(prompt);
		}

		/// <summary>
		/// Creates a confirmation interaction
		/// </summary>
		/// <param name="prompt">Prompt to display to the user</param>
		/// <param name="confirm">The delegate handler executed when the user confirms the action</param>
		/// <param name="deny">The delegate handler executed when the user denies the action, or provides an ambiguous response</param>
		/// <remarks>
		/// <para>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction
		/// </para>
		/// <para>
		/// Services are injected into the confirm/deny handler parameters, from command context, and contested registered with <seealso cref="WithContext(object[])"/>
		/// </para>
		/// </remarks>
		public ConfirmInteraction(TerminalNode prompt, Delegate confirm, Delegate deny)
		{
			WithPrompt(prompt);
			ConfirmHandler = confirm;
			DenyHandler = deny;
		}

		/// <summary>
		/// Creates a confirmation interaction
		/// </summary>
		/// <param name="promptBuilder">Builder method to create a response <seealso cref="TerminalNode"/>, displayed as the response/prompt for the confirm/deny dialog</param>
		/// <param name="confirm">The delegate handler executed when the user confirms the action</param>
		/// <param name="deny">The delegate handler executed when the user denies the action, or provides an ambiguous response</param>
		/// <remarks>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction
		/// </remarks>
		public ConfirmInteraction(Action<TerminalNode> promptBuilder, Delegate confirm, Delegate deny)
		{
			WithPrompt(promptBuilder);
			ConfirmHandler = confirm;
			DenyHandler = deny;
		}

		/// <summary>
		/// Creates a confirmation interaction with the specified prompt
		/// </summary>
		/// <param name="prompt">Prompt to display to the user</param>
		/// <remarks>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction
		/// </remarks>
		public ConfirmInteraction(string prompt)
		{
			WithPrompt(prompt);
		}

		/// <summary>
		/// Creates a confirmation interaction
		/// </summary>
		/// <param name="prompt">Prompt to display to the user</param>
		/// <param name="confirm">The delegate handler executed when the user confirms the action</param>
		/// <param name="deny">The delegate handler executed when the user denies the action, or provides an ambiguous response</param>
		/// <remarks>
		/// <para>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction
		/// </para>
		/// <para>
		/// Services are injected into the confirm/deny handler parameters, from command context, and contested registered with <seealso cref="WithContext(object[])"/>
		/// </para>
		/// </remarks>
		public ConfirmInteraction(string prompt, Delegate confirm, Delegate deny)
		{
			WithPrompt(prompt);
			ConfirmHandler = confirm;
			DenyHandler = deny;
		}

		/// <summary>
		/// Sets the response the command/prompt of the confirmation dialog
		/// </summary>
		/// <param name="prompt">Prompt to display to the user</param>
		/// <returns>Parent confirmation interaction</returns>
		/// <remarks>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction
		/// </remarks>
		public ConfirmInteraction WithPrompt(string prompt)
		{
			Prompt = ScriptableObject.CreateInstance<TerminalNode>();
			Prompt.WithDisplayText(prompt);
			PostprocessPrompt();
			return this;
		}

		/// <summary>
		/// Sets the response the command/prompt of the confirmation dialog
		/// </summary>
		/// <param name="prompt">Prompt to display to the user</param>
		/// <returns>Parent confirmation interaction</returns>
		/// <remarks>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction
		/// </remarks>
		public ConfirmInteraction WithPrompt(TerminalNode prompt)
		{
			Prompt = prompt;
			PostprocessPrompt();
			return this;
		}

		/// <summary>
		/// Sets the response the command/prompt of the confirmation dialog
		/// </summary>
		/// <param name="promptBuilder">Builder method to create a response <seealso cref="TerminalNode"/>, displayed as the response/prompt for the confirm/deny dialog</param>
		/// <returns>Parent confirmation interaction</returns>
		/// <remarks>
		/// You do not need to add any custom Confirm/Deny text, as it is added by the interaction.
		/// </remarks>
		public ConfirmInteraction WithPrompt(Action<TerminalNode> promptBuilder)
		{
			var prompt = ScriptableObject.CreateInstance<TerminalNode>();
			promptBuilder(prompt);

			WithPrompt(prompt);
			return this;
		}

		/// <summary>
		/// Sets the confirmation delegate that is executed when the user confirms the action
		/// </summary>
		/// <param name="confirmHandler">Delegate that is executed when the user confirms the action</param>
		/// <returns>Parent confirmation interaction</returns>
		///	<remarks>
		/// Parameters are injected into the delegate, based from the command context, and contested provided with <seealso cref="WithContext(object[])"/>
		/// </remarks>
		public ConfirmInteraction Confirm(Delegate confirmHandler)
		{
			ConfirmHandler = confirmHandler;
			return this;
		}

		/// <summary>
		/// Sets the deny delegate that is executed when the user confirms the action, or enters an ambiguous response
		/// </summary>
		/// <param name="denyHandler">Delegate that is executed when the user denies the action</param>
		/// <returns>Parent confirmation interaction</returns>
		///	<remarks>
		/// Parameters are injected into the delegate, based from the command context, and contested provided with <seealso cref="WithContext(object[])"/>
		/// </remarks>
		public ConfirmInteraction Deny(Delegate denyHandler)
		{
			DenyHandler = denyHandler;
			return this;
		}

		/// <summary>
		/// Provides services that can be injected into the confirm and deny handlers
		/// </summary>
		/// <remarks>
		/// Used to pass context/state to the interaction handlers
		/// </remarks>
		/// <param name="services">Services to make available to the confirm and deny handlers</param>
		/// <returns>Parent confirmation interaction</returns>
		public ConfirmInteraction WithContext(params object[] services)
		{
			Services.WithServices(services);
			return this;
		}

		/// <summary>
		/// Handles terminal input for the terminal interaction
		/// </summary>
		/// <param name="arguments">Arguments provided by the user</param>
		/// <returns>Interaction response, or <see langword="null"/></returns>
		public object HandleTerminalResponse(ArgumentStream arguments)
		{
			if (!arguments.TryReadNext(out string response))
			{
				return Deny(arguments);
			}

			if (response.Trim().Equals("confirm", StringComparison.InvariantCultureIgnoreCase))
			{
				return Confirm(arguments);
			}

			return Deny(arguments);
		}

		/// <summary>
		/// Executes the confirmation handler, or returns a blank response if none is set.
		/// </summary>
		/// <param name="arguments">Arguments provided by the user</param>
		/// <returns>Response object</returns>
		private object Confirm(ArgumentStream arguments)
		{
			if (DenyHandler == null)
			{
				return string.Empty;
			}

			if (CommandActivator.TryCreateInvoker(arguments, Services, ConfirmHandler.GetMethodInfo(), out var invoker))
			{
				return invoker(ConfirmHandler.Target);
			}

			return null;
		}

		/// <summary>
		/// Executes the deny handler, or returns a blank response if none is set.
		/// </summary>
		/// <param name="arguments">Arguments provided by the user</param>
		/// <returns>Response object</returns>
		private object Deny(ArgumentStream arguments)
		{
			if (DenyHandler == null)
			{
				return string.Empty;
			}

			if (CommandActivator.TryCreateInvoker(arguments, Services, DenyHandler.GetMethodInfo(), out var invoker))
			{
				return invoker(DenyHandler.Target);
			}
			return null;
		}

		/// <summary>
		/// Performs display text post processing, appending the Confirm/Deny to the end of the parent provided prompt
		/// </summary>
		private void PostprocessPrompt()
		{
			var text = Prompt.displayText ?? string.Empty;

			text = TextUtil.SetEndPadding(text, '\n', 2) + "Please CONFIRM or DENY.\n";

			Prompt.displayText = text;
		}
	}
}
