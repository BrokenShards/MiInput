////////////////////////////////////////////////////////////////////////////////
// Input.cs 
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

using System;
using System.IO;
using System.Text;
using System.Xml;
using SharpLogger;
using SharpSerial;

namespace SFInput
{
	/// <summary>
	///   Singleton input manager class.
	/// </summary>
	public class Input : XmlLoadable
	{
		/// <summary>
		///   Maximum number of joysticks that can be connected at once.
		/// </summary>
		public const uint MaxJoysticks = 4;
		/// <summary>
		///   Minimum axis value that registers a button press.
		/// </summary>
		public const float AxisPressThreshold = 0.4f;

		/// <summary>
		///   Default input settings path.
		/// </summary>
		public const string DefaultPath = "input.xml";

		/// <summary>
		///   Checks if the given string represents a valid buttton/key for the given input device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="but">
		///   The button/key string.
		/// </param>
		/// <returns>
		///   True if the given string represents a valid button/key for the given input device, otherwise false.
		/// </returns>
		public static bool IsButton( InputDevice dev, string but )
		{
			if( dev == InputDevice.Keyboard )
				return KeyboardManager.IsKey( but );
			else if( dev == InputDevice.Mouse )
				return MouseManager.IsButton( but );
			else if( dev == InputDevice.Joystick )
				return JoystickManager.IsButton( but );

			return false;
		}
		/// <summary>
		///   Checks if the given string represents a valid axis for the given input device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="axis">
		///   The axis string.
		/// </param>
		/// <returns>
		///   True if the given string represents a valid axis for the given input device, otherwise false.
		/// </returns>
		public static bool IsAxis( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return MouseManager.IsAxis( axis );
			else if( dev == InputDevice.Joystick )
				return JoystickManager.IsAxis( axis );

			return false;
		}

		private Input()
		{
			Keyboard = new KeyboardManager();
			Mouse    = new MouseManager();
			Joystick = new JoystickManager[ MaxJoysticks ];
			Actions  = new ActionSet[ MaxJoysticks ];

			for( uint i = 0; i < MaxJoysticks; i++ )
			{
				Joystick[ i ] = new JoystickManager( i );
				Actions[ i ]  = new ActionSet();
			}
		}

		/// <summary>
		///   The singleton input manager instance.
		/// </summary>
		public static Input Manager
		{
			get
			{
				if( _instance == null )
				{
					lock( _syncRoot )
					{
						if( _instance == null )
						{
							_instance = new Input();
						}
					}
				}

				return _instance;
			}
		}

		/// <summary>
		///   Keyboard manager.
		/// </summary>
		public KeyboardManager Keyboard
		{
			get; private set;
		}
		/// <summary>
		///   Mouse manager.
		/// </summary>
		public MouseManager Mouse
		{
			get; private set;
		}

		/// <summary>
		///   Array of joystick managers for each player.
		/// </summary>
		public JoystickManager[] Joystick
		{
			get; private set;
		}

		/// <summary>
		///   The index of the first connected joystick.
		/// </summary>
		public uint FirstJoystick
		{
			get { return Joystick[ 0 ].FirstConnected; }
		}
		
		/// <summary>
		///   Currently mapped actions for each player.
		/// </summary>
		public ActionSet[] Actions
		{
			get; private set;
		}

		/// <summary>
		///   If the joystick at the given index is connected.
		/// </summary>
		/// <param name="player">
		///   The player index of the joystick.
		/// </param>
		/// <returns>
		///   True if the joystick is connected and false otherwise.
		/// </returns>
		public bool JoystickConnected( uint player )
		{
			if( player >= MaxJoysticks )
				return false;

			return Joystick[ player ].IsConnected;
		}

		/// <summary>
		///   Checks if the given button/key is pressed on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="but">
		///   The button/key to check.
		/// </param>
		/// <returns>
		///   True if the button/key is valid for the given device and is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( InputDevice dev, string but )
		{
			if( dev == InputDevice.Keyboard )
				return Keyboard.IsPressed( but );
			else if( dev == InputDevice.Mouse )
				return Mouse.IsPressed( but );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].IsPressed( but );
			}

			return false;
		}
		/// <summary>
		///   Checks if the given button/key has just been pressed on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="but">
		///   The button/key to check.
		/// </param>
		/// <returns>
		///   True if the button/key is valid for the given device and has just been pressed, otherwise false.
		/// </returns>
		public bool JustPressed( InputDevice dev, string but )
		{
			if( dev == InputDevice.Keyboard )
				return Keyboard.JustPressed( but );
			else if( dev == InputDevice.Mouse )
				return Mouse.JustPressed( but );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].JustPressed( but );
			}

			return false;
		}
		/// <summary>
		///   Checks if the given button/key has just been released on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="but">
		///   The button/key to check.
		/// </param>
		/// <returns>
		///   True if the button/key is valid for the given device and has just been released, otherwise false.
		/// </returns>
		public bool JustReleased( InputDevice dev, string but )
		{
			if( dev == InputDevice.Keyboard )
				return Keyboard.JustReleased( but );
			else if( dev == InputDevice.Mouse )
				return Mouse.JustReleased( but );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].JustReleased( but );
			}

			return false;
		}

		/// <summary>
		///   Gets the current value of the given axis on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The current value of the given axis on the given device if it is valid, otherwise 0.0f.
		/// </returns>
		public float GetAxis( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return Mouse.GetAxis( axis );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].GetAxis( axis );
			}

			return 0.0f;
		}
		/// <summary>
		///   Gets the previous value of the given axis on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The previous value of the given axis on the given device if it is valid, otherwise 0.0f.
		/// </returns>
		public float GetLastAxis( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return Mouse.GetLastAxis( axis );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].GetLastAxis( axis );
			}

			return 0.0f;
		}
		/// <summary>
		///   Gets the delta value of the given axis on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The delta value of the given axis on the given device if it is valid, otherwise 0.0f.
		/// </returns>
		public float GetAxisDelta( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return Mouse.AxisDelta( axis );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].AxisDelta( axis );
			}

			return 0.0f;
		}
		/// <summary>
		///   Checks if the given axis is pressed on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the axis is valid for the given device and is pressed, otherwise false.
		/// </returns>
		public bool AxisIsPressed( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return Mouse.AxisIsPressed( axis );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].AxisIsPressed( axis );
			}

			return false;
		}
		/// <summary>
		///   Checks if the given axis has just been pressed on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the axis is valid for the given device and has just been pressed, otherwise false.
		/// </returns>
		public bool AxisJustPressed( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return Mouse.AxisJustPressed( axis );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].AxisJustPressed( axis );
			}

			return false;
		}
		/// <summary>
		///   Checks if the given axis has just been released on the given device.
		/// </summary>
		/// <param name="dev">
		///   The input device.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the axis is valid for the given device and has just been released, otherwise false.
		/// </returns>
		public bool AxisJustReleased( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return Mouse.AxisJustReleased( axis );
			else if( dev == InputDevice.Joystick )
			{
				uint first = FirstJoystick;

				if( first < MaxJoysticks )
					return Joystick[ first ].AxisJustReleased( axis );
			}

			return false;
		}

		/// <summary>
		///   Checks if the input mapped to action is pressed.
		/// </summary>
		/// <param name="action">
		///   The name of the mapped action.
		/// </param>
		/// <param name="player">
		///   The player index.
		/// </param>
		/// <returns>
		///   True if index and action are valid, mapped and pressed, otherwise false.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///   If player greater than or equal to <see cref="MaxJoysticks"/>.
		/// </exception>
		public bool IsPressed( string action, uint player = 0 )
		{
			if( player >= Actions.Length )
				throw new ArgumentOutOfRangeException();

			return Actions[ player ].Get( action )?.IsPressed ?? false;
		}
		/// <summary>
		///   Checks if the input mapped to action was just pressed.
		/// </summary>
		/// <param name="action">
		///   The name of the mapped action.
		/// </param>
		/// <param name="player">
		///   The player index.
		/// </param>
		/// <returns>
		///   True if index and action are valid, mapped and was just pressed, otherwise false.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///   If player greater than or equal to <see cref="MaxJoysticks"/>.
		/// </exception>
		public bool JustPressed( string action, uint player = 0 )
		{
			if( player >= Actions.Length )
				throw new ArgumentOutOfRangeException();

			return Actions[ player ].Get( action )?.JustPressed ?? false;
		}
		/// <summary>
		///   Checks if the input mapped to action was just released.
		/// </summary>
		/// <param name="action">
		///   The name of the mapped action.
		/// </param>
		/// <param name="player">
		///   The player index.
		/// </param>
		/// <returns>
		///   True if index and action are valid, mapped and was just released, otherwise false.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///   If player greater than or equal to <see cref="MaxJoysticks"/>.
		/// </exception>
		public bool JustReleased( string action, uint player = 0 )
		{
			if( player >= Actions.Length )
				throw new ArgumentOutOfRangeException();

			return Actions[ player ].Get( action )?.JustReleased ?? false;
		}
		/// <summary>
		///   Gets the current value of the action.
		/// </summary>
		/// <param name="action">
		///   The name of the mapped action.
		/// </param>
		/// <param name="player">
		///   The player index.
		/// </param>
		/// <returns>
		///   The current value of the mapped action if it is valid, otherwise zero.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///   If player greater than or equal to <see cref="MaxJoysticks"/>.
		/// </exception>
		public float GetValue( string action, uint player = 0 )
		{
			if( player >= Actions.Length )
				throw new ArgumentOutOfRangeException();

			return Actions[ player ].Get( action )?.Value ?? 0.0f;
		}

		/// <summary>
		///   Updates input device states.
		/// </summary>
		public void Update()
		{
			Keyboard.Update();
			Mouse.Update();

			for( uint i = 0; i < MaxJoysticks; i++ )
			{
				Joystick[ i ].Player = i;
				Joystick[ i ].Update();
			}
		}

		/// <summary>
		///   Attempts to load the object from the xml element.
		/// </summary>
		/// <param name="element">
		///   The xml element.
		/// </param>
		/// <returns>
		///   True if the object was loaded successfully and false otherwise.
		/// </returns>
		public override bool LoadFromXml( XmlElement element )
		{
			if( element == null )
				return Logger.LogReturn( "Unable to load input: root node is null.", false, LogType.Error );
			if( element.Name.ToLower() != "input" )
				return Logger.LogReturn( "Unable to load input: root node name must be \"input\".", false, LogType.Error );

			uint player = 0;

			foreach( var x in element.SelectNodes( "action_set" ) )
			{
				if( player >= MaxJoysticks )
					break;

				Actions[ player ] = new ActionSet();

				if( !Actions[ player ].LoadFromXml( x as XmlElement ) )
					return Logger.LogReturn( "Unable to load action set: action loaded successfully but could not be added.", false, LogType.Error );

				player++;
			}

			return true;
		}

		/// <summary>
		///   Loads the action set from an optional path.
		/// </summary>
		/// <param name="path">
		///   The path to load the action set from, or null to use the default path.
		/// </param>
		/// <returns>
		///   True on success and false on failure.
		/// </returns>
		public bool LoadFromFile( string path = null )
		{
			if( string.IsNullOrWhiteSpace( path ) )
				path = DefaultPath;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load( path );

				return LoadFromXml( doc.DocumentElement );
			}
			catch( Exception e )
			{
				return Logger.LogReturn( "Unable to load input from file: " + e.Message + ".", false, LogType.Error );
			}
		}
		/// <summary>
		///   Saves the action set to an optional path, optionally overwriting any existing file.
		/// </summary>
		/// <param name="path">
		///   File path.
		/// </param>
		/// <param name="overwrite">
		///   If an already existing file should be overwritten.
		/// </param>
		/// <returns>
		///   True if the action set was successfully written to file, otherwise false.
		/// </returns>
		public bool SaveToFile( string path = null, bool overwrite = true )
		{
			if( string.IsNullOrWhiteSpace( path ) )
				path = DefaultPath;

			return ToFile( this, path, overwrite );
		}

		/// <summary>
		///   Converts the object to an xml string.
		/// </summary>
		/// <returns>
		///   Returns the object as an xml string.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine( "<input>" );

			for( uint i = 0; i < MaxJoysticks; i++ )
				sb.AppendLine( Actions[ i ].ToString( 1 ) );

			sb.Append( "</input>" );

			return sb.ToString();
		}

		private static volatile Input _instance;
		private static readonly object _syncRoot = new object();
	}
}
