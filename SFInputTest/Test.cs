////////////////////////////////////////////////////////////////////////////////
// Test.cs 
////////////////////////////////////////////////////////////////////////////////
//
// SFInput - A basic input manager for use with SFML.Net.
// Copyright (C) 2020 Michael Furlong <michaeljfurlong@outlook.com>
//
// This program is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free 
// Software Foundation, either version 3 of the License, or (at your option) 
// any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for 
// more details.
// 
// You should have received a copy of the GNU General Public License along with
// this program. If not, see <https://www.gnu.org/licenses/>.
//
////////////////////////////////////////////////////////////////////////////////

using System.IO;

using SFML.Graphics;
using SFML.Window;

using SFInput;
using SharpLogger;

namespace SFInputTest
{
	static class Test
	{
		const string InputPath = "input.xml";

		static int Main( string[] args )
		{
			int val = RunTests();
			System.Console.ReadLine();
			return val;
		}

		static int RunTests()
		{
			if( !ActionTest() )
				return Logger.LogReturn( "Action test failed.", -1, LogType.Error );
			if( !WindowTest() )
				return Logger.LogReturn( "Window test failed.", -1, LogType.Error );

			Logger.Log( "Testing successful." );
			return 0;
		}

		static bool ActionTest()
		{ 
			Action act = new Action( "test" );
			act.Inputs.Add( new InputMap( InputDevice.Keyboard, InputType.Button, "K" ) );

			if( !Input.Manager.Actions.Add( act ) )
				return Logger.LogReturn( "Unable to add valid action to action set.", false, LogType.Error );

			Action a = Input.Manager.Actions[ "test" ];
			if( a == null )
				return Logger.LogReturn( "Unable to retrieve previously added action from the action set.", false, LogType.Error );

			Logger.HideLogs = true;
			bool result = Input.Manager.Actions.Add( a, false );
			Logger.HideLogs = false;

			if( result )
				return Logger.LogReturn( "Input manager allowed adding an action that already exists when replace is false.", false, LogType.Error );

			if( !Input.Manager.SaveToFile( InputPath ) )
				return Logger.LogReturn( "Input manager failed saving to file.", false, LogType.Error );
			if( !Input.Manager.LoadFromFile( InputPath ) )
				return Logger.LogReturn( "Input manager failed loading from file.", false, LogType.Error );

			if( !Input.Manager.Actions.Contains( a ) )
				return Logger.LogReturn( "Lost mapped input after loading from file.", false, LogType.Error );
			if( Input.Manager.Actions[ "test" ].Name != act.Name )
				return Logger.LogReturn( "Input manager did not load from file correctly.", false, LogType.Error );

			try
			{
				File.Delete( InputPath );
			}
			catch
			{ }

			return true;
		}

		static bool WindowTest()
		{
			Action test = new Action( "test" );
			test.Inputs.Add( new InputMap( InputDevice.Keyboard, InputType.Button, "D", "A" ) );

			if( !Input.Manager.Actions.Add( test, true ) )
				return Logger.LogReturn( "Unable to add test action to input manager.", false, LogType.Error );

			uint press = 0;

			Logger.Log( "Press D key." );

			using( RenderWindow window = new RenderWindow( new VideoMode( 640, 480 ), "SFInput Test", Styles.Close ) )
			{
				window.Closed += OnClose;

				while( window.IsOpen )
				{
					window.DispatchEvents();

					Input.Manager.Update();

					if( press == 0 && Input.Manager.Actions[ "test" ].IsPressed )
					{
						Logger.Log( "Press A key." );
						press++;
					}

					if( press == 1 && Input.Manager.Actions[ "test" ].IsNegative )
						press++;

					if( press > 1 )
						window.Close();

					window.Clear();

					window.Display();
				}
			}

			Input.Manager.Actions.Remove( "test" );
			return true;
		}

		private static void OnClose( object sender, System.EventArgs e )
		{
			( sender as RenderWindow ).Close();
		}
	}
}
