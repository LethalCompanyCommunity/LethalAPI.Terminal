# LethalAPI.Terminal
Terminal Commands API for the Lethal Company Modding API

# Entity Commands

This section details the commands related to entities such as landmines and turrets within the game.

## Commands

### ListMines
- **Description**: Lists all landmines within the game.
- **Usage**: `ListMines`
- **Details**: Scans for landmines and returns their object codes.

### ListTurrets
- **Description**: Lists all turrets.
- **Usage**: `ListTurrets`
- **Details**: Scans for turrets and returns their object codes.

### Detonate
- **Description**: Detonates designated landmine(s).
- **Usage**: `Detonate [All / Mine Id]`
- **Details**: Users can specify a particular mine by its ID or use "All" to detonate all mines. Checks if the specified landmine has already exploded and returns the status.

### Berserk
- **Description**: Makes designated turret(s) go berserk.
- **Usage**: `Berserk [All / Turret Id]`
- **Details**: Targets a specific turret by its ID or use "All" to affect every turret. Toggles the turret's mode between normal and berserk, providing feedback on the action taken.

# Equipment Commands

This section details the commands related to equipment, specifically focusing on the functionalities around the teleporter in the game.

## Commands

### Teleport
- **Description**: Teleport the specified member of the crew.
- **Usage**: `Teleport [Player Name]`
- **Details**: Teleports a specified player to a set location. If no player is specified, it attempts to teleport the player currently targeted in the map screen.

### InverseTeleport
- **Description**: Start the inverse teleporter.
- **Usage**: `Inverse`
- **Details**: Initiates the inverse teleportation process. Checks if the teleporter is available and interactable before initiating.

### ResetInverse
- **Description**: Resets the inverse teleporter cooldown.
- **Usage**: `ResetInverse`, `Reset`
- **Details**: Resets the cooldown for the inverse teleporter, allowing it to be used again immediately.

### Scramble
- **Description**: Reset then activate inverse teleporter.
- **Usage**: `Scramble`
- **Details**: Performs a reset of the inverse teleporter's cooldown and then immediately initiates inverse teleportation.

# Info Commands

This section details the commands that provide information about various aspects of the game environment, such as time and crew member status.

## Commands

### Time
- **Description**: Tells the current time.
- **Usage**: `Time`
- **Details**: Provides the current time.

### Status
- **Description**: Get the status of crew members.
- **Usage**: `Status [Player name]`
- **Details**: Displays the status of the designated crew member. If none is specified, lists the status of all crew members.

### Clear
- **Description**: Clears the console.
- **Usage**: `Clear`
- **Details**: Clears all text from the console, providing a clean slate for new commands and responses.

# Ship Commands

This section details the commands related to ship operations, such as managing lights, doors, and the ship's landing status.

## Commands

### Lights
- **Description**: Toggle the lights on or off.
- **Usage**: `Lights`
- **Details**: Interacts with the game object named "LightSwitch" to toggle the state of the lights.

### Door
- **Description**: Toggle the door's open or close state.
- **Usage**: `Door`, `Doors`
- **Details**: Determines the current state of the door and sends a command to either open or close it.

### Close
- **Description**: Close the door.
- **Usage**: `Close`
- **Details**: Closes the ship's door and provides current door power.

### Open
- **Description**: Open the door.
- **Usage**: `Open`
- **Details**: Opens the ship's door and provides current door power.

### Launch
- **Description**: Activate an emergency exit from the current situation.
- **Usage**: `Launch`, `GTFO`
- **Details**: Launches the ship.

### Land
- **Description**: Land the ship.
- **Usage**: `Land`
- **Details**: Lands the ship.

## Attribution

This document and the associated commands are based on the work found in the original repository. For more detailed information, contributions, or to view the source code, please visit:

[LethalAPI.Terminal](https://github.com/LethalCompany/LethalAPI.Terminal)

## Acknowledgements

The development of this README was assisted by AI tools provided by OpenAI. Their guidance and information helped in compiling the summaries and structuring this document.

## Disclaimer

Please note that the code and commands provided in this repository are currently in **beta**. While every effort is made to ensure reliability and functionality, the code is still under development and may contain bugs or issues. **No guarantees are made regarding its functionality or success**. Users are advised to use this code at their own risk and to report any issues they encounter to help improve the project.

