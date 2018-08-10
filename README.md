# Platform-Fighter-Engine
## Overview
WIP framework in Unity. Goal is to be able to create games similar to the **Super Smash Bros** series.  
Currently built for Unity 2018.2.2f1. Currently these packages are in use.
* [Unity's new Input System](https://github.com/Unity-Technologies/InputSystem) (Can be ignored, only used for the example project)
* TextMeshPro
* Post-Processing

#### Contact Me
* [Twitter](https://twitter.com/christides11)

## Upcoming Features
* Online Support (via LLAPI implementation)
	* Rollback with delay settings (similar to GGPO)
* 3 Example Characters & 3 Example Stages
* Standard Gamemodes
	* FFA and Teams
* Replay Mode
* Training Mode
	* Hitbox and Attack Data
	* Save States
	* Play/Record Features, Playback Mode
	* Input Display
	* Slow Motion
	* Health, Meter, and Character Settings
	* Infinite Block
* InControl/ReWired/cInput 2 support

## Wanted Features
* PlayMaker Support
* Bolt Support

## Resources
* [SSBM control stick input-output maps](https://imgur.com/a/2na5b)
* [SSB Wiki](https://www.ssbwiki.com/)
* [Melee Light](https://github.com/schmooblidon/meleelight)
* Networking Resources
	* [Rollback Netcode in INVERSUS](http://blog.hypersect.com/rollback-networking-in-inversus/)
	* [Understanding Fighting Game Networking](http://mauve.mizuumi.net/2012/07/05/understanding-fighting-game-networking/)
	* [Introduction to Network Physics](https://gafferongames.com/post/introduction_to_networked_physics/)
	* [Fast-Paced Multiplayer](http://www.gabrielgambetta.com/client-server-game-architecture.html)
* Smash Researcher twitters
	* https://twitter.com/BenArthur_7
	* https://twitter.com/drafix570
	* https://twitter.com/Ruben_dal
	* [https://twitter.com/Meshima_](https://twitter.com/Meshima_)
	
## Current Issues
* Inability to have multiple controllers on CSS
	* This is a problem with Unity's EventSystem/UI itself, and isn't really something in the scope of this project. Your best bet is to try [this UI replacement out](https://bitbucket.org/chris_drain/unity-ui/overview).
	
## 3rd Party Assets In-Use
* [FixedPointy](https://github.com/ijshahin/FixedPointy): Fixed-Point Math Library.
* [JsonDotNet](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347): For converting data into json to send over the network.
* [Unity-chan](https://assetstore.unity.com/packages/3d/characters/unity-chan-model-18705): For example project.

