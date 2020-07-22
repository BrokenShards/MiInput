////////////////////////////////////////////////////////////////////////////////
// JoystickManager.cs 
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
using SFML.Window;

namespace SFInput
{
	/// <summary>
	///   Manages the state of the Joysticks.
	/// </summary>
	public class JoystickManager
	{
		/// <summary>
		///   Checks if the given string represents a valid button.
		/// </summary>
		/// <remarks>
		///   A valid button string is a number between zero and <see cref="Joystick.ButtonCount"/>.
		/// </remarks>
		/// <param name="val">
		///   The string to check.
		/// </param>
		/// <returns>
		///   True if the given string represents a valid joystick button and false otherwise.
		/// </returns>
		public static bool IsButton( string val )
		{
			if( string.IsNullOrEmpty( val ) )
				return false;

			if( !uint.TryParse( val, out uint b ) || b >= Joystick.ButtonCount )
				return false;

			return true;
		}
		/// <summary>
		///   Parses the given string to its joystick button representation.
		/// </summary>
		/// <param name="val">
		///   The string to parse.
		/// </param>
		/// <returns>
		///   The joystick button parsed from the string on success or null on failure.
		/// </returns>
		public static uint? ToButton( string val )
		{
			if( !IsButton( val ) )
				return null;

			return uint.Parse( val );
		}

		/// <summary>
		///   Checks if the given string represents a valid axis.
		/// </summary>
		/// <remarks>
		///   A valid axis string is either a case-insensitive <see cref="Joystick.Axis"/> name or value.
		/// </remarks>
		/// <param name="val">
		///   The string to check.
		/// </param>
		/// <returns>
		///   True if the given string represents a valid joystick axis and false otherwise.
		/// </returns>
		public static bool IsAxis( string val )
		{
			if( string.IsNullOrEmpty( val ) )
				return false;

			if( !Enum.TryParse( val, true, out Joystick.Axis ax ) )
			{
				if( uint.TryParse( val, out uint b ) )
				{
					if( b >= Enum.GetNames( typeof( Joystick.Axis ) ).Length )
						return false;
				}
				else
					return false;
			}

			return true;
		}
		/// <summary>
		///   Parses the given string to its joystick axis representation.
		/// </summary>
		/// <param name="val">
		///   The string to parse.
		/// </param>
		/// <returns>
		///   The joystick axis parsed from the string on success or null on failure.
		/// </returns>
		public static Joystick.Axis? ToAxis( string val )
		{
			if( !IsAxis( val ) )
				return null;

			if( !Enum.TryParse( val, true, out Joystick.Axis ax ) )
				return (Joystick.Axis)uint.Parse( val );

			return ax;
		}


		/// <summary>
		///   Constructs the instance.
		/// </summary>
		public JoystickManager()
		{
			m_current = new JoystickState[ Joystick.Count ];
			m_last    = new JoystickState[ Joystick.Count ];

			for( uint i = 0; i < Joystick.Count; i++ )
			{
				m_current[ i ] = new JoystickState( i );
				m_last[ i ]    = new JoystickState( i );
			}
		}

		/// <summary>
		///   Checks if there is a joystick connected at the given index.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <returns>
		///   True if a joystick is connected at the given index and false otherwise.
		/// </returns>
		public bool IsConnected( uint joystick )
		{
			if( joystick >= Joystick.Count )
				return false;

			return Joystick.IsConnected( joystick );
		}

		/// <summary>
		///   Updates the joystick states.
		/// </summary>
		public void Update()
		{
			for( uint i = 0; i < Joystick.Count; i++ )
			{
				m_last[ i ] = new JoystickState( m_current[ i ] );
				m_current[ i ].Update();
			}
		}

		/// <summary>
		///   Check if the given button is pressed.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( uint joystick, uint but )
		{
			if( joystick >= Joystick.Count )
				return false;

			return m_current[ joystick ].IsPressed( but );
		}
		/// <summary>
		///   Check if the given button is pressed.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( uint joystick, string but )
		{
			if( joystick >= Joystick.Count )
				return false;

			return m_current[ joystick ].IsPressed( but );
		}

		/// <summary>
		///   Check if the given button was just pressed.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button was just pressed and false otherwise.
		/// </returns>
		public bool JustPressed( uint joystick, uint but )
		{
			if( joystick >= Joystick.Count )
				return false;

			return m_current[ joystick ].IsPressed( but ) && !m_last[ joystick ].IsPressed( but );
		}
		/// <summary>
		///   Check if the given button was just pressed.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button was just pressed and false otherwise.
		/// </returns>
		public bool JustPressed( uint joystick, string but )
		{
			if( joystick >= Joystick.Count )
				return false;

			return m_current[ joystick ].IsPressed( but ) && !m_last[ joystick ].IsPressed( but );
		}

		/// <summary>
		///   Check if the given button was just released.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button was just released and false otherwise.
		/// </returns>
		public bool JustReleased( uint joystick, uint but )
		{
			if( joystick >= Joystick.Count )
				return false;

			return ( !m_current[ joystick ].IsPressed( but ) && m_last[ joystick ].IsPressed( but ) );
		}
		/// <summary>
		///   Check if the given button was just released.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button was just released and false otherwise.
		/// </returns>
		public bool JustReleased( uint joystick, string but )
		{
			if( joystick >= Joystick.Count )
				return false;

			return ( !m_current[ joystick ].IsPressed( but ) && m_last[ joystick ].IsPressed( but ) );
		}

		/// <summary>
		///   Gets the value of the given axis from the given joystick player index.
		/// </summary>
		/// <param name="joystick">
		///   The player index of the joystick.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The value of the given axis from the given joystick player index. Will also return zero if 
		///   <paramref name="axis"/> or <paramref name="joystick"/> are out of range or not connected.
		/// </returns>
		public float GetAxis( uint joystick, Joystick.Axis axis )
		{
			if( axis < 0 || (uint)axis >= Joystick.AxisCount || joystick >= Joystick.Count || !IsConnected( joystick ) )
				return 0.0f;

			return m_current[ joystick ].GetAxis( axis );
		}
		/// <summary>
		///   Gets the value of the given axis from the given joystick player index.
		/// </summary>
		/// <param name="joystick">
		///   The player index of the joystick.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The value of the given axis from the given joystick player index. Will also return zero if 
		///   <paramref name="axis"/> or <paramref name="joystick"/> are out of range or not connected.
		/// </returns>
		public float GetAxis( uint joystick, string axis )
		{
			if( joystick >= Joystick.Count || !IsConnected( joystick ) )
				return 0.0f;

			return m_current[ joystick ].GetAxis( axis );
		}

		/// <summary>
		///   Gets the difference in value between the last two update calls of the given axis from the given joystick
		///   player index.
		/// </summary>
		/// <param name="joystick">
		///   The player index of the joystick.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The difference in value between the last to update calls of the given axis from the given joystick player
		///   index. Will also return zero if <paramref name="axis"/> or <paramref name="joystick"/> are out of range
		///   or not connected.
		/// </returns>
		public float GetAxisDelta( uint joystick, Joystick.Axis axis )
		{
			if( axis < 0 || (uint)axis >= Joystick.AxisCount || joystick >= Joystick.Count || !IsConnected( joystick ) )
				return 0.0f;

			return m_current[ joystick ].GetAxis( axis ) - m_last[ joystick ].GetAxis( axis );
		}
		/// <summary>
		///   Gets the difference in value between the last two update calls of the given axis from the given joystick
		///   player index.
		/// </summary>
		/// <param name="joystick">
		///   The player index of the joystick.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The difference in value between the last to update calls of the given axis from the given joystick player
		///   index. Will also return zero if <paramref name="axis"/> or <paramref name="joystick"/> are out of range
		///   or not connected.
		/// </returns>
		public float GetAxisDelta( uint joystick, string axis )
		{
			if( joystick >= Joystick.Count || !IsConnected( joystick ) )
				return 0.0f;

			return m_current[ joystick ].GetAxis( axis ) - m_last[ joystick ].GetAxis( axis );
		}

		private JoystickState[] m_current,
							    m_last;
	}
}
