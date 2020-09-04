////////////////////////////////////////////////////////////////////////////////
// Action.cs 
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using SharpID;
using SharpLogger;

namespace SFInput
{
	public class Action : INamable, IEnumerable<InputMap>
	{
		/// <summary>
		///   Constructor.
		/// </summary>
		public Action()
		{
			Name   = string.Empty;
			Inputs = new List<InputMap>();
		}
		/// <summary>
		///   Copy constructor.
		/// </summary>
		/// <param name="a">
		///   The object to copy.
		/// </param>
		public Action( Action a )
		{
			Name   = new string( a.Name.ToCharArray() );
			Inputs = a.Inputs.Count > 0 ? new List<InputMap>( a.Inputs.Count ) : new List<InputMap>();

			for( int i = 0; i < a.Inputs.Count; i++ )
				Inputs[ i ] = new InputMap( a.Inputs[ i ] );
		}
		/// <summary>
		///   Constructs the object with the given positive name.
		/// </summary>
		/// <param name="name">
		///   The positive name.
		/// </param>
		public Action( string name )
		{
			Name   = string.IsNullOrWhiteSpace( name ) ? string.Empty : name;
			Inputs = new List<InputMap>();
		}

		/// <summary>
		///   Positive action name.
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set { m_name = string.IsNullOrWhiteSpace( value ) ? string.Empty : Naming.AsValid( value ); }
		}

		/// <summary>
		///   Mapped inputs.
		/// </summary>
		public List<InputMap> Inputs
		{
			get; private set;
		}

		/// <summary>
		///   Returns the current value of the mapped input.
		/// </summary>
		public float Value
		{
			get
			{
				if( Inputs.Count == 0 )
					return 0.0f;
								
				for( int i = 0; i < Inputs.Count; i++ )
				{
					InputMap map = Inputs[ i ];

					if( !map.IsValid )
						continue;

					if( map.Type == InputType.Axis )
					{
						float v = map.Device == InputDevice.Mouse ?
						          Input.Manager.Mouse.GetAxis( Inputs[ i ].Value ) :
								  Input.Manager.Joystick[ Input.Manager.FirstJoystick ].GetAxis( Inputs[ i ].Value );

						if( v != 0.0f )
							return map.Invert ? -v : v;
					}
					else if( map.Type == InputType.Button )
					{
						bool p = map.Device == InputDevice.Keyboard ? Input.Manager.Keyboard.IsPressed( Inputs[ i ].Value ) :
						       ( map.Device == InputDevice.Mouse    ? Input.Manager.Mouse.IsPressed( Inputs[ i ].Value ) :
						       ( map.Device == InputDevice.Joystick ? Input.Manager.Joystick[ Input.Manager.FirstJoystick ].IsPressed( Inputs[ i ].Value ) : false ) );
						bool n = map.Device == InputDevice.Keyboard ? Input.Manager.Keyboard.IsPressed( Inputs[ i ].Negative ) :
							   ( map.Device == InputDevice.Mouse    ? Input.Manager.Mouse.IsPressed( Inputs[ i ].Negative ) :
							   ( map.Device == InputDevice.Joystick ? Input.Manager.Joystick[ Input.Manager.FirstJoystick ].IsPressed( Inputs[ i ].Negative ) : false ) );

						if( ( p && n ) || ( !p && !n ) )
							continue;

						if( map.Invert )
						{
							p = !p;
							n = !n;
						}

						if( p )
							return 1.0f;
						if( n )
							return -1.0f;
					}
				}

				return 0.0f;
			}
		}

		/// <summary>
		///   If the mapped inputs are classed as positive/pressed.
		/// </summary>
		public bool IsPositive
		{
			get
			{
				return Value >= Input.AxisPressThreshold;
			}
		}
		/// <summary>
		///   If the mapped inputs are classed as negative.
		/// </summary>
		public bool IsNegative
		{
			get
			{
				return Value <= -Input.AxisPressThreshold;
			}
		}

		/// <summary>
		///   If any of the mapped inputs are pressed.
		/// </summary>
		public bool IsPressed
		{
			get
			{
				foreach( InputMap map in Inputs )
				{
					bool pos = false, 
					     neg = false;

					if( map.Type == InputType.Button )
					{
						pos = Input.Manager.IsPressed( map.Device, map.Value );
						neg = Input.Manager.IsPressed( map.Device, map.Negative );
					}
					else if( map.Type == InputType.Axis )
					{
						float axis = Input.Manager.GetAxis( map.Device, map.Value );

						pos = axis >= Input.AxisPressThreshold;
						neg = axis <= -Input.AxisPressThreshold;
					}

					if( pos && !neg )
						return true;
					if( !pos && neg )
						return false;
				}

				return false;
			}
		}
		/// <summary>
		///   If any of the mapped inputs were just pressed.
		/// </summary>
		public bool JustPressed
		{
			get
			{
				foreach( InputMap map in Inputs )
				{
					if( Input.Manager.JustPressed( map.Device, map.Value ) && !Input.Manager.JustPressed( map.Device, map.Negative ) )
						return true;
				}

				return false;
			}
		}
		/// <summary>
		///   If any of the mapped inputs were just pressed.
		/// </summary>
		public bool JustReleased
		{
			get
			{
				foreach( InputMap map in Inputs )
				{
					if( Input.Manager.JustReleased( map.Device, map.Value ) && !Input.Manager.JustReleased( map.Device, map.Negative ) )
						return true;
				}

				return false;
			}
		}

		/// <summary>
		///   If the action is valid.
		/// </summary>
		public bool IsValid
		{
			get { return !string.IsNullOrWhiteSpace( Name ); }
		}

		/// <summary>
		///   Loads data from an xml element.
		/// </summary>
		/// <param name="node">
		///   The node to load data from.
		/// </param>
		/// <returns>
		///   True if loaded successfully and false otherwise.
		/// </returns>
		public bool LoadFromXml( XmlNode node )
		{
			if( node == null )
				return Logger.LogReturn( "Unable to load action from a null xml element.", false, LogType.Error );

			// Naming
			{
				string name = node.Attributes[ "name" ]?.Value?.Trim();

				if( !Naming.IsValid( name ) )
					return Logger.LogReturn( "Unable to load action; name either does not exist or is invalid.", false, LogType.Error );

				Name = Naming.IsValid( name ) ? name : string.Empty;
			}

			// Inputs
			foreach( XmlNode n in node.ChildNodes )
			{
				string loname = n.Name.ToLower();

				if( loname == "axis" || loname == "button" )
				{
					InputMap map = new InputMap();

					if( !map.LoadFromXml( n ) )
						return false;

					Inputs.Add( map );
				}
			}

			return true;
		}

		/// <summary>
		///   Returns the action as an xml string, ready to be written to file.
		/// </summary>
		/// <returns>
		///   The action as an xml string.
		/// </returns>
		public override string ToString()
		{
			return ToString( 0 );
		}
		/// <summary>
		///   Returns the action as an xml string with a given amount of indentation.
		/// </summary>
		/// <param name="tab">
		///   The amount of tabs to use for indentation.
		/// </param>
		/// <returns>
		///   The action as an xml string.
		/// </returns>
		public string ToString( uint tab )
		{
			StringBuilder sb = new StringBuilder();

			string tabs= string.Empty;

			for( uint i = 0; i < tab; i++ )
				tabs += '\t';

			sb.Append( tabs ); sb.Append( "<action name=\"" ); sb.Append( Name ); sb.Append( "\">\n" );

			foreach( InputMap p in Inputs )
			{
				if( p == null )
					continue;

				sb.Append( p.ToString( tab + 1 ) ); sb.Append( "\n" );
			}

			sb.Append( tabs ); sb.Append( "</action>" );

			return sb.ToString();
		}

		public IEnumerator<InputMap> GetEnumerator()
		{
			return ( (IEnumerable<InputMap>)Inputs ).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ( (IEnumerable<InputMap>)Inputs ).GetEnumerator();
		}

		private string m_name;
	}
}
