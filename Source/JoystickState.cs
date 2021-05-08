////////////////////////////////////////////////////////////////////////////////
// JoystickState.cs 
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
using XInputDotNetPure;

namespace MiInput
{
	/// <summary>
	///   Possible XInput device buttons.
	/// </summary>
	public enum XButton
	{
		/// <summary>
		///   A button.
		/// </summary>
		A,
		/// <summary>
		///   B button.
		/// </summary>
		B,
		/// <summary>
		///   X button.
		/// </summary>
		X,
		/// <summary>
		///   Y button.
		/// </summary>
		Y,
		/// <summary>
		///   Start button.
		/// </summary>
		Start,
		/// <summary>
		///   Back button.
		/// </summary>
		Back,
		/// <summary>
		///   Guide button.
		/// </summary>
		Guide,
		/// <summary>
		///   Left bumper.
		/// </summary>
		LB,
		/// <summary>
		///   Right bumper.
		/// </summary>
		RB,
		/// <summary>
		///   Left trigger.
		/// </summary>
		LT,
		/// <summary>
		///   Right trigger.
		/// </summary>
		RT,
		/// <summary>
		///   Left stick click.
		/// </summary>
		LS,
		/// <summary>
		///   Right stick click.
		/// </summary>
		RS,
		/// <summary>
		///   DPad up.
		/// </summary>
		DPadUp,
		/// <summary>
		///   DPad down.
		/// </summary>
		DPadDown,
		/// <summary>
		///   DPad left.
		/// </summary>
		DPadLeft,
		/// <summary>
		///   DPad right.
		/// </summary>
		DPadRight,

		/// <summary>
		///   Button count.
		/// </summary>
		COUNT
	}
	/// <summary>
	///   Possible XInput device axies.
	/// </summary>
	public enum XAxis
	{
		/// <summary>
		///   Left stick horizontal axis.
		/// </summary>
		LeftStickX,
		/// <summary>
		///   Left stick vertical axis.
		/// </summary>
		LeftStickY,
		/// <summary>
		///   Right stick horizontal axis.
		/// </summary>
		RightStickX,
		/// <summary>
		///   Right stick vertical axis.
		/// </summary>
		RightStickY,
		/// <summary>
		///   Left trigger.
		/// </summary>
		LeftTrigger,
		/// <summary>
		///   Right trigger.
		/// </summary>
		RightTrigger,
		/// <summary>
		///   Both triggers.
		/// </summary>
		Triggers,

		/// <summary>
		///   Axis count.
		/// </summary>
		COUNT
	}

	/// <summary>
	///   Represents the state of a single joystick at a given moment.
	/// </summary>
	public class JoystickState : ICloneable, IEquatable<JoystickState>
	{
		/// <summary>
		///   Button count.
		/// </summary>
		public const uint ButtonCount = (uint)XButton.COUNT;
		/// <summary>
		///   Axis count.
		/// </summary>
		public const uint AxisCount = (uint)XAxis.COUNT;

		/// <summary>
		///   Constructs a new state.
		/// </summary>
		public JoystickState()
		{
			m_button = new bool[ ButtonCount ];
			m_axis   = new float[ AxisCount ];
			Reset();
		}
		/// <summary>
		///   Constructs a new state by copying from another instance.
		/// </summary>
		/// <param name="js">
		///   The state to copy from.
		/// </param>
		public JoystickState( JoystickState js )
		{
			if( js == null )
				throw new ArgumentNullException( nameof( js ) );

			m_button = ButtonCount > 0 ? new bool[ ButtonCount ] : null;
			m_axis = AxisCount > 0 ? new float[ AxisCount ] : null;

			if( m_button != null )
				for( uint i = 0; i < ButtonCount; i++ )
					m_button[ i ] = js.m_button[ i ];

			if( m_axis != null )
				for( uint i = 0; i < AxisCount; i++ )
					m_axis[ i ] = js.m_axis[ i ];
		}

		/// <summary>
		///   Updates the object to the current state of the device.
		/// </summary>
		public void Update()
		{
			GamePadState state = GamePad.GetState( (PlayerIndex)Input.FirstJoystick );

			if( !state.IsConnected )
			{
				Reset();
				return;
			}

			m_axis[ (uint)XAxis.LeftStickX ]   = state.ThumbSticks.Left.X;
			m_axis[ (uint)XAxis.LeftStickY ]   = state.ThumbSticks.Left.Y;
			m_axis[ (uint)XAxis.RightStickX ]  = state.ThumbSticks.Right.X;
			m_axis[ (uint)XAxis.RightStickY ]  = state.ThumbSticks.Right.Y;
			m_axis[ (uint)XAxis.LeftTrigger ]  = state.Triggers.Left;
			m_axis[ (uint)XAxis.RightTrigger ] = state.Triggers.Right;
			m_axis[ (uint)XAxis.Triggers ]     = -state.Triggers.Left + state.Triggers.Right;

			m_button[ (uint)XButton.A ]     = state.Buttons.A == ButtonState.Pressed;
			m_button[ (uint)XButton.B ]     = state.Buttons.B == ButtonState.Pressed;
			m_button[ (uint)XButton.Y ]     = state.Buttons.X == ButtonState.Pressed;
			m_button[ (uint)XButton.X ]     = state.Buttons.Y == ButtonState.Pressed;
			m_button[ (uint)XButton.Start ] = state.Buttons.Start == ButtonState.Pressed;
			m_button[ (uint)XButton.Back ]  = state.Buttons.Back  == ButtonState.Pressed;
			m_button[ (uint)XButton.Guide ] = state.Buttons.Guide == ButtonState.Pressed;
			m_button[ (uint)XButton.LB ]    = state.Buttons.LeftShoulder  == ButtonState.Pressed;
			m_button[ (uint)XButton.RB ]    = state.Buttons.RightShoulder == ButtonState.Pressed;
			m_button[ (uint)XButton.LT ]    = state.Triggers.Left  >= Input.AxisPressThreshold;
			m_button[ (uint)XButton.RT ]    = state.Triggers.Right >= Input.AxisPressThreshold;
			m_button[ (uint)XButton.LS ]    = state.Buttons.LeftStick  == ButtonState.Pressed;
			m_button[ (uint)XButton.RS ]    = state.Buttons.RightStick == ButtonState.Pressed;

			m_button[ (uint)XButton.DPadUp ]    = state.DPad.Up    == ButtonState.Pressed;
			m_button[ (uint)XButton.DPadDown ]  = state.DPad.Down  == ButtonState.Pressed;
			m_button[ (uint)XButton.DPadLeft ]  = state.DPad.Left  == ButtonState.Pressed;
			m_button[ (uint)XButton.DPadRight ] = state.DPad.Right == ButtonState.Pressed;
		}

		/// <summary>
		///   Reset state values.
		/// </summary>
		public virtual void Reset()
		{
			if( m_button != null )
				for( uint i = 0; i < ButtonCount; i++ )
					m_button[ i ] = false;
			if( m_axis != null )
				for( uint i = 0; i < AxisCount; i++ )
					m_axis[ i ] = 0.0f;
		}

		/// <summary>
		///   If the button is pressed.
		/// </summary>
		/// <param name="button">
		///   The index of the button.
		/// </param>
		/// <returns>
		///   True if the button index is within range and the button is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( uint button )
		{
			if( button >= ButtonCount )
				return false;

			return m_button[ button ];
		}
		/// <summary>
		///   If the button is pressed.
		/// </summary>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the button is within range and is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( XButton but )
		{
			if( but < 0 || but >= XButton.COUNT )
				return false;

			return IsPressed( (uint)but );
		}
		/// <summary>
		///   If the button is pressed.
		/// </summary>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the button is valid and is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( string but )
		{
			if( !JoystickManager.IsButton( but ) )
				return false;

			return IsPressed( (uint)JoystickManager.ToButton( but ) );
		}

		/// <summary>
		///   The current axis value.
		/// </summary>
		/// <param name="axis">
		///   The index of the axis.
		/// </param>
		/// <returns>
		///   The value of the axis if the index is within range, otherwise 0.
		/// </returns>
		public float GetAxis( uint axis )
		{
			if( axis >= AxisCount )
				return 0.0f;

			return m_axis[ axis ];
		}
		/// <summary>
		///   The value of the axis.
		/// </summary>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The value of the axis if it is within range, otherwise 0.0.
		/// </returns>
		public float GetAxis( XAxis axis )
		{
			if( axis < 0 || axis >= XAxis.COUNT )
				return 0.0f;

			return GetAxis( (uint)axis );
		}
		/// <summary>
		///   The value of the axis.
		/// </summary>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   The value of the axis if it is valid, otherwise 0.0.
		/// </returns>
		public float GetAxis( string axis )
		{
			if( !JoystickManager.IsAxis( axis ) )
				return 0.0f;

			return GetAxis( (uint)JoystickManager.ToAxis( axis ) );
		}

		/// <summary>
		///   If the axis is engaged enough to be pressed.
		/// </summary>
		/// <param name="axis">
		///   The index of the axis.
		/// </param>
		/// <param name="bidir">
		///   If a negative axis value should trigger a press.
		/// </param>
		/// <returns>
		///   If the axis is engaged enough to be pressed. 
		/// </returns>
		public bool AxisIsPressed( uint axis, bool bidir = false )
		{
			return ( bidir ? Math.Abs( GetAxis( axis ) ) : GetAxis( axis ) ) >= Input.AxisPressThreshold;
		}
		/// <summary>
		///   If the axis is engaged enough to be pressed.
		/// </summary>
		/// <param name="axis">
		///   The index of the axis.
		/// </param>
		/// <param name="bidir">
		///   If a negative axis value should trigger a press.
		/// </param>
		/// <returns>
		///   If the axis is engaged enough to be pressed. 
		/// </returns>
		public bool AxisIsPressed( XAxis axis, bool bidir = false )
		{
			return ( bidir ? Math.Abs( GetAxis( axis ) ) : GetAxis( axis ) ) >= Input.AxisPressThreshold;
		}
		/// <summary>
		///   If the axis is engaged enough to be pressed.
		/// </summary>
		/// <param name="axis">
		///   The name of the axis.
		/// </param>
		/// <param name="bidir">
		///   If a negative axis value should trigger a press.
		/// </param>
		/// <returns>
		///   If the axis is engaged enough to be pressed. 
		/// </returns>
		public bool AxisIsPressed( string axis, bool bidir = false )
		{
			return ( bidir ? Math.Abs( GetAxis( axis ) ) : GetAxis( axis ) ) >= Input.AxisPressThreshold;
		}

		/// <summary>
		///   Deep coppies the object.
		/// </summary>
		/// <returns>
		///   A deep copy of the object.
		/// </returns>
		public object Clone()
		{
			return new JoystickState( this );
		}

		/// <summary>
		///   Checks if this object is equal to another.
		/// </summary>
		/// <param name="other">
		///   The object to check against.
		/// </param>
		/// <returns>
		///   True if this object is considered equal to the given object.
		/// </returns>
		public bool Equals( JoystickState other )
		{
			for( uint i = 0; i < ButtonCount; i++ )
				if( m_button[ i ] != other.m_button[ i ] )
					return false;
			for( uint i = 0; i < AxisCount; i++ )
				if( m_axis[ i ] != other.m_axis[ i ] )
					return false;

			return true;
		}
		/// <summary>
		///   Checks if this object is equal to another.
		/// </summary>
		/// <param name="obj">
		///   The object to check against.
		/// </param>
		/// <returns>
		///   True if this object is considered equal to the given object.
		/// </returns>
		public override bool Equals( object obj )
		{
			return Equals( obj as JoystickState );
		}
		/// <summary>
		///   Serves the default hash function.
		/// </summary>
		/// <returns>
		///   A hash code for the current object.
		/// </returns>
		public override int GetHashCode()
		{
			return HashCode.Combine( m_axis, m_button );
		}

		/// <summary>
		///   Array containing axis values.
		/// </summary>
		protected float[] m_axis;
		/// <summary>
		///   Array containing buttin values.
		/// </summary>
		protected bool[] m_button;
	}
}
