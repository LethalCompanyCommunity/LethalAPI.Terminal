# LethalAPI.Terminal
Terminal Commands API for the Lethal Company Modding API

## Misc Changes
This mod makes some changes to how the terminal works, that isn't fully integral to custom commands.

These changes are as such:

* Reduces the delay after entering the terminal before you can type by 80%
    
  *  You ever open the terminal and start typing, even hearing the keyboard sound, but it doesn't write anything? This fixes that.

* Disable scrolling to the top of the terminal on every command execution
  *  This now only happens when a command clears the screen.

    
* Trims newlines from the start of command responses
  *  The game force adds newlines to the start of commands, which causes issues for commands that don't clear the terminal. 

## Creating terminal commands
Terminal commands are declared as annotated methods. E.g.,

```cs
[TerminalCommand("Ping"), CommandInfo("Test command")]
public string PingCommand()
{
    return "Pong!";
}
```

All terminal commands are annotated with the `TerminalCommand` Attribute, and optionally, the `CommandInfo` Attribute.

The `CommandInfo` attribute attaches command metadata and enrols the command to be displayed in the help command.

You can also attach access control attributes, e.g., `AllowedCaller` to specify who can execute a command and when. More on this later.

## Registering Commands

All commands have to be registered to this library. Registering commands can be done through the `CommandRegistry` API. E.g.,

```cs
private ModCommands Commands;

public void Awake()
{
  Commands = CommandRegistry.CreateModRegistry();
  Commands.RegisterFrom(this); // Register commands from the plugin class
  Commands.RegisterFrom(AnotherObject); // Register commands from another instance
  Commands.RegisterFrom<InfoCommands>(); // Activate and register commands from a type
}
```

You can also deregister commands by calling `ModCommands.Deregister()`.


## Handling arguments

All arguments are injected into the method, either parsed from the user input, or sourced from the execution context. E.g.,

```cs

[TerminalCommand("RickRoll")]
public string RickRollCommand(Terminal caller, [RemainingText] string text)
{
  // Echoes the user's message back to them, and rickrolls them
  caller.PlayVideoFile(RickrollVideoPath);
  return text;
}

[TerminalCommand("Multiply")]
public int MultiplyCommand(int a, int b)
{
  return a * b;
}
```

Notice how commands don't need to return a string but can return anything. You can return your own `TerminalNode`, which is how the game itself handles command responses, or you can return anything, and it will be converted to a string.

Additionally, if you return `null`, execution will fall through to the next valid command handler. More on this later.

You can even parse some extra types such as `PlayerControllerB`, which will be parsed from Steam64ID, or player name. Argument injection is a useful tool to reduce boilerplate and create command overloads.

You can inject as many arguments as you want, and the method will only be executed when all specified arguments can be provided, either from context, or parsed from user input. Generally, you will never need to manually parse basic arguments yourself. However, if you do want to handle argument parsing yourself, you can inject either `string[]` or `ArgumentStream`.

## Command Overloading

When a player runs a command, the first step in this process is selecting candidate commands. All commands registered with the same command name, and matching signatures can be evaluated. These commands with matching/compatible signatures will be executed in descending order of priority, and then argument count.

So a command with more arguments will take priority over a command with less arguments. However all these arguments still must be valid and parsed from user input. Meaning you can create command patterns such as:

```cs

[TerminalCommand("BuyArtefact")]
public string BuyArtefactCommand()
{
  return $"Available Artefacts:\n"
    + FormatAvailableArtefacts();
}

[TerminalCommand("BuyArtefact")]
public string BuyArtefactCommand(string artefactName)
{
  if(TryBuyArtefact(artefactName))
    return "Artefact purchased!";

  return "Insufficient funds";
}
```

Here, running `BuyArtefact` will execute the first handler, but running `BuyArtefact gun` will execute the second. This is because the 2nd one has more arguments, so it will take priority when the user's input matches the commands signature.

Additionally, the 2nd command could yield execution to the first, by returning `null`. Since commands are executed in order, if one command returns null, command handling will be passed ot the next handler.

This means you can conditionally override other commands, including commands built-in to the game. To aide with this, you can also set the command priority using the `CommandPriority` attribute, to make it execute before another command regardless of argument count.

## Access Control

You can also decorate commands with the `AllowedCaller` attribute, to set what players can execute it, e.g., host-only. For commands registered to be shown in the help command results, commands that a player doesn't have access to won't be shown.  E.g.,

```cs
[TerminalCommand("Kill")]
public string KillPlayerCommand()
{
  return "Usage: Kill [Player]\n" +
    "You must be the host to use this command";
}

[TerminalCommand("Kill"), AllowedCaller(AllowedCaller.Host)]
[CommandInfo("Kills the target player", "[Player]")]
public string KillPlayerCommand(PlayerControllerB player)
{
  if (StartOfRound.Instance == null)
    return "Game not started";

  player.KillPlayer(Vector3.up);
  return $"Killed {player.playerUsername}";
}
```

The first command can be executed by anyone, but still won't show in the help command, since it doesn't have the `CommandInfo` attribute.

The second command can only be executed by the lobby host, and it will show in the help command, but only to the host. If any other player runs the `Other` help command, they will not see the command listed, since they don't have access to it.

### Custom Access Controls

You can also create your own access control attributes, by inheriting `AccessControlAttribute`. E.g.,
```cs
public class TeleporterUnlockedAttribute : AccessControlAttribute
{
  public override bool CheckAllowed()
  {
    if (StartOfRound.Instance == null)
      return false;

    return StartOfRound.Instance.SpawnedShipUnlockables.ContainsKey(5);
  }
}
```

## Misc

This mod also provides a few extension methods:

* `Terminal.PlayVideoFile(string filePath)`
  * Allows you to play local files in the background of the terminal
    
* `Terminal.PlayVideoLink(Uri url)`
  * Allows you to play remote videos in the background of the terminal



