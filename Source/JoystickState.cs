////////////////////////////////////////////////////////////////////////////////
// JoystickState.cs 
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
	///   Represents the state of a single joystick at a given moment.
	/// </summary>
	public class JoystickState
	{
		/// <summary>
		///   Constructs a new state.
		/// </summary>
		public JoystickState()
		{
			Player    = 0;
			m_axies   = new float[ Joystick.AxisCount ];
			m_buttons = new bool[ Joystick.ButtonCount ];

			for( int i = 0; i < m_axies.Length; i++ )
				m_axies[ i ] = 0.0f;
			for( int i = 0; i < m_buttons.Length; i++ )
				m_buttons[ i ] = false;
		}
		/// <summary>
		///   Constructs a new state by copying from another instance.
		/// </summary>
		/// <param name="js">
		///   The state to copy from.
		/// </param>
		public JoystickState( JoystickState js )
		{
			Player    = js.Player;
			m_axies   = new float[ Joystick.AxisCount ];
			m_buttons = new bool[ Joystick.ButtonCount ];

			for( int i = 0; i < m_axies.Length; i++ )
				m_axies[ i ] = js.m_axies[ i ];
			for( int i = 0; i < m_buttons.Length; i++ )
				m_buttons[ i ] = js.m_buttons[ i ];
		}
		/// <summary>
		///   Constructs a new state with a given player number.
		/// </summary>
		/// <param name="player">
		///   The player number.
		/// </param>
		public JoystickState( uint player )
		{
			Player    = player;
			m_axies   = new float[ Joystick.AxisCount ];
			m_buttons = new bool[ Joystick.ButtonCount ];

			for( int i = 0; i < m_axies.Length; i++ )
				m_axies[ i ] = 0.0f;
			for( int i = 0; i < m_buttons.Length; i++ )
				m_buttons[ i ] = false;
		}

		/// <summary>
		///   The targeted index of the joystick to update from.
		/// </summary>
		public uint Player
		{
			get { return m_player; }
			set { m_player = value > Joystick.Count ? 0 : value; }
		}

		/// <summary>
		///   Gets the state of the given axis.
		/// </summary>
		/// <param name="axis">
		///   The axis.
		/// </param>
		/// <returns>
		///   The state of the given axis when <see cref="Update"/> was last called.
		/// </returns>
		public float GetAxis( Joystick.Axis axis )
		{
			if( axis < 0 || (int)axis >= m_axies.Length )
				return 0.0f;

			return m_axies[ (int)axis ];
		}
		/// <summary>
		///   Gets the state of the axis represented by the given string.
		/// </summary>
		/// <param name="axis">
		///   The string to parse to an axis.
		/// </param>
		/// <returns>
		///   The value of the axis on success or zero on failure.
		/// </returns>
		public float GetAxis( string axis )
		{
			if( !JoystickManager.IsAxis( axis ) )
				return 0.0f;

			return GetAxis( JoystickManager.ToAxis( axis ).Value );
		}

		/// <summary>
		///   Checks if the given button is pressed.
		/// </summary>
		/// <param name="but">
		///   The button index.
		/// </param>
		/// <returns>
		///   True if <paramref name="but"/> is within range and it is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( uint but )
		{
			if( but >= m_buttons.Length )
				return false;

			return m_buttons[ but ];
		}
		/// <summary>
		///   Checks if the button represented by the given string is pressed.
		/// </summary>
		/// <param name="but">
		///   The string to parse.
		/// </param>
		/// <returns>
		///   True if <paramref name="but"/> represents a valid button and is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( string but )
		{
			if( !JoystickManager.IsButton( but ) )
				return false;

			return IsPressed( JoystickManager.ToButton( but ).Value );
		}

		/// <summary>
		///   Updates the joystick state.
		/// </summary>
		public void Update()
		{
			if( Joystick.IsConnected( Player ) )
			{
				for( int a = 0; a < m_axies.Length; a++ )
					m_axies[ a ] = Joystick.HasAxis( Player, (Joystick.Axis)a ) ?
								   Joystick.GetAxisPosition( Player, (Joystick.Axis)a ) : 0.0f;

				for( uint b = 0; b < m_buttons.Length; b++ )
					m_buttons[ b ] = Joystick.GetButtonCount( Player ) > b && Joystick.IsButtonPressed( Player, b );
			}
			else
			{ 
				for( int i = 0; i < m_axies.Length; i++ )
					m_axies[ i ] = 0.0f;
				for( int i = 0; i < m_buttons.Length; i++ )
					m_buttons[ i ] = false;
			}
		}

		private uint    m_player;
		private float[] m_axies;
		private bool[]  m_buttons;
	}
}
