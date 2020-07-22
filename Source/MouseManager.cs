////////////////////////////////////////////////////////////////////////////////
// MouseManager.cs 
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
		XPosition,
		YPosition,
	}

	/// <summary>
	///   Manages the state of the Mouse.
	/// </summary>
	public sealed class MouseManager
	{
		/// <summary>
		///   Checks if the given string represents a valid button.
		/// </summary>
		/// <remarks>
		///   A valid button string is either a case-insensitive <see cref="Mouse.Button"/> name or value.
		/// </remarks>
		/// <param name="val">
		///   The string to check.
		/// </param>
		/// <returns>
		///   True if the given string represents a valid mouse button and false otherwise.
		/// </returns>
		public static bool IsButton( string val )
		{
			if( string.IsNullOrEmpty( val ) )
				return false;

			if( !Enum.TryParse( val, true, out Mouse.Button but ) )
			{
				if( uint.TryParse( val, out uint b ) )
				{
					if( b >= (uint)Mouse.Button.ButtonCount )
						return false;
				}
				else
					return false;
			}

			return true;
		}
		/// <summary>
		///   Parses the given string to its mouse button representation.
		/// </summary>
		/// <param name="val">
		///   The string to parse.
		/// </param>
		/// <returns>
		///   The mouse button parsed from the string on success or null on failure.
		/// </returns>
		public static Mouse.Button? ToButton( string val )
		{
			if( !IsButton( val ) )
				return null;

			if( !Enum.TryParse( val, true, out Mouse.Button but ) )
				return (Mouse.Button)uint.Parse( val );

			return but;
		}

		/// <summary>
		///   Checks if the given string represents a valid axis.
		/// </summary>
		/// <remarks>
		///   A valid axis string is either a case-insensitive <see cref="MouseAxis"/> name or value.
		/// </remarks>
		/// <param name="val">
		///   The string to check.
		/// </param>
		/// <returns>
		///   True if the given string represents a valid mouse axis and false otherwise.
		/// </returns>
		public static bool IsAxis( string val )
		{
			if( string.IsNullOrEmpty( val ) )
				return false;

			if( !Enum.TryParse( val, true, out MouseAxis ax ) )
			{
				if( uint.TryParse( val, out uint b ) )
				{
					if( b >= Enum.GetNames( typeof( MouseAxis ) ).Length )
						return false;
				}
				else
					return false;
			}

			return true;
		}
		/// <summary>
		///   Parses the given string to its mouse axis representation.
		/// </summary>
		/// <param name="val">
		///   The string to parse.
		/// </param>
		/// <returns>
		///   The mouse axis parsed from the string on success or null on failure.
		/// </returns>
		public static MouseAxis? ToAxis( string val )
		{
			if( !IsAxis( val ) )
				return null;

			if( !Enum.TryParse( val, true, out MouseAxis ax ) )
				return (MouseAxis)uint.Parse( val );

			return ax;
		}


		/// <summary>
		///   Constructs the instance.
		/// </summary>
		public MouseManager()
		{
			m_current   = new MouseState();
			m_last      = new MouseState();
		}

		/// <summary>
		///   Updates the mouse states.
		/// </summary>
		public void Update()
		{
			m_last = new MouseState( m_current );
			m_current.Update();
		}

		/// <summary>
		///   Get the current mouse position in desktop coordinates.
		/// </summary>
		/// <returns>
		///   The current mouse position relative to the desktop.
		/// </returns>
		public Vector2i GetPosition()
		{
			return m_current.Position;
		}
		/// <summary>
		///   Get the current mouse position relative to the given window.
		/// </summary>
		/// <param name="window">
		///   The window.
		/// </param>
		/// <returns>
		///   The mouse position relative to the given window.
		/// </returns>
		public Vector2i GetPosition( Window window )
		{
			return Mouse.GetPosition( window );
		}

		/// <summary>
		///   Get the value of the given mouse axis.
		/// </summary>
		/// <param name="axis">
		///   The mouse axis.
		/// </param>
		/// <returns>
		///   The value of the given mouse axis.
		/// </returns>
		public float GetAxis( MouseAxis axis )
		{
			if( axis == MouseAxis.XPosition )
				return GetPosition().X;
			else if( axis == MouseAxis.YPosition )
				return GetPosition().Y;

			return 0.0f;
		}
		/// <summary>
		///   Get the value of the given mouse axis.
		/// </summary>
		/// <param name="axis">
		///   The name or number of the mouse axis.
		/// </param>
		/// <returns>
		///   The value of the given mouse axis.
		/// </returns>
		public float GetAxis( string axis )
		{
			if( !IsAxis( axis ) )
				return 0.0f;

			return GetAxis( ToAxis( axis ).Value );
		}

		/// <summary>
		///   Get the delta value of the given mouse axis.
		/// </summary>
		/// <param name="axis">
		///   The mouse axis.
		/// </param>
		/// <returns>
		///   The delta value of the given mouse axis.
		/// </returns>
		public float GetAxisDelta( MouseAxis axis )
		{
			if( axis == MouseAxis.XPosition )
				return m_current.Position.X - m_last.Position.X;
			else if( axis == MouseAxis.YPosition )
				return m_current.Position.Y - m_last.Position.Y;

			return 0.0f;
		}
		/// <summary>
		///   Get the delta value of the given mouse axis.
		/// </summary>
		/// <param name="axis">
		///   The name or number of the mouse axis.
		/// </param>
		/// <returns>
		///   The delta value of the given mouse axis.
		/// </returns>
		public float GetAxisDelta( string axis )
		{
			if( !IsAxis( axis ) )
				return 0.0f;

			return GetAxisDelta( ToAxis( axis ).Value );
		}

		/// <summary>
		///   Gets the value of the given mouse axis relative to the given window.
		/// </summary>
		/// <param name="axis">
		///   The mouse axis.
		/// </param>
		/// <param name="window">
		///   The window.
		/// </param>
		/// <returns>
		///   The value of the given mouse axis relative to the given window.
		/// </returns>
		public float GetAxis( MouseAxis axis, Window window )
		{
			if( axis == MouseAxis.XPosition )
				return GetPosition( window ).X;
			else if( axis == MouseAxis.YPosition )
				return GetPosition( window ).Y;

			return 0.0f;
		}

		/// <summary>
		///   Gets the value of the given mouse axis relative to the given window.
		/// </summary>
		/// <param name="axis">
		///   The name or number of the mouse axis.
		/// </param>
		/// <param name="window">
		///   The window.
		/// </param>
		/// <returns>
		///   The value of the given mouse axis relative to the given window.
		/// </returns>
		public float GetAxis( string axis, Window window )
		{
			if( !IsAxis( axis ) )
				return 0.0f;

			return GetAxis( ToAxis( axis ).Value, window );
		}

		/// <summary>
		///   Check if the given button is pressed.
		/// </summary>
		/// <param name="button">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( Mouse.Button button )
		{
			return m_current.IsPressed( button );
		}
		/// <summary>
		///   Check if the given button is pressed.
		/// </summary>
		/// <param name="button">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( string button )
		{
			return m_current.IsPressed( button );
		}
		/// <summary>
		///   Check if the given button was just pressed.
		/// </summary>
		/// <param name="button">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button was just pressed and false otherwise.
		/// </returns>
		public bool JustPressed( Mouse.Button button )
		{
			return ( m_current.IsPressed( button ) && !m_last.IsPressed( button ) );
		}
		/// <summary>
		///   Check if the given button was just pressed.
		/// </summary>
		/// <param name="button">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button was just pressed and false otherwise.
		/// </returns>
		public bool JustPressed( string button )
		{
			return ( m_current.IsPressed( button ) && !m_last.IsPressed( button ) );
		}
		/// <summary>
		///   Check if the given button was just released.
		/// </summary>
		/// <param name="button">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button was just released and false otherwise.
		/// </returns>
		public bool JustReleased( Mouse.Button button )
		{
			return ( !m_current.IsPressed( button ) && m_last.IsPressed( button ) );
		}
		/// <summary>
		///   Check if the given button was just released.
		/// </summary>
		/// <param name="button">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button was just released and false otherwise.
		/// </returns>
		public bool JustReleased( string button )
		{
			return ( !m_current.IsPressed( button ) && m_last.IsPressed( button ) );
		}

		private MouseState m_current,
						   m_last;
	}
}
