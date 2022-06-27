using CommandMod.Commands;
using CommandMod.UI;
using ModLoader;
using ModLoader.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommandMod
{
	public class CommandMod : Mod
	{
		public static CommandMod Main;

		public KeySettings keys;

		public event EventHandler<string> onMenssage;
		private const int limit = 25;
		private Queue<string> _queue;
		private Dictionary<string, BaseCommand> _commands = new Dictionary<string, BaseCommand>();

		private ConsoleUI _console;

		public CommandMod() : base("commandMod", "Command Mod", "dani0105", "0.5.7", "v2.0.0", "Add api for other modders to implement commands!, to start use this mod press 'T'")
		{
			Main = this;
			this._queue = new Queue<string>();
		}

		/// <summary>
		/// Here you can add new command for the console
		/// </summary>
		/// <param name="command"> command class </param>
		public void registerCommand(BaseCommand command)
		{
			if (_commands.ContainsKey(command.Id))
			{
				throw new Exception($"{command.Id} has already been registered");
			}
			_commands.Add(command.Id, command);
		}

		/// <summary>
		/// check the user input to verify if a single menssage or a command
		/// </summary>
		/// <param name="inputString">user input</param>
		public void processInput(string inputString)
		{
			if (inputString.StartsWith("/")) // is a command
			{
				this.processCommand(inputString.Remove(0, 1));
			}
			else // is a single menssage(for the future)
			{
				this.onMenssage?.Invoke(this, inputString);
			}

		}

		/// <summary>
		/// this check if the command exist and execute
		/// </summary>
		/// <param name="commandString">command typed for the user </param>
		private void processCommand(string commandString)
		{

			string[] keywords = commandString.Split(' ');
			if (keywords.Length > 0)
			{
				BaseCommand command;
				if (_commands.TryGetValue(keywords[0], out command))
				{
					string response = command.invoke(keywords.Skip(1).ToArray());
					this.addMenssage(response);
					return;
				}
				this.addMenssage($"'{commandString}' is not recognized as an internal or external command");
			}

		}

		private void addMenssage(string message)
		{
			message = '>' + message + '\n';
			if (this._queue.Count > limit)
			{
				this._queue.Dequeue();
			}
			this._queue.Enqueue(message);

			StringBuilder builder = new StringBuilder();
			foreach (string msg in this._queue)
			{
				builder.Append(msg);
			}
			this._console.Messages = builder.ToString();
		}

		/// <summary>
		/// show the enabled command list 
		/// </summary>
		/// <param name="args">list of params or argument typed by the user</param>
		private string helpCommand(string[] args)
		{

			StringBuilder message = new StringBuilder();

			if (args.Length == 1)
			{
				BaseCommand command;
				if (_commands.TryGetValue(args[0], out command))
				{
					message.AppendLine("--- Command information ---\n");
					message.AppendLine("Command: " + command.Id);
					message.AppendLine("Description: \n" + command.Description);
					message.Append("Format: \n" + command.Format);
				}
				else
				{
					message.AppendLine($"'{args[0]}' is not recognized as an internal or external command");
				}
				return message.ToString();
			}

			message.AppendLine("For more information on a specific command, type /help <commadn_name>");

			string format = "--- Command List ---";
			message.AppendLine(format);
			foreach (BaseCommand command in _commands.Values)
			{
				format = "\n/{0}: {1}";
				message.AppendFormat(format, command.Id, command.Description);
			}
			return message.ToString();
		}

		private string clearCommand(string[] args)
		{
			this._queue.Clear();
			return "";
		}

		public override void Load()
		{
			// Load keybindings
			KeySettings.Setup();

			// Add console
			GameObject console = new GameObject("Command Mod");
			this._console = console.AddComponent<ConsoleUI>();
			UnityEngine.Object.DontDestroyOnLoad(console);
			console.SetActive(true);
			ConsoleCommand helpCommand = new ConsoleCommand("help", "Show all command information", "/help <command_name>", this.helpCommand);
			ConsoleCommand clearCommand = new ConsoleCommand("clear", "clear all messages", "/clear", this.clearCommand);
			this.registerCommand(helpCommand);
			this.registerCommand(clearCommand);
		}

		public override void Unload()
		{
			throw new NotImplementedException();
		}
	}
}
