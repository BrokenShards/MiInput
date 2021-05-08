////////////////////////////////////////////////////////////////////////////////
// InputMap.cs 
////////////////////////////////////////////////////////////////////////////////
//
// MiInput - A basic input manager for use with SFML.Net.
// Copyright (C) 2021 Michael Furlong <michaeljfurlong@outlook.com>
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

using MiCore;

namespace MiInput
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
			if( m1 is null || !m1.IsValid || m2 is null || !m2.IsValid )
				return false;

			if( m1.Device == m2.Device && m1.Type == m2.Type )
			{
				string val1 = string.IsNullOrWhiteSpace( m1.Value )    ? null : m1.Value.ToLower(),
				       neg1 = string.IsNullOrWhiteSpace( m1.Negative ) ? null : m1.Negative.ToLower(),
					   val2 = string.IsNullOrWhiteSpace( m2.Value )    ? null : m2.Value.ToLower(),
					   neg2 = string.IsNullOrWhiteSpace( m2.Negative ) ? null : m2.Negative.ToLower();

				return ( val1 is not null && val1 == neg2 ) || ( val2 is not null && val2 == neg1 );
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

				if( Device is InputDevice.Keyboard )
				{
					if( Type is not InputType.Button )
						return false;

					if( ( !string.IsNullOrWhiteSpace( Value )    && !KeyboardManager.IsKey( Value ) ) ||
						( !string.IsNullOrWhiteSpace( Negative ) && !KeyboardManager.IsKey( Negative ) ) )
						return false;
				}
				else if( Device is InputDevice.Mouse )
				{
					if( Type is InputType.Axis )
					{
						if( !MouseManager.IsAxis( Value ) )
							return false;
					}
					else if( Type is InputType.Button )
					{
						if( ( !string.IsNullOrWhiteSpace( Value )    && !MouseManager.IsButton( Value ) ) ||
							( !string.IsNullOrWhiteSpace( Negative ) && !MouseManager.IsButton( Negative ) ) )
							return false;
					}
				}
				else if( Device is InputDevice.Joystick )
				{
					if( Type is InputType.Axis )
					{
						if( !JoystickManager.IsAxis( Value ) )
							return false;
					}
					else if( Type is InputType.Button )
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
			if( ele is null )
				return Logger.LogReturn( "Failed loading InputMap: Null xml element.", false, LogType.Error );

			// Type
			{
				string loname = ele.Name.ToLower();
				Type = loname == "button" ? InputType.Button : ( loname == "axis" ? InputType.Axis : (InputType)( -1 ) );

				if( Type < 0 )
					return Logger.LogReturn( "Failed loading InputMap: Invalid input type.", false, LogType.Error );
			}

			// Device
			{
				if( !ele.HasAttribute( nameof( Device ) ) )
					return Logger.LogReturn( "Failed loading InputMap: No Device attribute.", false, LogType.Error );					
				if( !Enum.TryParse( ele.GetAttribute( nameof( Device ) ), true, out InputDevice dev ) )
					return Logger.LogReturn( "Failed loading InputMap: Invalid Device attribute.", false, LogType.Error );
				
				Device = dev;
			}

			// Value
			{
				if( !ele.HasAttribute( nameof( Value ) ) && !ele.HasAttribute( "Positive" ) &&
					!ele.HasAttribute( nameof( Negative ) ) )
					return Logger.LogReturn( "Failed loading InputMap: No Positive and Negative or Value attributes.", false, LogType.Error );

				string val = ele.GetAttribute( nameof( Value ) ),
				       neg = ele.GetAttribute( nameof( Negative ) );

				if( string.IsNullOrWhiteSpace( val ) )
					val = ele.GetAttribute( "Positive" );

				if( string.IsNullOrWhiteSpace( val ) )
					val = null;
				if( string.IsNullOrWhiteSpace( neg ) )
					neg = null;
				
				if( val is null && neg is null )
					return Logger.LogReturn( "Failed loading InputMap: Invalid Positive, Negative and/or Value attributes.", false, LogType.Error );

				if( Device is InputDevice.Keyboard )
				{
					if( val is not null && !KeyboardManager.IsKey( val ) )
						return Logger.LogReturn( "Failed loading InputMap: Invalid Positive or Value attribute.", false, LogType.Error );
					if( neg is not null && !KeyboardManager.IsKey( neg ) )
						return Logger.LogReturn( "Failed loading InputMap: Invalid Negative attribute.", false, LogType.Error );
				}
				else if( Device is InputDevice.Mouse )
				{
					if( Type is InputType.Axis )
					{
						if( !MouseManager.IsAxis( val ) )
							return Logger.LogReturn( "Failed loading InputMap: Unable to parse mouse axis.", false, LogType.Error );
					}
					else if( Type is InputType.Button )
					{
						if( val is not null && !MouseManager.IsButton( val ) )
							return Logger.LogReturn( "Failed loading InputMap: Invalid Positive or Value attribute.", false, LogType.Error );
						if( neg is not null && !MouseManager.IsButton( neg ) )
							return Logger.LogReturn( "Failed loading InputMap: Invalid Negative attribute.", false, LogType.Error );
					}
				}
				else if( Device is InputDevice.Joystick )
				{
					if( Type is InputType.Axis )
					{
						if( !JoystickManager.IsAxis( val ) )
							return Logger.LogReturn( "Failed loading InputMap: Unable to parse joystick axis.", false, LogType.Error );
					}
					else if( Type is InputType.Button )
					{
						if( val is not null && !JoystickManager.IsButton( val ) )
							return Logger.LogReturn( "Failed loading InputMap: Invalid Positive or Value attribute.", false, LogType.Error );
						if( neg is not null && !JoystickManager.IsButton( neg ) )
							return Logger.LogReturn( "Failed loading InputMap: Invalid Negative attribute.", false, LogType.Error );
					}
				}

				Value    = val ?? string.Empty;
				Negative = neg ?? string.Empty;
			}

			// Invert
			{
				if( ele.HasAttribute( nameof( Invert ) ) )
				{
					string invert = ele.GetAttribute( nameof( Invert ) );

					if( string.IsNullOrWhiteSpace( invert ) )
						return Logger.LogReturn( "Failed loading InputMap: Invalid Invert attribute.", false, LogType.Error );
					if( !bool.TryParse( invert, out bool i ) )
						return Logger.LogReturn( "Failed loading InputMap: Unable to parse Invert attribute.", false, LogType.Error );

					Invert = i;
				}
				
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
			StringBuilder sb = new();

			string spacer = Type == InputType.Button ? "        " : "      ";

			sb.Append( Type == InputType.Button ? "<Button " : "<Axis " )
				.Append( nameof( Device ) ).Append( "=\"" ).Append( Device.ToString() ).AppendLine( "\"" );

			if( Type is InputType.Axis )
			{
				sb.Append( spacer ).Append( nameof( Value ) ).Append( "=\"" )
					.Append( string.IsNullOrWhiteSpace( Value ) ? string.Empty : Value ).AppendLine( "\"" );
			}
			else if( Type is InputType.Button )
			{
				sb.Append( spacer ).Append( "Positive" ).Append( "=\"" )
					.Append( string.IsNullOrWhiteSpace( Value ) ? string.Empty : Value ).AppendLine( "\"" );

				sb.Append( spacer ).Append( nameof( Negative ) ).Append( "=\"" )
					.Append( string.IsNullOrWhiteSpace( Negative ) ? string.Empty : Negative ).AppendLine( "\"" );
			}

			sb.Append( spacer ).Append( nameof( Invert ) ).Append( "=\"" ).Append( Invert ).Append( "\"/>" );
			return sb.ToString();
		}
	}
}
