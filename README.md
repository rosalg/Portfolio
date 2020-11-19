# Gilbert Rosal's Portfolio

Hello and welcome to Gilbert Rosal's project repository. This file describes each project.

This repository contains all of Gilbert's coding projects starting from Fall 2018.

At the moment there are three projects:

# Stanford Scrappers
This project is contained in the Stanford Scrappers folder. The folder contains the Java code for Gilbert's 106A Graphics Contest entry Stanford Scrappers, an independently developed 2-player fighting game that uses a keyboard interface to allow players to menu, read instructions and view credits, select from one of 6 maps, then fight one another. Gilbert finished this project in exactly 1 week over Thanksgiving break of 2018.

The main code for the game is in the Game folder, in GraphicsClass.java. This file contains the main update loop, keeping track of game state, I/O from the user on the keyboard, and updating player sprites based on key presses.

The assets folder contains any audio or jpg files used in the game. I TAKE NO CREDIT FOR ANYTHING IN THESE FOLDERS.

The CharacterModels folder contains all the java files declaring different body parts of the sprite, then combines them all into a GCompound so that each drawn shape does not have to be drawn individually.

Lastly, the Characters folder contains the modules for firing a projectile and the players actually fighting.

The full project is contained in the folder.

# Coin
This project is made up of the Assets, Packages, and Project Settings folders. The game, titled Coin, is a single-player puzzle app developed using C# and Unity2D that has mirror reflections of the sprite that both move when one moves, but are faced with a different set of obstacles. The player must get both sprites to an exit at the same time. This game is in the midst of development, containing 45 levels, a self-made soundtrack, and self-made artwork.

The Assets folder contain self-describing folders, as well as scripts for the game in C#. The scripts are labelled to correspond with what each script manages. The only script I did NOT make myself was the CharacterController2D, however I made modifications of my own to make it work for the purposes of this game.

The full game is available on mobile platforms:
iOS - https://apps.apple.com/us/app/coin-a-tale-of-two-sides/id1506452163<br/>
Google PlayStore -https://play.google.com/store/apps/details?id=com.Leggs.Coin

# SuperSmashCS107E
This project is made up of four primary modules: gameplay, controller, player, graphics library.

The gameplay module is in charge of managing all sprites, updating information based on information communicated by the controller module. The controller module is in charge of initializing pins on the Raspberry Pi as I/O pins that then read information from the Gamecube remotes, utilizing getter functions to obtain information about the buttons and joysticks contained in a 64 bit array. The player module is in charge of handling the individual player, controlling jumping, movement, and redrawing of the player. Lastly, the gl module contains helper functions to draw to the framebuffer.

I worked on the gameplay, controller, and player modules. My partner worked on the GL module, extending it from the assignment we had in the class.

The full project is contained in the folder, however GameCube remotes need to be connected to a breadboard in order for the game to be playable.

# Unreal
This project is a collection of the lesson and assignments from Stanford's CS193u: Video Game Development in C++ and Unreal.

In this class, we learned about concepts including networking, basic movement, materials, animations, actions, event bindings, blueprints, AI, UI and widget creation, projectile based combat, and managing client/server interactions.

The full download is available here: https://github.com/rosalg/ActionRogueLike

# Blueshot
This project was the final project for MUSIC257: Neuroplasiticity and Musical Gaming.

Blueshot is a 2-player cooperative game where players can connect over the internet to battle incoming waves of enemies. Each player can either see the color of an enemy (red, green, blue) or cannot see the color. If the player cannot see the color, then that enemy is trying to kill them. That player must kill the enemy before it reaches them by shooting it with the correct color. The teammate is the player who can see the enemies color. Using a ping, the teammate must then communicate to the player being chased what color the enemy is.

The game's goal is to train users in two important skills: empathizing and low-level object formation. Firstly, by forcing users to experience running and communicating, we hope to generate a better ability to empathize with struggle. Secondly, by creating an association between sound and object, we hope to improve the response of brain cells that attach meaning to sound.

The game was made using Unity3D in a group of 4, with version control handled through Unity. I was in charge of managing enemies, audio (both production and scripting), and bug-fixing networking issues. The game was layered over a package that leveraged mirroring in order to support networking.

The full download for Mac devices is available here: https://sweyns.itch.io/blueshot
