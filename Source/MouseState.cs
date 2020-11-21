////////////////////////////////////////////////////////////////////////////////
// MouseState.cs 
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
using SFML.System;
using SFML.Window;

namespace SFInput
{
	/// <summary>
	///   Mouse input axies.
	/// </summary>
	public enum MouseAxis
	{
		/// <summary>
		///   Horizontal position.
		/// </summary>
		XPosition,
		/// <summary>
		///   Vertical position.
		/// </summary>
		YPosition,

		/// <summary>
		///   Axis count.
		/// </summary>
		COUNT
	}

	/// <summary>
	///   Represents the state of the mouse at a given moment.
	/// </summary>
	public class MouseState : ICloneable, IEquatable<MouseState>
	{
		/// <summary>
		///   Button count.
		/// </summary>
		public const uint ButtonCount = (uint)Mouse.Button.ButtonCount;
		/// <summary>
		///   Axis count.
		/// </summary>
		public const uint AxisCount = (uint)MouseAxis.COUNT;

		/// <summary>
		///   Construct a new state.
		/// </summary>
		public MouseState()
		{
			Position = new Vector2i();
			m_button = new bool[ ButtonCount ];
			m_axis   = new float[ AxisCount ];
			Reset();
		}
		/// <summary>
		///   Construct a new state by copying from another instance.
		/// </summary>
		/// <param name="state">
		///   The state to copy from.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///   If <paramref name="state"/> is null.
		/// </exception>
		public MouseState( MouseState state )
		{
			if( state == null )
				throw new ArgumentNullException();

			m_button = ButtonCount > 0 ? new bool[ ButtonCount ] : null;
			m_axis = AxisCount > 0 ? new float[ AxisCount ] : null;

			if( m_button != null )
				for( uint i = 0; i < ButtonCount; i++ )
					m_button[ i ] = state.m_button[ i ];

			if( m_axis != null )
				for( uint i = 0; i < AxisCount; i++ )
					m_axis[ i ] = state.m_axis[ i ];

			Position = state.Position;
		}

		/// <summary>
		///   Mouse's desktop position.
		/// </summary>
		public Vector2i Position
		{
			get; private set;
		}

		/// <summary>
		///   The mouse position normalized in relation to the desktop.
		/// </summary>
		/// <returns>
		///   The mouse position normalized in relation to the desktop.
		/// </returns>
		public Vector2f NormalizePosition()
		{
			VideoMode desk = VideoMode.DesktopMode;
			return new Vector2f( Position.X / desk.Width, Position.Y / desk.Height );
		}

		/// <summary>
		///   The mouse position normalized in relation to a window size.
		/// </summary>
		/// <param name="size">
		///   The window size.
		/// </param>
		/// <returns>
		///   The mouse position normalized in relation to the window size.
		/// </returns>
		public Vector2f NormalizePosition( Vector2f size )
		{
			if( size.X <= 0 || size.Y <= 0 )
				return default( Vector2f );

			return new Vector2f( Position.X / size.X, Position.Y / size.Y );
		}
		/// <summary>
		///   The mouse position normalized in relation to a window position and size.
		/// </summary>
		/// <param name="pos">
		///   The window position.
		/// </param>
		/// <param name="size">
		///   The window size.
		/// </param>
		/// <returns>
		///   The mouse position normalized in relation to the window position and size.
		/// </returns>
		public Vector2f NormalizePosition( Vector2f pos, Vector2f size )
		{
			if( size.X <= 0 || size.Y <= 0 )
				return default( Vector2f );

			Vector2f position = new Vector2f( Position.X - pos.X, Position.Y - pos.Y );
			return new Vector2f( position.X / size.X, position.Y / size.Y );
		}

		/// <summary>
		///   Updates values to the current state of the mouse.
		/// </summary>
		public void Update()
		{
			Position = Mouse.GetPosition();

			for( int i = 0; i < ButtonCount; i++ )
				m_button[ i ] = Mouse.IsButtonPressed( (Mouse.Button)i );

			m_axis[ (uint)MouseAxis.XPosition ] = Position.X;
			m_axis[ (uint)MouseAxis.YPosition ] = Position.Y;
		}
		/// <summary>
		///   Resets all values.
		/// </summary>
		public void Reset()
		{
			if( m_button != null )
				for( uint i = 0; i < ButtonCount; i++ )
					m_button[ i ] = false;
			if( m_axis != null )
				for( uint i = 0; i < AxisCount; i++ )
					m_axis[ i ] = 0.0f;

			Position = default( Vector2i );
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
		public bool IsPressed( Mouse.Button but )
		{
			if( but < 0 || but >= Mouse.Button.ButtonCount )
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
			if( !MouseManager.IsButton( but ) )
				return false;

			return IsPressed( (uint)MouseManager.ToButton( but ) );
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
		public float GetAxis( MouseAxis axis )
		{
			if( axis < 0 || axis >= MouseAxis.COUNT )
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
			if( !MouseManager.IsAxis( axis ) )
				return 0.0f;

			return GetAxis( (uint)MouseManager.ToAxis( axis ) );
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
		public bool AxisIsPressed( MouseAxis axis, bool bidir = false )
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
			return new MouseState( this );
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
		public bool Equals( MouseState other )
		{
			for( uint i = 0; i < ButtonCount; i++ )
				if( m_button[ i ] != other.m_button[ i ] )
					return false;
			for( uint i = 0; i < AxisCount; i++ )
				if( m_axis[ i ] != other.m_axis[ i ] )
					return false;

			return true;
		}

		private bool[] m_button;
		private float[] m_axis;
	}
}
