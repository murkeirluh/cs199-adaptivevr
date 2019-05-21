#  CS 199 Scripts

This repo contains scripts for the virtual reality application for the study Adaptive Virtual Reality Disaster Simulation for Community Training.

Code description
--------
### ActivateDoor.cs
This script checks for the conditions during the scene where user has to turn off the LPG tank and the main electric switch (if applicable). If correct conditions are met, the door will be highlighted and activated, and the user can move on to the next module (During typhoon).


### Adaptivity.cs
This script handles the showing of hints and the adaptive timer that ticks after user's last successful tap. Also stored here is the user's counter for incorrect items and decisions made. 


### BGSfxManager.cs
Manages background sound effects for Before Typhoon. Audio sources are attached to the Game object.


### BackgroundTimer.cs
This code is responsible for the 15-minute timer that runs in the background. This also brings up the "take a break" screen once the 15 minutes is up. 


### ChangeMat.cs
This handles the changing of the walker tile's material once hovered in and out by the reticle.


### ChecklistUpdate.cs
This script updates the checklist display text. This script is also responsible for the rearrangement of the checklist items.


### ChildDecision.cs
This code handles the situation where in a child asks to play in the water in the During typhoon scene.


### Correct.cs
Pops up a congratulatory/explanation message  after the fallen electrical posts hazard in the During typhoon scenario.


### DontDestroy.cs
This script handles the GameObjects that shouldn't be destroyed during the gameplay.


### Drop_Script.cs
Pops up a congratulatory + explanation message after the still water hazard scene in the During typhoon scenario.


### ElectricEvent_Script.cs
This script handles the electrical posts hazard event.


### Fade.cs
This script handles the fade out effect during scene transitions.


### Fall_Script.cs
This script is responsible for the scene where the user can fall after attempting to walk towards the still water without grabbing the stick.


### FloodAlert.cs
Activates flood alerts in the LPG/Main switch scenario.


### Launcher.cs
This script provides functions for loading and starting a new game. Generation of Player ID and module name updating is taken care by this script.


### LetNPCIn.cs
This script is responsible for the scene where the neighbor NPC knocks on the door. The knocking sound is played here, and this also handles the NPC “entering” the room.\


### Log.cs
Responsible for activating the walker tile if the user looks at an angle downwards. It also checks conditions such as if the walker tile will intersect with another object.


### Moving_Water_Script.cs
This script is responsible for the scene where the user can get carried away by the moving water hazard.


### NPC.cs
This script handles the scenario where the NPC neighbor talks to the user. This also takes care of the UI elements in the environment.


### PlaceObject.cs
This script takes care of placing/dropping an object held by the user on a surface that the player looks at.


### Player.cs
This script is attached to the Game object. Stores important player info and personalization variables. It is also responsible for saving and loading player onto the game. 


### PlayerData.cs
This script handles the serializing of the Player data variables for storing and saving to file.


### PutBackpack.cs
This script is responsible for putting stuff in the backpack and placing items in the environment. This also updates the checklist dictionary and handles the respawning of the supplies in the room.


### SaveSystem.cs
In here, the file reading and writing is handled. The serialized variables from the PlayerData object are either stored or loaded from file.


### SceneController.cs
Responsible for the transitions between scenes and handles the UI element changes in different scenes.

### SelectObject.cs
This script handles the user's "grabbing" mechanism. This takes care of the player holding objects.

### SelectStick.cs
This script takes care of the events associated with the stick in the During typhoon scene, such as grabbing the stick before the still water scenario and then dropping it after the user crosses the water.

### SetLPGElecActiveState.cs
Checks for hasElectricity/hasLPG variables and disables the respective GameObjects (whichever is false).

### ShowObjectName.cs
This script is responsible for the showing of object names once the reticle hovers on one.

### SuppliesKitGame.cs
This script is attached to the Checklist object.This is responsible for the spawning and deactivating of the supplies in the house, rearrangement of the checklist, and for transitioning to the next scene after the Emergency Supplies Kit game.

### TurnOff.cs
This script is responsible for the scene for turning off the LPG tank and main switch.

### VideoManager.cs
This script controls the videos played during the Introduction scene. 

### Walk.cs
This script is responsible for making the user walk around the environment. Attached to Player object.

### Win_Script.cs
This handles the event where the user reaches the evacuation center in the During typhoon scenario. This shows a congratulatory pop-up message then returns the user back to the menu.


