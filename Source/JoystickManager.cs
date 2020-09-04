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
using XInputDotNetPure;

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
		///   A valid button string is a number between zero and the button count.
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

			if( !Enum.TryParse( val, true, out XButton xb ) && ( !uint.TryParse( val, out uint b ) || b >= (uint)XButton.COUNT ) )
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
		///   The joystick button parsed from the string on success or -1 on failure.
		/// </returns>
		public static int ToButton( string val )
		{
			if( !IsButton( val ) )
				return -1;

			if( Enum.TryParse( val, true, out XButton xb ) )
				return (int)xb;
			if( uint.TryParse( val, out uint b ) && b < (uint)XButton.COUNT )
				return (int)b;

			return -1;
		}

		/// <summary>
		///   Checks if the given string represents a valid axis.
		/// </summary>
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

			if( !Enum.TryParse( val, true, out XAxis ax ) )
			{
				if( uint.TryParse( val, out uint b ) )
				{
					if( b >= (uint)XAxis.COUNT )
						return false;
				}
				else
					return false;
			}

			return true;
		}
		/// <summary>
		///   Parses the given string to its joystick axis value.
		/// </summary>
		/// <param name="val">
		///   The string to parse.
		/// </param>
		/// <returns>
		///   The joystick axis value parsed from the string on success or -1 on failure.
		/// </returns>
		public static int ToAxis( string val )
		{
			if( val != null )
			{
				if( Enum.TryParse( val, true, out XAxis ax ) )
					return (int)ax;
				if( uint.TryParse( val, out uint a ) && a < (uint)XAxis.COUNT )
					return (int)a;
			}

			return -1;
		}
		
		/// <summary>
		///   Constructor.
		/// </summary>
		public JoystickManager()
		{
			m_last    = new JoystickState( 0 );
			m_current = new JoystickState( 0 );
		}
		/// <summary>
		///   Copy constructor.
		/// </summary>
		/// <param name="man">
		///   The manager to copy.
		/// </param>
		public JoystickManager( JoystickManager man )
		{
			m_last    = new JoystickState( man.m_last );
			m_current = new JoystickState( man.m_current );
		}
		/// <summary>
		///   Constructor that sets player index.
		/// </summary>
		/// <param name="player">
		///   The player index.
		/// </param>
		public JoystickManager( uint player )
		{
			m_last    = new JoystickState( player );
			m_current = new JoystickState( player );
		}

		/// <summary>
		///   The targeted index of the joystick to update from.
		/// </summary>
		public uint Player
		{
			get { return m_current.Player; }
			set { m_current.Player = value; }
		}

		/// <summary>
		///   If the joystick is connected.
		/// </summary>
		public bool IsConnected
		{
			get { return GamePad.GetState( (PlayerIndex)Player ).IsConnected; }
		}

		/// <summary>
		///   The first connected joystick.
		/// </summary>
		public uint FirstConnected
		{
			get
			{
				for( uint i = 0; i < Input.MaxJoysticks; i++ )
				{
					GamePadState state = GamePad.GetState( (PlayerIndex)i );

					if( state.IsConnected )
						return i;
				}

				return Input.MaxJoysticks;
			}
		}

		/// <summary>
		///   Updates the managed device states.
		/// </summary>
		public void Update()
		{
			m_last = new JoystickState( m_current );
			m_current.Update();
		}
		/// <summary>
		///   Reset the device state.
		/// </summary>
		public void Reset()
		{
			m_last.Reset();
			m_current.Reset();
		}

		/// <summary>
		///   If the button is pressed.
		/// </summary>
		/// <param name="but">
		///   The index of the button.
		/// </param>
		/// <returns>
		///   True if the button index is valid and the button is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( uint but )
		{
			return m_current.IsPressed( but );
		}
		/// <summary>
		///   If the button is pressed.
		/// </summary>
		/// <param name="but">
		///   The name of the button.
		/// </param>
		/// <returns>
		///   True if the button name is valid and the button is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( string but )
		{
			return m_current.IsPressed( but );
		}
		/// <summary>
		///   If the button is pressed.
		/// </summary>
		/// <param name="but">
		///   The index of the button.
		/// </param>
		/// <returns>
		///   True if the button index is valid and the button is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( XButton but )
		{
			return m_current.IsPressed( but );
		}

		/// <summary>
		///   If the button has just been pressed.
		/// </summary>
		/// <param name="but">
		///   The index of the button.
		/// </param>
		/// <returns>
		///   True if the button index is valid and the button has just been pressed, otherwise false.
		/// </returns>
		public bool JustPressed( uint but )
		{
			return m_current.IsPressed( but ) && !m_last.IsPressed( but );
		}
		/// <summary>
		///   If the button has just been pressed.
		/// </summary>
		/// <param name="but">
		///   The name of the button.
		/// </param>
		/// <returns>
		///   True if the button name is valid and the button has just been pressed, otherwise false.
		/// </returns>
		public bool JustPressed( string but )
		{
			return m_current.IsPressed( but ) && !m_last.IsPressed( but );
		}
		/// <summary>
		///   If the button has just been pressed.
		/// </summary>
		/// <param name="but">
		///   The index of the button.
		/// </param>
		/// <returns>
		///   True if the button index is valid and the button has just been pressed, otherwise false.
		/// </returns>
		public bool JustPressed( XButton but )
		{
			return m_current.IsPressed( but ) && !m_last.IsPressed( but );
		}

		/// <summary>
		///   If the button has just been released.
		/// </summary>
		/// <param name="but">
		///   The index of the button.
		/// </param>
		/// <returns>
		///   True if the button index is valid and the button has just been released, otherwise false.
		/// </returns>
		public bool JustReleased( uint but )
		{
			return !m_current.IsPressed( but ) && m_last.IsPressed( but );
		}
		/// <summary>
		///   If the button has just been released.
		/// </summary>
		/// <param name="but">
		///   The name of the button.
		/// </param>
		/// <returns>
		///   True if the button name is valid and the button has just been released, otherwise false.
		/// </returns>
		public bool JustReleased( string but )
		{
			return !m_current.IsPressed( but ) && m_last.IsPressed( but );
		}
		/// <summary>
		///   If the button has just been released.
		/// </summary>
		/// <param name="but">
		///   The index of the button.
		/// </param>
		/// <returns>
		///   True if the button index is valid and the button has just been released, otherwise false.
		/// </returns>
		public bool JustReleased( XButton but )
		{
			return !m_current.IsPressed( but ) && m_last.IsPressed( but );
		}

		/// <summary>
		///   Gets the current state of the axis.
		/// </summary>
		/// <param name="ax">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   The current state of the axis if the index is valid, otherwise 0.0.
		/// </returns>
		public float GetAxis( uint ax )
		{
			return m_current.GetAxis( ax );
		}
		/// <summary>
		///   Gets the current state of the axis.
		/// </summary>
		/// <param name="ax">
		///   The name of the axis.
		/// </param>
		/// <returns>
		///   The current state of the axis if the name is valid, otherwise 0.0.
		/// </returns>
		public float GetAxis( string ax )
		{
			return m_current.GetAxis( ax );
		}
		/// <summary>
		///   Gets the current state of the axis.
		/// </summary>
		/// <param name="ax">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   The current state of the axis if the index is valid, otherwise 0.0.
		/// </returns>
		public float GetAxis( XAxis ax )
		{
			return m_current.GetAxis( ax );
		}

		/// <summary>
		///   Gets the previous state of the axis.
		/// </summary>
		/// <param name="ax">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   The previous state of the axis if the index is valid, otherwise 0.0.
		/// </returns>
		public float GetLastAxis( uint ax )
		{
			return m_last.GetAxis( ax );
		}
		/// <summary>
		///   Gets the previous state of the axis.
		/// </summary>
		/// <param name="ax">
		///   The name of the axis.
		/// </param>
		/// <returns>
		///   The previous state of the axis if the name is valid, otherwise 0.0.
		/// </returns>
		public float GetLastAxis( string ax )
		{
			return m_last.GetAxis( ax );
		}
		/// <summary>
		///   Gets the previous state of the axis.
		/// </summary>
		/// <param name="ax">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   The previous state of the axis if the index is valid, otherwise 0.0.
		/// </returns>
		public float GetLastAxis( XAxis ax )
		{
			return m_last.GetAxis( ax );
		}

		/// <summary>
		///   Gets the difference in value between the last two update calls of the given axis from the given joystick
		///   player index.
		/// </summary>
		/// <param name="ax">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The difference in value between the last to update calls of the given axis from the given joystick player
		///   index. Will also return zero if <paramref name="axis"/> is out of range.
		/// </returns>
		public float AxisDelta( uint ax )
		{
			return m_current.GetAxis( ax ) - m_last.GetAxis( ax );
		}
		/// <summary>
		///   Gets the difference in value between the last two update calls of the given axis from the given joystick
		///   player index.
		/// </summary>
		/// <param name="ax">
		///   The axis to check.
		/// </param>
		///   The difference in value between the last to update calls of the given axis from the given joystick player
		///   index. Will also return zero if <paramref name="axis"/> is out of range.
		/// </returns>
		public float AxisDelta( string ax )
		{
			return m_current.GetAxis( ax ) - m_last.GetAxis( ax );
		}
		/// <summary>
		///   Gets the difference in value between the last two update calls of the given axis from the given joystick
		///   player index.
		/// </summary>
		/// <param name="ax">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The difference in value between the last to update calls of the given axis from the given joystick player
		///   index. Will also return zero if <paramref name="ax"/> is out of range.
		/// </returns>
		public float AxisDelta( XAxis ax )
		{
			return m_current.GetAxis( ax ) - m_last.GetAxis( ax );
		}

		/// <summary>
		///   Check if the given axis is pressed.
		/// </summary>
		/// <param name="ax">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   True if the given axis is pressed and false otherwise.
		/// </returns>
		public bool AxisIsPressed( uint ax )
		{
			return m_current.AxisIsPressed( ax );
		}
		/// <summary>
		///   Check if the given axis is pressed.
		/// </summary>
		/// <param name="ax">
		///   The name of the axis.
		/// </param>
		/// <returns>
		///   True if the given axis is pressed and false otherwise.
		/// </returns>
		public bool AxisIsPressed( string ax )
		{
			return m_current.AxisIsPressed( ax );
		}
		/// <summary>
		///   Check if the given axis is pressed.
		/// </summary>
		/// <param name="ax">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   True if the given axis is pressed and false otherwise.
		/// </returns>
		public bool AxisIsPressed( XAxis ax )
		{
			return m_current.AxisIsPressed( ax );
		}

		/// <summary>
		///   Check if the given axis was just pressed.
		/// </summary>
		/// <param name="ax">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   True if the given axis was just pressed and false otherwise.
		/// </returns>
		public bool AxisJustPressed( uint ax )
		{
			return m_current.AxisIsPressed( ax ) && !m_last.AxisIsPressed( ax );
		}
		/// <summary>
		///   Check if the given axis was just pressed.
		/// </summary>
		/// <param name="ax">
		///   The name of the axis.
		/// </param>
		/// <returns>
		///   True if the given axis was just pressed and false otherwise.
		/// </returns>
		public bool AxisJustPressed( string ax )
		{
			return m_current.AxisIsPressed( ax ) && !m_last.AxisIsPressed( ax );
		}
		/// <summary>
		///   Check if the given axis was just pressed.
		/// </summary>
		/// <param name="ax">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   True if the given axis was just pressed and false otherwise.
		/// </returns>
		public bool AxisJustPressed( XAxis ax )
		{
			return m_current.AxisIsPressed( ax ) && !m_last.AxisIsPressed( ax );
		}

		/// <summary>
		///   Check if the given axis was just released.
		/// </summary>
		/// <param name="ax">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis was just released and false otherwise.
		/// </returns>
		public bool AxisJustReleased( uint ax )
		{
			return !m_current.AxisIsPressed( ax ) && m_last.AxisIsPressed( ax );
		}
		/// <summary>
		///   Check if the given axis was just released.
		/// </summary>
		/// <param name="ax">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis was just released and false otherwise.
		/// </returns>
		public bool AxisJustReleased( string ax )
		{
			return !m_current.AxisIsPressed( ax ) && m_last.AxisIsPressed( ax );
		}
		/// <summary>
		///   Check if the given axis was just released.
		/// </summary>
		/// <param name="ax">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis was just released and false otherwise.
		/// </returns>
		public bool AxisJustReleased( XAxis ax )
		{
			return !m_current.AxisIsPressed( ax ) && m_last.AxisIsPressed( ax );
		}

		private JoystickState m_current,
						      m_last;
	}
}
