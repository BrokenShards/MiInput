////////////////////////////////////////////////////////////////////////////////
// Action.cs 
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

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using MiCore;

namespace MiInput
{
	/// <summary>
	///   A collection of inputs mapped to a name.
	/// </summary>
	public class Action : XmlLoadable, INamable, IEnumerable<InputMap>
	{
		/// <summary>
		///   Constructor.
		/// </summary>
		public Action()
		{
			Name     = string.Empty;
			m_inputs = new List<InputMap>();
		}
		/// <summary>
		///   Copy constructor.
		/// </summary>
		/// <param name="a">
		///   The object to copy.
		/// </param>
		public Action( Action a )
		{
			Name     = new string( a.Name.ToCharArray() );
			m_inputs = a.m_inputs.Count > 0 ? new List<InputMap>( a.m_inputs.Count ) : new List<InputMap>();

			for( int i = 0; i < a.m_inputs.Count; i++ )
				m_inputs[ i ] = new InputMap( a.m_inputs[ i ] );
		}
		/// <summary>
		///   Constructs the object with the given positive name.
		/// </summary>
		/// <param name="name">
		///   The positive name.
		/// </param>
		public Action( string name )
		{
			Name     = string.IsNullOrWhiteSpace( name ) ? string.Empty : name;
			m_inputs = new List<InputMap>();
		}
		/// <summary>
		///   Constructs the object with the given positive name and input map.
		/// </summary>
		/// <param name="name">
		///   The positive name.
		/// </param>
		/// <param name="map">
		///   The input map.
		/// </param>
		public Action( string name, params InputMap[] map )
		{
			Name = string.IsNullOrWhiteSpace( name ) ? string.Empty : name;
			m_inputs = new List<InputMap>();

			Add( map );
		}

		/// <summary>
		///   Action name.
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set { m_name = string.IsNullOrWhiteSpace( value ) ? string.Empty : Naming.AsValid( value ); }
		}

		/// <summary>
		///   If no inputs are mapped.
		/// </summary>
		public bool Empty
		{
			get { return Count is 0; }
		}
		/// <summary>
		///   The amount of mapped inputs.
		/// </summary>
		public int Count
		{
			get { return m_inputs.Count; }
		}

		/// <summary>
		///   Accesses the input map at the given index.
		/// </summary>
		/// <param name="index">
		///   The input map index.
		/// </param>
		/// <returns>
		///   The input map at the given index.
		/// </returns>
		public InputMap this[ uint index ]
		{
			get { return Get( index ); }
		}

		/// <summary>
		///   If the action already contains the input map.
		/// </summary>
		/// <param name="map">
		///   The input map.
		/// </param>
		/// <returns>
		///   True if the action already contains the input map.
		/// </returns>
		public bool Contains( InputMap map )
		{
			if( map is null || !map.IsValid )
				return false;

			foreach( InputMap m in m_inputs )
				if( map == m )
					return true;

			return false;
		}

		/// <summary>
		///   Gets the input map at the given index.
		/// </summary>
		/// <param name="index">
		///   The input map index.
		/// </param>
		/// <returns>
		///   The input map at the given index or null if the index is out of range.
		/// </returns>
		public InputMap Get( uint index )
		{
			if( index >= Count )
				return null;

			return m_inputs[ (int)index ];
		}

		/// <summary>
		///   Replaces an existing input map.
		/// </summary>
		/// <param name="index">
		///   The input map index.
		/// </param>
		/// <param name="map">
		///   The input map.
		/// </param>
		/// <returns>
		///   True if index is within range, map is valid and does not collide with any existing input maps, and the
		///   existing input map was replaces with map, otherwise false.
		/// </returns>
		public bool Set( uint index, InputMap map )
		{
			if( map is null || !map.IsValid || index >= Count )
				return false;

			for( int i = 0; i < Count; i++ )
				if( i != index && InputMap.Collides( map, m_inputs[ i ] ) )
					return false;

			m_inputs[ (int)index ] = map;
			return true;
		}
		/// <summary>
		///   Adds an input map.
		/// </summary>
		/// <param name="map">
		///   The input map.
		/// </param>
		/// <returns>
		///   True if the input map is valid, does not collide with any existing input maps, and was added successfully.
		/// </returns>
		public bool Add( InputMap map )
		{
			if( map is null || !map.IsValid )
				return false;

			for( int i = 0; i < Count; i++ )
				if( InputMap.Collides( map, m_inputs[ i ] ) )
					return false;

			m_inputs.Add( map );
			return true;
		}
		/// <summary>
		///   Adds several input maps and returns how many were successful.
		/// </summary>
		/// <param name="maps">
		///   List if input maps.
		/// </param>
		/// <returns>
		///   The amount of successfully added input maps.
		/// </returns>
		public uint Add( params InputMap[] maps )
		{
			if( maps is null )
				return 0;

			uint counter = 0;

			foreach( InputMap m in maps )
				if( Add( m ) )
					counter++;

			return counter;
		}

		/// <summary>
		///   Removes the input map at the given index.
		/// </summary>
		/// <param name="index">
		///   The input map index.
		/// </param>
		/// <returns>
		///   If the index is valid and the input map was removed successfully.
		/// </returns>
		public bool Remove( uint index )
		{
			if( index >= Count )
				return false;

			m_inputs.RemoveAt( (int)index );
			return true;
		}
		/// <summary>
		///   Removes the given input map.
		/// </summary>
		/// <param name="map">
		///   The input map.
		/// </param>
		/// <returns>
		///   If the input map existed and was removed successfully.
		/// </returns>
		public bool Remove( InputMap map )
		{
			if( map is null || !map.IsValid )
				return false;

			return m_inputs.Remove( map );
		}

		/// <summary>
		///   Removes as input maps.
		/// </summary>
		public void Clear()
		{
			m_inputs.Clear();
		}

		/// <summary>
		///   Returns the current value of the mapped input.
		/// </summary>
		public float Value
		{
			get
			{
				if( m_inputs is null || m_inputs.Count is 0 )
					return 0.0f;
								
				for( int i = 0; i < m_inputs.Count; i++ )
				{
					InputMap map = m_inputs[ i ];

					if( !map.IsValid )
						continue;

					if( map.Type is InputType.Axis )
					{
						float v = map.Device == InputDevice.Mouse ?
						          Input.Manager.Mouse.GetAxis( m_inputs[ i ].Value ) :
								  Input.Manager.Joystick.GetAxis( m_inputs[ i ].Value );

						if( v != 0.0f )
							return map.Invert ? -v : v;
					}
					else if( map.Type is InputType.Button )
					{
						bool p = map.Device is InputDevice.Keyboard ? Input.Manager.Keyboard.IsPressed( m_inputs[ i ].Value ) :
						       ( map.Device is InputDevice.Mouse    ? Input.Manager.Mouse.IsPressed( m_inputs[ i ].Value ) :
						       ( map.Device is InputDevice.Joystick && Input.Manager.Joystick.IsPressed( m_inputs[ i ].Value ) ) );
						bool n = map.Device is InputDevice.Keyboard ? Input.Manager.Keyboard.IsPressed( m_inputs[ i ].Negative ) :
							   ( map.Device is InputDevice.Mouse    ? Input.Manager.Mouse.IsPressed( m_inputs[ i ].Negative ) :
							   ( map.Device is InputDevice.Joystick && Input.Manager.Joystick.IsPressed( m_inputs[ i ].Negative ) ) );

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
			get { return Value >= Input.AxisPressThreshold; }
		}
		/// <summary>
		///   If the mapped inputs are classed as negative/pressed.
		/// </summary>
		public bool IsNegative
		{
			get { return Value <= -Input.AxisPressThreshold; }
		}

		/// <summary>
		///   If any of the mapped inputs are pressed.
		/// </summary>
		public bool IsPressed
		{
			get
			{
				foreach( InputMap map in m_inputs )
				{
					bool pos = false, 
					     neg = false;

					if( map.Type is InputType.Button )
					{
						pos = Input.Manager.IsPressed( map.Device, map.Value );
						neg = Input.Manager.IsPressed( map.Device, map.Negative );
					}
					else if( map.Type is InputType.Axis )
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
				for( int i = 0; i < m_inputs.Count; i++ )
				{
					if( Input.Manager.JustPressed( m_inputs[ i ].Device, m_inputs[ i ].Value ) &&
						!Input.Manager.JustPressed( m_inputs[ i ].Device, m_inputs[ i ].Negative ) )
						return true;
				}

				return false;
			}
		}
		/// <summary>
		///   If any of the mapped inputs were just released.
		/// </summary>
		public bool JustReleased
		{
			get
			{
				for( int i = 0; i < m_inputs.Count; i++ )
				{
					if( Input.Manager.JustReleased( m_inputs[ i ].Device, m_inputs[ i ].Value ) &&
						!Input.Manager.JustReleased( m_inputs[ i ].Device, m_inputs[ i ].Negative ) )
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
			get
			{
				if( !Naming.IsValid( Name ) || m_inputs is null )
					return false;

				for( int i = 0; i < m_inputs.Count - 1; i++ )
				{
					if( !m_inputs[ i ].IsValid )
						return false;

					for( int j = i + 1; j < m_inputs.Count; j++ )
						if( InputMap.Collides( m_inputs[ i ], m_inputs[ j ] ) )
							return false;
				}

				return true;
			}
		}

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
				return Logger.LogReturn( "Failed loading Action: Null xml element.", false, LogType.Error );

			if( !ele.HasAttribute( nameof( Name ) ) )
				return Logger.LogReturn( "Failed loading Action: No Name attribute.", false, LogType.Error );

			string name = ele.GetAttribute( nameof( Name ) );

			if( !Naming.IsValid( name ) )
				return Logger.LogReturn( "Failed loading Action: Invalid Name attribute.", false, LogType.Error );

			Name = name;
			
			foreach( XmlElement n in ele.ChildNodes )
			{
				string loname = n.Name.ToLower();

				if( loname is "axis" || loname is "button" )
				{
					InputMap map = new();

					if( !map.LoadFromXml( n ) )
						return Logger.LogReturn( "Failed loading Action: Unable to load InputMap.", false, LogType.Error );

					m_inputs.Add( map );
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
			StringBuilder sb = new();

			sb.Append( '<' ).Append( nameof( Action ) ).Append( ' ' )
				.Append( nameof( Name ) ).Append( "=\"" ).Append( Name ).AppendLine( "\">" );

			for( int i = 0; i < m_inputs.Count - 1; i++ )
			{
				if( m_inputs[ i ] is null )
					continue;

				sb.AppendLine( m_inputs[ i ].ToString( 1 ) );
			}

			sb.Append( "</" ).Append( nameof( Action ) ).Append( '>' );
			return sb.ToString();
		}

		/// <summary>
		///   Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		///   An enumerator that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<InputMap> GetEnumerator()
		{
			return ( (IEnumerable<InputMap>)m_inputs ).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ( (IEnumerable<InputMap>)m_inputs ).GetEnumerator();
		}

		private string m_name;
		private readonly List<InputMap> m_inputs;
	}
}
