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

			if( Input.Manager.UseXInput )
			{
				if( !Enum.TryParse( val, true, out XButtons xb ) && ( !uint.TryParse( val, out uint b ) || b >= (uint)XButtons.COUNT ) )
					return false;
			}
			else
			{
				if( !uint.TryParse( val, out uint b ) || b >= Joystick.ButtonCount )
					return false;
			}

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

			if( Input.Manager.UseXInput && Enum.TryParse( val, true, out XButtons xb ) )
				return (int)xb;

			uint count = Input.Manager.UseXInput ? (uint)XButtons.COUNT : Joystick.ButtonCount;

			if( uint.TryParse( val, out uint b ) && b < count )
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
			return Input.Manager.UseXInput ? IsXAxis( val ) : IsSAxis( val );
		}
		/// <summary>
		///   Checks if the given string represents a valid SFML axis.
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
		private static bool IsSAxis( string val )
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
		///   Checks if the given string represents a valid XInput axis.
		/// </summary>
		/// <remarks>
		///   A valid axis string is either a case-insensitive <see cref="XAxis"/> name or value.
		/// </remarks>
		/// <param name="val">
		///   The string to check.
		/// </param>
		/// <returns>
		///   True if the given string represents a valid joystick axis and false otherwise.
		/// </returns>
		private static bool IsXAxis( string val )
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
			if( Input.Manager.UseXInput )
			{
				if( Enum.TryParse( val, true, out XAxis ax ) )
					return (int)ax;
				else if( uint.TryParse( val, out uint a ) && a < (uint)XAxis.COUNT )
					return (int)a;
			}
			else
			{
				if( Enum.TryParse( val, true, out Joystick.Axis ax ) )
					return (int)ax;
				else if( uint.TryParse( val, out uint a ) && a < Joystick.AxisCount )
					return (int)a;
			}

			return -1;
		}

		/// <summary>
		///   Constructs the instance.
		/// </summary>
		public JoystickManager()
		{
			m_current = new JoystickState[ Input.MaxJoysticks ];
			m_last = new JoystickState[ Input.MaxJoysticks ];

			for( uint i = 0; i < Input.MaxJoysticks; i++ )
			{
				m_current[ i ] = new JoystickState( i );
				m_last[ i ] = new JoystickState( i );
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
			if( joystick >= Input.MaxJoysticks )
				return false;

			if( Input.Manager.UseXInput )
				return GamePad.GetState( (PlayerIndex)joystick ).IsConnected;

			return Joystick.IsConnected( joystick );
		}

		/// <summary>
		///   The first connected joystick.
		/// </summary>
		public uint FirstConnected
		{
			get
			{
				if( Input.Manager.UseXInput )
				{
					for( uint i = 0; i < Input.MaxJoysticks; i++ )
					{
						GamePadState state = GamePad.GetState( (PlayerIndex)i );

						if( state.IsConnected )
							return i;
					}
				}
				else
				{
					for( uint i = 0; i < Input.MaxJoysticks; i++ )
						if( Joystick.IsConnected( i ) )
							return i;
				}

				return Input.MaxJoysticks;
			}
		}

		/// <summary>
		///   Updates the joystick states.
		/// </summary>
		public void Update()
		{
			for( uint i = 0; i < Input.MaxJoysticks; i++ )
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
			if( joystick >= Input.MaxJoysticks )
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
			if( joystick >= Input.MaxJoysticks )
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
			if( joystick >= Input.MaxJoysticks )
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
			if( joystick >= Input.MaxJoysticks )
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
			if( joystick >= Input.MaxJoysticks )
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
			if( joystick >= Input.MaxJoysticks )
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
		public float GetAxis( uint joystick, uint axis )
		{
			uint count = Input.Manager.UseXInput ? (uint)XAxis.COUNT : Joystick.AxisCount;

			if( axis >= count || joystick >= Input.MaxJoysticks )
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
			if( joystick >= Input.MaxJoysticks || !IsConnected( joystick ) )
				return 0.0f;

			return m_current[ joystick ].GetAxis( axis );
		}

		/// <summary>
		///   Gets the previous value of the given axis from the given joystick player index.
		/// </summary>
		/// <param name="joystick">
		///   The player index of the joystick.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The previous value of the given axis from the given joystick player index. Will also return zero if 
		///   <paramref name="axis"/> or <paramref name="joystick"/> are out of range or not connected.
		/// </returns>
		public float GetLastAxis( uint joystick, uint axis )
		{
			uint count = Input.Manager.UseXInput ? (uint)XAxis.COUNT : Joystick.AxisCount;

			if( axis >= count || joystick >= Input.MaxJoysticks )
				return 0.0f;

			return m_last?[ joystick ]?.GetAxis( axis ) ?? 0.0f;
		}
		/// <summary>
		///   Gets the previous value of the given axis from the given joystick player index.
		/// </summary>
		/// <param name="joystick">
		///   The player index of the joystick.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The previous value of the given axis from the given joystick player index. Will also return zero if 
		///   <paramref name="axis"/> or <paramref name="joystick"/> are out of range or not connected.
		/// </returns>
		public float GetLastAxis( uint joystick, string axis )
		{
			if( joystick >= Input.MaxJoysticks || !IsConnected( joystick ) )
				return 0.0f;

			return m_last?[ joystick ]?.GetAxis( axis ) ?? 0.0f;
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
		public float GetAxisDelta( uint joystick, uint axis )
		{
			return GetAxis( joystick, axis ) - GetLastAxis( joystick, axis );
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
			return GetAxis( joystick, axis ) - GetLastAxis( joystick, axis );
		}

		/// <summary>
		///   Check if the given axis is pressed.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis is pressed and false otherwise.
		/// </returns>
		public bool IsAxisPressed( uint joystick, uint axis )
		{
			if( joystick >= Input.MaxJoysticks )
				return false;

			return m_current[ joystick ].IsAxisPressed( axis );
		}
		/// <summary>
		///   Check if the given axis is pressed.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis is pressed and false otherwise.
		/// </returns>
		public bool IsAxisPressed( uint joystick, string axis )
		{
			if( joystick >= Input.MaxJoysticks )
				return false;

			return m_current[ joystick ].IsAxisPressed( axis );
		}
		/// <summary>
		///   Check if the given axis was just pressed.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis was just pressed and false otherwise.
		/// </returns>
		public bool AxisJustPressed( uint joystick, uint axis )
		{
			if( joystick >= Input.MaxJoysticks )
				return false;

			return m_current[ joystick ].IsAxisPressed( axis ) && !( m_last?[ joystick ]?.IsAxisPressed( axis ) ?? false );
		}
		/// <summary>
		///   Check if the given axis was just pressed.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis was just pressed and false otherwise.
		/// </returns>
		public bool AxisJustPressed( uint joystick, string axis )
		{
			if( joystick >= Input.MaxJoysticks )
				return false;

			return m_current[ joystick ].IsAxisPressed( axis ) && !( m_last?[ joystick ]?.IsAxisPressed( axis ) ?? false );
		}
		/// <summary>
		///   Check if the given axis was just released.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis was just released and false otherwise.
		/// </returns>
		public bool AxisJustReleased( uint joystick, uint axis )
		{
			if( joystick >= Input.MaxJoysticks )
				return false;

			return !m_current[ joystick ].IsAxisPressed( axis ) && ( m_last?[ joystick ]?.IsAxisPressed( axis ) ?? false );
		}
		/// <summary>
		///   Check if the given axis was just released.
		/// </summary>
		/// <param name="joystick">
		///   The joystick index.
		/// </param>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis was just released and false otherwise.
		/// </returns>
		public bool AxisJustReleased( uint joystick, string axis )
		{
			if( joystick >= Input.MaxJoysticks )
				return false;

			return !m_current[ joystick ].IsAxisPressed( axis ) && ( m_last?[ joystick ]?.IsAxisPressed( axis ) ?? false );
		}

		private JoystickState[] m_current,
							    m_last;
	}
}
