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

			// Create action.
			Action act = new Action( "horizontal" );
			act.Inputs.Add( new InputMap( InputDevice.Keyboard, InputType.Button, "A", "D" ) );

			// Add action to action set.
			if( !Input.Manager.Actions.Add( act ) )
				return Logger.LogReturn( "Unable to add valid action to action set.", false, LogType.Error );

			// Retrieve assigned action.
			Action a = Input.Manager.Actions[ "horizontal" ];
			if( a == null )
				return Logger.LogReturn( "Unable to retrieve previously added action from the action set.", false, LogType.Error );

			while( window.IsOpen )
			{
				window.DispatchEvents();

				// Update input managers.
				Input.Manager.Update();

				float h = Input.Manager.Actions[ "horizontal" ].Value;

				if( Math.Abs( h ) > 0.02f )
					System.Console.WriteLine( "Horizontal: " + h.ToString() + "." );

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
