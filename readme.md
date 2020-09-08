# SFInput
A basic input manager for use with SFML.Net.

SFInput is a simple action based input manager using SFML.Net for mouse and keyboard input and XInput for joystick 
input. SFInput is written entirely for my own usage in my own projects but I am open to fixing any issues that may be
found.

## Usage
See `SFInputTest/Example.cs" for example code and usage.

## TO-DO
### Possibilities
- Design a simple GUI application for creating/modifying action sets.

## Changelog
### Version 0.3.1
- Now only XInput is used for the joystick backend making action loading, saving and usage more consistent.

### Version 0.3.0
- Restructured project to reduce repeated code and seperate SFML and XInput joystick implementations.
- Changing between SFML and XInput joystick input backends will invalidate actions and is now stated.

### Version 0.2.0
- Now XInput can be used for the joystick backend. This is enabled by default and can be changed on the fly.
- Cleaned up API for less verbose usage. 

### Version 0.1.0
- Initial release.
