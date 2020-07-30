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
using SharpLogger;
using XInputDotNetPure;

namespace SFInput
{
	/// <summary>
	///   Possible XInput device buttons.
	/// </summary>
	public enum XButtons
	{
		A,
		B,
		X,
		Y,
		Start,
		Back,
		Guide,
		LB,
		RB,
		LT,
		RT,
		LS,
		RS,
		DPadUp,
		DPadDown,
		DPadLeft,
		DPadRight,

		COUNT
	}
	/// <summary>
	///   Possible XInput device axies.
	/// </summary>
	public enum XAxis
	{
		LeftStickX,
		LeftStickY,
		RightStickX,
		RightStickY,
		LeftTrigger,
		RightTrigger,
		Triggers,

		COUNT
	}

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
			Player    = Input.Manager.Joystick.FirstConnected;
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
			set { m_player = value >= Input.MaxJoysticks ? 0 : value; }
		}

		/// <summary>
		///   Gets the state of the given axis.
		/// </summary>
		/// <param name="axis">
		///   The axis value.
		/// </param>
		/// <returns>
		///   The state of the given axis when <see cref="Update"/> was last called.
		/// </returns>
		public float GetAxis( uint axis )
		{
			if( axis < 0 || axis >= m_axies.Length )
				return 0.0f;

			return m_axies[ axis ];
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
			int a = JoystickManager.ToAxis( axis );

			if( a < 0 )
				return 0.0f;

			return GetAxis( (uint)a );
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
			int b = JoystickManager.ToButton( but );

			if( b < 0 )
				return false;

			return IsPressed( (uint)b );
		}

		/// <summary>
		///   Checks if the given axis is pressed.
		/// </summary>
		/// <param name="axis">
		///   The axis index.
		/// </param>
		/// <returns>
		///   True if <paramref name="axis"/> is within range and it is pressed, otherwise false.
		/// </returns>
		public bool IsAxisPressed( uint axis )
		{
			if( axis >= m_axies.Length )
				return false;

			return m_axies[ axis ] >= Input.AxisPressThreshold;
		}
		/// <summary>
		///   Checks if the axis represented by the given string is pressed.
		/// </summary>
		/// <param name="axis">
		///   The string to parse.
		/// </param>
		/// <returns>
		///   True if <paramref name="axis"/> represents a valid axis and is pressed, otherwise false.
		/// </returns>
		public bool IsAxisPressed( string axis )
		{
			int a = JoystickManager.ToAxis( axis );

			if( a < 0 )
				return false;

			return IsAxisPressed( (uint)a );
		}

		/// <summary>
		///   Updates the joystick state.
		/// </summary>
		public void Update()
		{
			if( Player >= Input.MaxJoysticks )
				Player = 0;

			if( !Input.Manager.Joystick.IsConnected( Player ) )
				Player = Input.Manager.Joystick.FirstConnected;

			if( Input.Manager.UseXInput )
			{
				GamePadState state = GamePad.GetState( (PlayerIndex)Player );

				if( !state.IsConnected )
				{
					for( uint i = 0; i < Input.MaxJoysticks; i++ )
					{
						GamePadState s = GamePad.GetState( (PlayerIndex)i );

						if( s.IsConnected )
						{
							Player = i;
							state  = s;
							break;
						}
						else if( i == Input.MaxJoysticks - 1 )
						{
							for( int a = 0; a < m_axies.Length; a++ )
								m_axies[ a ] = 0.0f;
							for( int b = 0; b < m_buttons.Length; b++ )
								m_buttons[ b ] = false;

							return;
						}
					}
				}

				m_axies[ (uint)XAxis.LeftStickX ]   = state.ThumbSticks.Left.X;
				m_axies[ (uint)XAxis.LeftStickY ]   = state.ThumbSticks.Left.Y;
				m_axies[ (uint)XAxis.RightStickX ]  = state.ThumbSticks.Right.X;
				m_axies[ (uint)XAxis.RightStickY ]  = state.ThumbSticks.Right.Y;
				m_axies[ (uint)XAxis.LeftTrigger ]  = state.Triggers.Left;
				m_axies[ (uint)XAxis.RightTrigger ] = state.Triggers.Right;
				m_axies[ (uint)XAxis.Triggers ]     = state.Triggers.Left - state.Triggers.Right;

				m_buttons[ (uint)XButtons.A ]     = state.Buttons.A             == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.B ]     = state.Buttons.B             == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.Y ]     = state.Buttons.X             == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.X ]     = state.Buttons.Y             == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.Start ] = state.Buttons.Start         == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.Back ]  = state.Buttons.Back          == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.Guide ] = state.Buttons.Guide         == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.LB ]    = state.Buttons.LeftShoulder  == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.RB ]    = state.Buttons.RightShoulder == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.LS ]    = state.Buttons.LeftStick     == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.RS ]    = state.Buttons.RightStick    == ButtonState.Pressed;

				m_buttons[ (uint)XButtons.LT ]    = state.Triggers.Left  >= Input.AxisPressThreshold;
				m_buttons[ (uint)XButtons.RT ]    = state.Triggers.Right >= Input.AxisPressThreshold;

				m_buttons[ (uint)XButtons.DPadUp ]    = state.DPad.Up    == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.DPadDown ]  = state.DPad.Down  == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.DPadLeft ]  = state.DPad.Left  == ButtonState.Pressed;
				m_buttons[ (uint)XButtons.DPadRight ] = state.DPad.Right == ButtonState.Pressed;
			}
			else
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
		}

		private uint    m_player;
		private float[] m_axies;
		private bool[]  m_buttons;
	}
}
