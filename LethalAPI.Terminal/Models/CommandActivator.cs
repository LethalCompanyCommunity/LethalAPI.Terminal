using System;
using System.Reflection;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Interfaces;
using UnityEngine;

namespace LethalAPI.LibTerminal.Models
{
	/// <summary>
	/// Provides command invoker activation from user input and services
	/// </summary>
	public static class CommandActivator
	{
		/// <summary>
		/// Attempts to create a command invoker from a MethodInfo
		/// </summary>
		/// <param name="arguments">User provided arguments</param>
		/// <param name="services">Services to inject into method parameters</param>
		/// <param name="method">Method info that represents the terminal command</param>
		/// <param name="invoker">Resulting invoker for the terminal command</param>
		/// <returns><see langword="true"/> if the provided arguments and services are sufficient to invoke the command</returns>
		public static bool TryCreateInvoker(ArgumentStream arguments, ServiceCollection services, MethodInfo method, out Func<object, object> invoker)
		{
			var commandAttribute = method.GetCustomAttribute<TerminalCommandAttribute>();

			var parameters = method.GetParameters();

			var values = new object[parameters.Length];

			invoker = null;

			for (int i = 0; i < parameters.Length; i++)
			{
				var parameter = parameters[i];
				var type = parameter.ParameterType;

				if (services.TryGetService(type, out var service))
				{
					values[i] = service;
					continue;
				}
				else if (type == typeof(string) && parameter.GetCustomAttribute<RemainingTextAttribute>() != null)
				{
					if (arguments.TryReadRemaining(out var remaining))
					{
						values[i] = remaining;
						continue;
					}

					return false;
				}

				if (arguments.TryReadNext(type, out var value))
				{
					values[i] = value;
					continue;
				}

				return false;
			}

			arguments.Reset();
			invoker = (instance) => ExecuteCommand(method, instance, values, commandAttribute?.ClearText ?? false);
			return true;
		}

		/// <summary>
		/// Executes this command with the specified arguments
		/// </summary>
		/// <param name="arguments">Arguments used to execute this command. Must precisely match the parameters of <seealso cref="Method"/></param>
		/// <returns>Resulting <seealso cref="TerminalNode"/> response, an <seealso cref="Interfaces.ITerminalInteraction"/>, or <see langword="null"/></returns>
		private static object ExecuteCommand(MethodInfo method, object instance, object[] arguments, bool clearConsole)
		{
			object result;
			try
			{
				result = method.Invoke(instance, arguments);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error caught while invoking command hander: {ex.Message}");
				Console.WriteLine(ex.StackTrace);
				return null;
			}

			if (result == null)
			{
				return null;
			}

			var type = result.GetType();

			if (typeof(TerminalNode).IsAssignableFrom(type))
			{
				return result; // Return manual terminal node response
			}
			else if (typeof(ITerminalInteraction).IsAssignableFrom(type))
			{
				return result; // Return terminal interaction
			}
			else if (typeof(ITerminalInterface).IsAssignableFrom(type))
			{
				return result; // Return new terminal interface
			}

			// Convert to auto-text response

			var response = ScriptableObject.CreateInstance<TerminalNode>();
			response.displayText = result.ToString() + '\n';
			response.clearPreviousText = clearConsole;

			return response;
		}
	}
}
