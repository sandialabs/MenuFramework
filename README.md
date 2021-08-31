# `MenuFramework`
The `MenuFramework` is a reusable Extended Reality (XR) multi-level menu system for the [Unity
Development Platform](https://unity.com/). The menu structure is architected for reusability to suit whatever need the
developer may have for it. This project is a multi-level menu system that provides access to
Extended Reality (XR) applications/projects. The menu system is managed by a multi-level state
machine. There are potentially multiple instances under each project category. `MenuFramework's`
architecture, GUI, and functionality are based on our paper _[Extended Reality for Enhanced 
Training and Knowledge Capture](https://www.modsimworld.org/papers/2020/MODSIM_2020_paper_44_.pdf)_.

## Overview

The `MenuFramework` is designed to take in multiple Unity training projects. `MenuFramework` provides a menu systems for developers to have a centralized location/interface for different types of applications.

## Structure

**Main Menu** </br>
  - Set up main types of training you as the developer would like to see for the trainee to use.
     - Examples: Sequential Training, Target Recognition, Explore CAD

**Secondary Menus** </br>
  - Set the call to start up training scene

**Settings** </br>
  - Page with menu settings for looks and menu control movements

**Menu Commands** </br>
  - Prefabed menu pages with speech commands

## `GameManager`

The `GameManager` controls the basic functionality of the menu system. It unitilizes Unity's default `GameManager` name as well as the Unity [`dontDestroyOnLoad`](https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html) method call which allows the `GameManager` to be a controller for the application system in the background allowing for any changes or saving points to be put in place and stay persistent.

## MRTK Utilization

`MenuFramework` is utilizing [Microsoft's MixedRealityToolKit (MRTK)](https://github.com/microsoft/MixedRealityToolkit) to build up an industry standard set up for XR that allows for User Experience (UX) to be more ideal to the user.</br>
Parts Include:</br>
- **Bounding Box**
   - Resize panels
   - Move panels
- **Camera Control System**
   - Eye tracking
   - Head tracking
   - Hand tracking
- **Speech Control System**
   - Speech listen and function
- **Voice feedback Control System**
- **Button layout**
   - Tooltips
   - Interactable controls for animation controls

## Class/Methods

### `GameManager`
Description: Unity default class call. **Note:** the `GameManager` has a gear symbol within Unity. This class controls the menu system main methods:
   - `LoginGreeting()` - Sets main label description by passing in a description string into this method. This will play at application start.
   - `SetMainMenuLabel()` - Sets top main menu label to see the string value for the user to see.
   - `ToggleHelpCommands()` - Method used to toggle the help commands window on and off depending.
   - `TransparentBacking()` - Method used to set the menu systems backing off for transparency
   - `ResetCamera()` - Method used to set the camera back to the first position
   - LoadingScenes
     - `LoadScene()` - Method used to load in a specific scene by name.
     - `OnLoadOperationComplete()` - Method used to check loading of scenes while async is allowed.
     - `CheckSceneLoaded()` - Method used to check and see if a scene has already been loaded into the application.
     - `ForceClose()` or `ForceCloseSpecific()` or `ForceCloseAll()` - Methods used to close scenes and set them to inactive.
   - `EnableMultiScene()` - Method used to allow for multiple scenes to load in at once.
   - `NextHelpMenu()` - Allows for menu dynamic help menu switch.
   - `ExitApplication()` - Quit the application.

### `SceneOptionsScript`
Description: Class used to control text changes programatically. 

### `AudioFeedback`
Description: Utilizing MRTK for pushing speech out to HoloLens; this class has a customspeak method to pass in any string to speak out loud.


## Basic functionality:
Utilizing the MRTK built in functionality this allows for the basic functionality listed below.

**Bounding Box**
- The bounding box allows the user to resize the menu to what ever size they would like it to be and also move the menu around where ever they would like to set it.

**Follow Me**
- The menu system has the ability to utilize a Radial View that allows for the menu system to toggle on and off a menu "follow" ability. There is also functionality to have the menu follow you if you get too far away from it.

**Minimize/Maximize**
- The menu system has the ability to Minimize or Maximize simply by clicking the corresponding buttons. The default menu will minimize to a smaller size and automatically follow you.

**Settings**
- A settings page is on the menu to change any functionality to allow for the user to have a better experience.

**Help Commands**
- The menu system has voice commands built into it so you do not have to click you can just speak allowing the user to have different ways of navigating through sections of training or pages in the menu. There is a panel dedicated to help commands simply by saying Help Commands.
