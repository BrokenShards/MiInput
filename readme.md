# SFInput
A basic input manager for use with SFML.Net.

## Usage
Use `Input.Manager` to query input. `Update` must be called each frame before input logic.

```
using System;
using SFML.Graphics;
using SFML.Window;
using SFInput;

static class Program
{
	static void Main( string args[] )
	{
		using( RenderWindow window = new RenderWindow( new VideoMode( 640, 480 ), "SFInput", Styles.Close ) )
		{
			window.Closed += OnClose;

			while( window.IsOpen )
			{
				window.DispatchEvents();

				// Update input managers.
				Input.Manager.Update();

				if( Input.Manager.JustPressed( InputDevice.Keyboard, "Space" ) )
					System.Console.WriteLine( "Space just pressed." );
				
				if( Input.Manager.JustPressed( InputDevice.Mouse, "Left" ) )
					System.Console.WriteLine( "Left mouse button just clicked." );
				
				if( Input.Manager.JustPressed( InputDevice.Joystick, "0" ) )
					System.Console.WriteLine( "Joystick button 0 just pressed." );

				window.Clear();
				window.Display();
			}
		}
	}

	static void OnClose( object sender, System.EventArgs e )
	{
		( sender as RenderWindow ).Close();
	}
}

```

## To Do
- Implement XInput backend for joysticks and a way to switch on the fly.

## Possibilities
- Design a simple GUI application for creating/modifying action sets.

## Changelog

### Version 0.1.0
- Initial release.
