// Example.cs //

using System;
using System.IO;
using MiInput;
using SFML.Window;
using SFML.Graphics;

using Action = MiInput.Action;

namespace MiInputTest
{
	public static class Example
	{
		public const string FilePath = "input.xml";

		public static int RunExample()
		{
			int  exitVal = 0;
			bool running = true;

			using( RenderWindow window = new RenderWindow( new VideoMode( 640, 480 ), "MiInput", Styles.Close ) )
			{
				window.Closed += OnClose;

				while( running )
				{
					// Create action.
					{
						Action hor = new Action( "horizontal", 
						                         new InputMap( InputDevice.Joystick, InputType.Axis, "LeftStickX" ), 
						                         new InputMap( InputDevice.Keyboard, InputType.Button, "D", "A" ) );

						// Add action to action set, replacing an already existing action with the same ID.
						if( !Input.Manager.Actions.Add( hor, true ) )
						{
							Console.WriteLine( "Failed adding action to set (is the action valid?)" );
							exitVal = -1;
							running = false;
						}
					}

					// Retrieve assigned action.
					{
						Action a = Input.Manager.Actions[ "horizontal" ];

						if( a == null )
						{
							Console.WriteLine( "Unable to retrieve previously added action from the action set." );
							exitVal = -2;
							running = false;
						}
					}

					// Save action set to file.
					if( !Input.Manager.SaveToFile( FilePath, true ) )
					{
						Console.WriteLine( "Unable to write action set to file." );
						exitVal = -3;
						running = false;
					}

					// Load action set from file.
					if( !Input.Manager.LoadFromFile( FilePath ) )
					{
						Console.WriteLine( "Unable to load action set from file." );
						exitVal = -4;
						running = false;
					}

					// Access assigned action.
					{
						Action horizontal = Input.Manager.Actions[ "horizontal" ];

						if( horizontal == null )
						{
							Console.WriteLine( "horizontal action does not exist." );
							exitVal = -5;
							running = false;
						}
					}

					// Break out on success.
					break;
				}
				
				// For holding the current and previous values of the horizontal action we created before.
				float thisX = 0.0f,
				      lastX = 0.0f;

				while( running && window.IsOpen )
				{
					window.DispatchEvents();

					// Update input managers. This must be called every frame before polling for input.
					Input.Manager.Update();

					lastX = thisX;
					thisX = Input.Manager.Actions[ "horizontal" ].Value;

					// Only print horizontal action value if it changed.
					if( thisX != lastX )
						Console.WriteLine( "Horizontal: " + thisX.ToString() + "." );

					if( Input.Manager.JustPressed( InputDevice.Keyboard, "Space" ) )
						Console.WriteLine( "Space just pressed." );

					if( Input.Manager.JustPressed( InputDevice.Mouse, "Left" ) )
						Console.WriteLine( "Left mouse button just clicked." );
					if( Input.Manager.JustReleased( InputDevice.Mouse, "Left" ) )
						Console.WriteLine( "Left mouse button just released." );

					if( Input.Manager.JustPressed( InputDevice.Joystick, "0" ) )
						Console.WriteLine( "Joystick button 0 just pressed." );

					window.Clear();
					window.Display();
				}
			}

			return exitVal;
		}

		private static void OnClose( object sender, EventArgs e )
		{
			( sender as RenderWindow ).Close();
		}
	}
}
