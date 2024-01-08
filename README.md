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

The `CommandInfo` attribute attaches command metadata and enrolls the command to be displayed in the help command.

You can also attach access control attributes, e.g., `AllowedCaller` to specify who can execute a command and when. More on this later.

## Registering Commands

All commands have to be registered to this library. Registering commands can be done through the `CommandRegistry` API. E.g.,

```cs
private TerminalModRegistry Commands;

public void Awake()
{
  Commands = TerminalRegistry.CreateModRegistry();
  Commands.RegisterFrom(this); // Register commands from the plugin class
  Commands.RegisterFrom(AnotherObject); // Register commands from another instance
  Commands.RegisterFrom<InfoCommands>(); // Activate and register commands from a type
}
```

You can also deregister commands by calling `TerminalModRegistry.Deregister()`.


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

## Custom Access Controls

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

## Terminal Interactions

Terminal interactions allow command to prompt the user for more information. Generally, they allow the terminal command to handle the next line of input into the terminal.
You can use an interaction by returning any `ITerminalInteraction` from a terminal command.

While you can implement your own custom terminal interaction by inheriting `ITerminalInteraction`, e.g., to create a multi-line input. There are some pre-existing interaction types you can use.

Every `ITerminalInteraction` has a display message, and then handler to accept the next line of input into the terminal. The display message acts as the initial response of the command/prompt for more input.

### ConfirmInteraction

The following command example demonstrates the Confirm Interaction. It can only be executed by the lobby host, and is used like `Kill [Playername]`.

This block of code will also only execute when the host enters a valid player name/handle into the terminal, and the target player was found. You can create alternate signatures to handle different situations, though such usage is not displayed in these examples.


```cs
[AllowedCaller(AllowedCaller.Host)]
[TerminalCommand("Kill"), CommandInfo("Kills the target player", "[Target]")]
public ITerminalInteraction KillPlayerCommand(PlayerControllerB target)
{
    return new ConfirmInteraction()
        .WithPrompt($"Are you sure you want to kill {target.playerUsername}?")
        .Confirm(() =>
        {
            target.KillPlayer(Vector3.up);
            return $"Killed {target.playerUsername}";
        })
        .Deny(() => "Kill aborted.");
}
```

Notice how the interaction is built using builder notation. 

* **WithPrompt**: Specifies a `string` or `TerminalNode` that will act as the command response/prompt.
* **Confirm**: Specifies a delegate to be executed when the user confirms the interaction.
* **Deny**: Specifies a delegate to be executed when the user inputs anything other than 'Confirm'
* **WithContext**: Registers any number of services to the interaction, to make available for injection into the confirm/deny delegates.

Both the Confirm and Deny delegates act like a command, where they can return a command response. They can even return further terminal interactions.

The `WithContext()` method isn't displayed in this example, but can be used when you want to move the interaction handlers to their own method, instead of an in-line delegate. 
It allows you to register any number of object instances, which can be injected into the handlers by object/parameter type, allowing you to pass state from the parent command/previous interaction into the handlers of the interaction. Think of this as dependency injection for methods.

### TerminalInteraction

This interaction type acts as a general purpose interaction. It can have any number handlers registered to it, and will be executed in-order, depending on the player input.

These handlers act like their own commands, where the best handler is selected based on user input. An example of this can be seen below.

```cs
[AllowedCaller(AllowedCaller.Host)]
[TerminalCommand("Payday"), CommandInfo("Spawns in credits")]
public ITerminalInteraction PaydayCommand(Terminal terminal)
{
    return new TerminalInteraction()
        .WithPrompt("How many credits would you like?")
        .WithHandler(PaydayCommand_AddCredits)  // Ran when the user enters a valid int
        .WithHandler(PaydayCommand_HandleFail)  // Ran all other times
        .WithContext(terminal);
}

private string PaydayCommand_AddCredits(Terminal terminal, int credits)
{
    terminal.groupCredits += credits;
    terminal.SyncGroupCreditsServerRpc(terminal.groupCredits, terminal.numberOfItemsInDropship);

    return $"Added {credits} to your account. New balance: {terminal.groupCredits}";
}

private string PaydayCommand_HandleFail([RemainingText] string message)
{
    return $"'{message}' is not a valid amount of credits.";
}
```
This is also an example of terminal interactions with handler methods instead of in-line delegates.

In this example, the interaction is prompting the user to input a number of credits. If the user enters a valid `int`, the `PaydayCommand_AddCredits` handler will be executed.
If the user does not enter a valid number, the `PaydayCommand_HandleFail` will be executed, as it can accept any text input.

You will also notice the use of `WithContext` in the builder notation. This registers the `Terminal` instance to the interaction, allowing it to be injected into the handlers. The AddCredits handler makes use of this, as it requires the terminal instance to modify the number of group credits.


## Terminal Interfaces

This library also allows you to create custom terminal interfaces. These custom interfaces allow you to completely overhaul the terminal, without breaking support with other mods.

An example use case for this, is to overhaul the terminal to create a Bash-like command line, with text post processing to add text to the left of the user's input area. And since the terminal interface even allows you to handle all terminal input and output, it would even be possible to create pipes, to reroute the output of one command into the input of another.

Custom terminal interfaces inherit `ITerminalInterface`, and provide full control over the terminal. Including:
* Command response text post processing
* The initial message when you enter a terminal
* Enable/Disable Lethal Company command response post processing
* Enable/Disable This library's custom command response post processing

 While this feature is powerful, it will also not be of use for the majority of mods. Also, this should only be used when you want to overhaul and the look or feel of the terminal, and should not be used in place of Terminal Interactions.

 Since these custom interfaces also receives all terminal input, if you implement it, you also need to implement command handling. Though, this is easily achieved using the APIs exposed in the `CommandHandler` class.

 For more documentation, see the code comments on [`ITerminalInterface`](https://github.com/LethalCompany/LethalAPI.Terminal/blob/development/LethalAPI.Terminal/Interfaces/ITerminalInterface.cs)

## Misc

This mod also provides a few extension methods:

* `Terminal.PlayVideoFile(string filePath)`
  * Allows you to play local files in the background of the terminal
    
* `Terminal.PlayVideoLink(Uri url)`
  * Allows you to play remote videos in the background of the terminal



  ## Building

  Building this projects requires the `LC_REFERENCES` environment variable, containing the path to the *'Lethal Company_Data\Managed'* folder inside your installation of Lethal Company.
