# MiInput
A basic input manager for use with SFML.Net and XInput.

MiInput is the new name for my old SFInput library.

MiInput is a simple action based input manager using SFML.Net for mouse and keyboard input and 
XInput for joystick input. MiInput was written entirely for my own usage in my own projects, but 
if anyone else ends up using it, I am open to suggestions and will attempt to fix any issues.

## Dependencies
- SFML.Net `https://github.com/SFML/SFML.Net.git`
- MiCore `https://github.com/BrokenShards/MiCore.git`
- XInputDotNetPure `https://github.com/speps/XInputDotNet.git`

## Usage
See `MiInputTest/Example.cs` for example code and usage.

## Changelog

### Version 0.10.0
- Now `JoystickState` and `JoystickManager` just use the first connected joystick rather than 
  specifying the player index, because of this, `Input.Actions` is no longer an array.
- Updated MiCore to version 0.9.0.

### Version 0.9.0
- Changed SFML source to latest official SFML.Net repository.
- Updated MiCore to version 0.5.0.

### Version 0.8.0
- Renamed to `MiInput` and updated to use the `MiCore` library.

### Version 0.7.0
- Added `Input.LastDevice` to easily check the most recent input device type.
- When loading `ActionSet` or `Input` from xml, the element name is no longer checked to be
  consistant with other classes.
- Log messages are now more consistant and concise.

### Version 0.6.2
- Fixed issue where `Input.SaveToFile(string,bool)` was not using `Input.DefaultPath` when the
  given path is null.
- Updated SharpSerial to version 0.5.0.

### Version 0.6.1
- Added constructor and function to `ActionSet` for adding multiple `Action`s at once.
- Added functions to `KeyboardManager`, `MouseManager` and `JoystickManager` to easily detect if
  any keys or buttons are pressed, were just pressed, were just released, or if any axies have
  moved.

### Version 0.6.0
- Now SharpSerial is used as an interface to load and save `InputMap`, `Action`, `ActionSet` and 
  `Input` to and from file.
- `Input.Actions` is now an array containing an action set for each player rather than just a 
  single action set.
- Corrected joystick trigger axis logic.

### Version 0.5.2
- Now `XInputInterface.dll` is coppied to the build directory on a successful build.
- Updated SharpLogger to version 0.3.1.

### Version 0.5.1
- Now the xml documentation is built with the binary and is included in releases.

### Version 0.5.0
- Dependencies have been updated and are now included as binary files rather than as git submodules.
- Dependency source is now clearly shown in the readme.

### Version 0.4.0
- Actions now privately contain their input maps and have functions for managing them in order to 
  prevent adding input maps that are either inavlid or collide with existing input maps.
- Action sets now have the `Clear()` function for removing all actions at once.
- Added example code in `MiInputTest/Example.cs` and updated tests.

### Version 0.3.1
- Now only XInput is used for the joystick backend making action loading, saving and usage more 
  consistent.

### Version 0.3.0
- Restructured project to reduce repeated code and seperate SFML and XInput joystick 
  implementations.
- Changing between SFML and XInput joystick input backends will invalidate actions and is now 
  stated.

### Version 0.2.0
- Now XInput can be used for the joystick backend. This is enabled by default and can be changed 
  on the fly.
- Cleaned up API for less verbose usage.

### Version 0.1.0
- Initial release.
