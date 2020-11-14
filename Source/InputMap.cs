////////////////////////////////////////////////////////////////////////////////
// InputMap.cs 
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
using System.Text;
using System.Xml;

using SharpSerial;
using SharpLogger;

namespace SFInput
{
	/// <summary>
	///   Enumeration of possible input devices.
	/// </summary>
	public enum InputDevice
	{
		/// <summary>
		///   Keyboard input.
		/// </summary>
		Keyboard,

		/// <summary>
		///   Mouse input.
		/// </summary>
		Mouse,

		/// <summary>
		///   Joystick/Controller input.
		/// </summary>
		Joystick,

		/// <summary>
		///   Total values.
		/// </summary>
		COUNT
	}

	/// <summary>
	///   Enumeration of possible input types.
	/// </summary>
	public enum InputType
	{
		/// <summary>
		///   A key or button press.
		/// </summary>
		Button,

		/// <summary>
		///   Axis value.
		/// </summary>
		Axis,

		/// <summary>
		///   Total values.
		/// </summary>
		COUNT
	}	

	/// <summary>
	///   Used to map inputs.
	/// </summary>
	[Serializable]
	public class InputMap : XmlLoadable
	{
		/// <summary>
		///   Checks if two input maps collide with eachother.
		/// </summary>
		/// <param name="m1">
		///   The first input map.
		/// </param>
		/// <param name="m2">
		///   The second input map.
		/// </param>
		/// <returns>
		///   True if both input maps are valid and collide with eachother.
		/// </returns>
		public static bool Collides( InputMap m1, InputMap m2 )
		{
			if( m1 == null || !m1.IsValid || m2 == null || !m2.IsValid )
				return false;

			if( m1.Device == m2.Device && m1.Type == m2.Type )
			{
				string val1 = string.IsNullOrWhiteSpace( m1.Value )    ? null : m1.Value.ToLower(),
				       neg1 = string.IsNullOrWhiteSpace( m1.Negative ) ? null : m1.Negative.ToLower(),
					   val2 = string.IsNullOrWhiteSpace( m2.Value )    ? null : m2.Value.ToLower(),
					   neg2 = string.IsNullOrWhiteSpace( m2.Negative ) ? null : m2.Negative.ToLower();

				return ( val1 != null && val1 == neg2 ) || ( val2 != null && val2 == neg1 );
			}

			return false;
		}

		/// <summary>
		///   Constructor.
		/// </summary>
		public InputMap()
		{
			Device   = InputDevice.Keyboard;
			Type     = InputType.Button;
			Value    = string.Empty;
			Negative = string.Empty;
			Invert   = false;
		}
		/// <summary>
		///   Constructs the map with the given values.
		/// </summary>
		/// <param name="dev">
		///   Input device.
		/// </param>
		/// <param name="typ">
		///   Input type.
		/// </param>
		/// <param name="val">
		///   Positive input value.
		/// </param>
		/// <param name="neg">
		///   Negative input value.
		/// </param>
		public InputMap( InputDevice dev, InputType typ = 0, string val = null, string neg = null )
		{
			Device   = dev;
			Type     = typ;
			Value    = string.IsNullOrWhiteSpace( val ) ? string.Empty : val.Trim();
			Negative = string.IsNullOrWhiteSpace( neg ) ? string.Empty : neg.Trim();
			Invert   = false;
		}
		/// <summary>
		///   Copy constructor.
		/// </summary>
		/// <param name="i">
		///   The map to copy from.
		/// </param>
		public InputMap( InputMap i )
		{
			Device   = i.Device;
			Type     = i.Type;
			Value    = new string( i.Value.ToCharArray() );
			Negative = new string( i.Negative.ToCharArray() );
			Invert   = i.Invert;
		}

		/// <summary>
		///   If the input map is valid.
		/// </summary>
		public bool IsValid
		{
			get
			{
				if( Device < 0 || (int)Device >= Enum.GetNames( typeof( InputDevice ) ).Length ||
				    Type   < 0 || (int)Type   >= Enum.GetNames( typeof( InputType ) ).Length ||
					( string.IsNullOrWhiteSpace( Value ) && string.IsNullOrWhiteSpace( Negative ) ) )
					return false;

				if( Device == InputDevice.Keyboard )
				{
					if( Type != InputType.Button )
						return false;

					if( ( !string.IsNullOrWhiteSpace( Value )    && !KeyboardManager.IsKey( Value ) ) ||
						( !string.IsNullOrWhiteSpace( Negative ) && !KeyboardManager.IsKey( Negative ) ) )
						return false;
				}
				else if( Device == InputDevice.Mouse )
				{
					if( Type == InputType.Axis )
					{
						if( !MouseManager.IsAxis( Value ) )
							return false;
					}
					else if( Type == InputType.Button )
					{
						if( ( !string.IsNullOrWhiteSpace( Value )    && !MouseManager.IsButton( Value ) ) ||
							( !string.IsNullOrWhiteSpace( Negative ) && !MouseManager.IsButton( Negative ) ) )
							return false;
					}
				}
				else if( Device == InputDevice.Joystick )
				{
					if( Type == InputType.Axis )
					{
						if( !JoystickManager.IsAxis( Value ) )
							return false;
					}
					else if( Type == InputType.Button )
					{
						if( ( !string.IsNullOrWhiteSpace( Value ) && !JoystickManager.IsButton( Value ) ) ||
							( !string.IsNullOrWhiteSpace( Negative ) && !JoystickManager.IsButton( Negative ) ) )
							return false;
					}
				}

				return true;
			}
		}
		
		/// <summary>
		///   Input device.
		/// </summary>
		public InputDevice Device { get; set; }
		/// <summary>
		///   Input type.
		/// </summary>
		public InputType Type { get; set; }
		/// <summary>
		///   Positive input.
		/// </summary>
		public string Value { get; set; }
		/// <summary>
		///   Negative input.
		/// </summary>
		public string Negative { get; set; }
		/// <summary>
		///   If the inputs should be inversed.
		/// </summary>
		public bool Invert { get; set; }

		/// <summary>
		///   Loads data from an xml element.
		/// </summary>
		/// <param name="ele">
		///   The element to load data from.
		/// </param>
		/// <returns>
		///   True if loaded successfully and false otherwise.
		/// </returns>
		public override bool LoadFromXml( XmlElement ele )
		{
			if( ele == null )
				return Logger.LogReturn( "Unable to load input map from a null xml element.", false, LogType.Error );

			// Type
			{
				string loname = ele.Name.ToLower();

				Type = loname == "button" ? InputType.Button : ( loname == "axis" ? InputType.Axis : (InputType)( -1 ) );

				if( Type < 0 )
					return Logger.LogReturn( "Trying to load input map with invalid input type.", false, LogType.Error );
			}

			// Device
			{
				string device = ele.Attributes[ "device" ]?.Value;

				if( device == null )
					return Logger.LogReturn( "Trying to load input map with no device attribute.", false, LogType.Error );
				else if( !Enum.TryParse( device, true, out InputDevice dev ) )
					return Logger.LogReturn( "Trying to load input map with an invalid device attribute.", false, LogType.Error );
				else
					Device = dev;
			}

			// Value
			{
				string val = ele.Attributes[ "value" ]?.Value ?? ele.Attributes[ "positive" ]?.Value,
				       neg = ele.Attributes[ "negative" ]?.Value;

				if( string.IsNullOrWhiteSpace( val ) )
					val = null;
				if( string.IsNullOrWhiteSpace( neg ) )
					neg = null;

				if( val == null && neg == null )
					return Logger.LogReturn( "Trying to load input map with no positive and negative or value attributes.", false, LogType.Error );

				if( Device == InputDevice.Keyboard )
				{
					if( val != null && !KeyboardManager.IsKey( val ) )
						return Logger.LogReturn( "Trying to load input map with invalid positive or value attribute.", false, LogType.Error );
					if( neg != null && !KeyboardManager.IsKey( neg ) )
						return Logger.LogReturn( "Trying to load input map with invalid negative attribute.", false, LogType.Error );
				}
				else if( Device == InputDevice.Mouse )
				{
					if( Type == InputType.Axis )
					{
						if( !MouseManager.IsAxis( val ) )
							return Logger.LogReturn( "Unable to load input map; failed parsing mouse axis.", false, LogType.Error );
					}
					else if( Type == InputType.Button )
					{
						if( val != null && !MouseManager.IsButton( val ) )
							return Logger.LogReturn( "Trying to load input map with invalid positive or value attribute.", false, LogType.Error );
						if( neg != null && !MouseManager.IsButton( neg ) )
							return Logger.LogReturn( "Trying to load input map with invalid negative attribute.", false, LogType.Error );
					}
				}
				else if( Device == InputDevice.Joystick )
				{
					if( Type == InputType.Axis )
					{
						if( !JoystickManager.IsAxis( val ) )
							return Logger.LogReturn( "Unable to load input map; failed parsing joystick axis.", false, LogType.Error );
					}
					else if( Type == InputType.Button )
					{
						if( val != null && !JoystickManager.IsButton( val ) )
							return Logger.LogReturn( "Trying to load input map with invalid positive or value attribute.", false, LogType.Error );
						if( neg != null && !JoystickManager.IsButton( neg ) )
							return Logger.LogReturn( "Trying to load input map with invalid negative attribute.", false, LogType.Error );
					}
				}

				Value    = val ?? string.Empty;
				Negative = neg ?? string.Empty;
			}

			// Invert
			{
				string invert = ele.Attributes[ "invert" ]?.Value;

				if( invert == null )
					return Logger.LogReturn( "Trying to load input map with no invert attribute.", false, LogType.Error );

				if( !bool.TryParse( invert, out bool i ) )
					return Logger.LogReturn( "Trying to load input map with an invalid invert attribute.", false, LogType.Error );

				Invert = i;
			}

			return true;
		}

		/// <summary>
		///   Gets the xml file representation of the object as a string with no added indentation.
		/// </summary>
		/// <returns>
		///   The xml object data as a string.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			string spacer = Type == InputType.Button ? "        " : "      ";

			sb.Append( Type == InputType.Button ? "<button " : "<axis " );
			sb.Append( "device=\"" );
			sb.Append( Device.ToString() );
			sb.AppendLine( "\"" );

			if( Type == InputType.Axis )
			{
				sb.Append( spacer );
				sb.Append( "value=\"" );
				sb.Append( string.IsNullOrWhiteSpace( Value ) ? string.Empty : Value );
				sb.AppendLine( "\"" );
			}
			else if( Type == InputType.Button )
			{
				sb.Append( spacer );
				sb.Append( "positive=\"" );
				sb.Append( string.IsNullOrWhiteSpace( Value ) ? string.Empty : Value );
				sb.AppendLine( "\"" );

				sb.Append( spacer );
				sb.Append( "negative=\"" );
				sb.Append( string.IsNullOrWhiteSpace( Negative ) ? string.Empty : Negative );
				sb.AppendLine( "\"" );
			}

			sb.Append( spacer );
			sb.Append( "invert=\"" );
			sb.Append( Invert );
			sb.Append( "\"/>" );

			return sb.ToString();
		}
	}
}
