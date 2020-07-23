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
using SharpLogger;

namespace SFInput
{
	/// <summary>
	///   Singleton input manager class.
	/// </summary>
	public class Input
	{
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
			Joystick = new JoystickManager();
			Actions  = new ActionSet();
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
		///   Joystick manager.
		/// </summary>
		public JoystickManager Joystick
		{
			get; private set;
		}

		/// <summary>
		///   Currently mapped actions.
		/// </summary>
		public ActionSet Actions
		{
			get; private set;
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
				return Joystick.IsPressed( 0, but );

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
				return Joystick.JustPressed( 0, but );

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
				return Joystick.JustReleased( 0, but );

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
				return Joystick.GetAxis( 0, axis );

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
				return Joystick.GetLastAxis( 0, axis );

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
		public float AxisDelta( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return Mouse.GetAxisDelta( axis );
			else if( dev == InputDevice.Joystick )
				return Joystick.GetAxisDelta( 0, axis );

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
		public bool IsAxisPressed( InputDevice dev, string axis )
		{
			if( dev == InputDevice.Mouse )
				return Mouse.IsAxisPressed( axis );
			else if( dev == InputDevice.Joystick )
				return Joystick.IsAxisPressed( 0, axis );

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
				return Joystick.AxisJustPressed( 0, axis );

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
				return Joystick.AxisJustReleased( 0, axis );

			return false;
		}

		/// <summary>
		///   Checks if the input mapped to action is pressed.
		/// </summary>
		/// <param name="action">
		///   The name of the mapped action.
		/// </param>
		/// <returns>
		///   True if action is valid, mapped and pressed, otherwise false.
		/// </returns>
		public bool IsPressed( string action )
		{
			return Actions.Get( action )?.IsPressed ?? false;
		}
		/// <summary>
		///   Checks if the input mapped to action was just pressed.
		/// </summary>
		/// <param name="action">
		///   The name of the mapped action.
		/// </param>
		/// <returns>
		///   True if action is valid, mapped and was just pressed, otherwise false.
		/// </returns>
		public bool JustPressed( string action )
		{
			return Actions.Get( action )?.JustPressed ?? false;
		}
		/// <summary>
		///   Checks if the input mapped to action was just released.
		/// </summary>
		/// <param name="action">
		///   The name of the mapped action.
		/// </param>
		/// <returns>
		///   True if action is valid, mapped and was just released, otherwise false.
		/// </returns>
		public bool JustReleased( string action )
		{
			return Actions.Get( action )?.JustReleased ?? false;
		}

		/// <summary>
		///   Updates input device states.
		/// </summary>
		public void Update()
		{
			Keyboard.Update();
			Mouse.Update();
			Joystick.Update();
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
			return Actions.LoadFromFile( path );
		}
		/// <summary>
		///   Saves the current action set to an optional path
		/// </summary>
		/// <param name="path">
		///   The path to save the action set to, or null to use the default path.
		/// </param>
		/// <returns></returns>
		public bool SaveToFile( string path = null )
		{
			if( path == null )
				path = DefaultPath;

			try
			{
				File.WriteAllText( path, Actions.ToString() );
			}
			catch( Exception e )
			{
				return Logger.LogReturn( "Unable to write input settings to file: " + e.Message + ".", false, LogType.Error );
			}

			return true;
		}

		private static volatile Input _instance;
		private static readonly object _syncRoot = new object();
	}
}
