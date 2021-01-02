////////////////////////////////////////////////////////////////////////////////
// MouseManager.cs 
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
using SFML.System;
using SFML.Window;

namespace MiInput
{
	/// <summary>
	///   Manages the state of the Mouse.
	/// </summary>
	public class MouseManager
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
		///   The mouse button parsed from the string on success or ButtonCount on failure.
		/// </returns>
		public static Mouse.Button ToButton( string val )
		{
			if( Enum.TryParse( val, true, out Mouse.Button but ) )
				return but;
			if( uint.TryParse( val, out uint b ) && b < (uint)Mouse.Button.ButtonCount )
				return (Mouse.Button)b;

			return Mouse.Button.ButtonCount;
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
		///   The mouse axis parsed from the string on success or MouseAxis.COUNT on failure.
		/// </returns>
		public static MouseAxis ToAxis( string val )
		{
			if( Enum.TryParse( val, true, out MouseAxis ax ) )
				return ax;
			if( uint.TryParse( val, out uint a ) && a < (uint)MouseAxis.COUNT )
				return (MouseAxis)a;

			return MouseAxis.COUNT;
		}

		/// <summary>
		///   Constructor.
		/// </summary>
		public MouseManager()
		{
			m_last    = new MouseState();
			m_current = new MouseState();
		}
		/// <summary>
		///   Copy constructor.
		/// </summary>
		public MouseManager( MouseManager mm )
		{
			m_last    = new MouseState( mm.m_last );
			m_current = new MouseState( mm.m_current );
		}

		/// <summary>
		///   Updates the managed device states.
		/// </summary>
		public void Update()
		{
			m_last = new MouseState( m_current );
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
		///   Get the current mouse position in desktop coordinates.
		/// </summary>
		/// <returns>
		///   The current mouse position relative to the desktop.
		/// </returns>
		public Vector2f GetNormalizedPosition()
		{
			return m_current.NormalizePosition();
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
		public Vector2f GetNormalizedPosition( Vector2f size )
		{
			return m_current.NormalizePosition( size );
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
		public Vector2f GetNormalizePosition( Vector2f pos, Vector2f size )
		{
			return m_current.NormalizePosition( pos, size );
		}

		/// <summary>
		///   Get the previous mouse position in desktop coordinates.
		/// </summary>
		/// <returns>
		///   The previous mouse position relative to the desktop.
		/// </returns>
		public Vector2i GetLastPosition()
		{
			return m_last.Position;
		}
		/// <summary>
		///   Get the last mouse position in desktop coordinates.
		/// </summary>
		/// <returns>
		///   The last mouse position relative to the desktop.
		/// </returns>
		public Vector2f GetLastNormalizedPosition()
		{
			return m_current.NormalizePosition();
		}
		/// <summary>
		///   The last mouse position normalized in relation to a window size.
		/// </summary>
		/// <param name="size">
		///   The window size.
		/// </param>
		/// <returns>
		///   The last mouse position normalized in relation to the window size.
		/// </returns>
		public Vector2f GetLastNormalizedPosition( Vector2f size )
		{
			return m_current.NormalizePosition( size );
		}
		/// <summary>
		///   The lastmouse position normalized in relation to a window position and size.
		/// </summary>
		/// <param name="pos">
		///   The window position.
		/// </param>
		/// <param name="size">
		///   The window size.
		/// </param>
		/// <returns>
		///   The last mouse position normalized in relation to the window position and size.
		/// </returns>
		public Vector2f GetaLastNormalizePosition( Vector2f pos, Vector2f size )
		{
			return m_current.NormalizePosition( pos, size );
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
		public bool IsPressed( Mouse.Button but )
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
		public bool JustPressed( Mouse.Button but )
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
		public bool JustReleased( Mouse.Button but )
		{
			return !m_current.IsPressed( but ) && m_last.IsPressed( but );
		}

		/// <summary>
		///   Checks if any button is currently pressed.
		/// </summary>
		/// <returns>
		///   True if any buttons are currently pressed and false otherwise.
		/// </returns>
		public bool AnyPressed()
		{
			for( Mouse.Button b = 0; b < Mouse.Button.ButtonCount; b++ )
				if( IsPressed( b ) )
					return true;

			return false;
		}
		/// <summary>
		///   Checks if any button has just been pressed.
		/// </summary>
		/// <returns>
		///   True if any buttons have just been pressed and false otherwise.
		/// </returns>
		public bool AnyJustPressed()
		{
			for( Mouse.Button b = 0; b < Mouse.Button.ButtonCount; b++ )
				if( JustPressed( b ) )
					return true;

			return false;
		}
		/// <summary>
		///   Checks if any button has just been pressed.
		/// </summary>
		/// <returns>
		///   True if any buttons have just been pressed and false otherwise.
		/// </returns>
		public bool AnyJustReleased()
		{
			for( Mouse.Button b = 0; b < Mouse.Button.ButtonCount; b++ )
				if( JustReleased( b ) )
					return true;

			return false;
		}

		/// <summary>
		///   Checks if any axis has just been moved.
		/// </summary>
		/// <returns>
		///   True if any axies have just been moved and false otherwise.
		/// </returns>
		public bool AnyJustMoved()
		{
			for( MouseAxis b = 0; b < MouseAxis.COUNT; b++ )
				if( m_current.GetAxis( b ) != m_last.GetAxis( b ) )
					return true;

			return false;
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
		public float GetAxis( MouseAxis ax )
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
		public float GetLastAxis( MouseAxis ax )
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
		///   index. Will also return zero if <paramref name="ax"/> is out of range.
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
		/// <returns>
		///   The difference in value between the last to update calls of the given axis from the given joystick player
		///   index. Will also return zero if <paramref name="ax"/> is out of range.
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
		public float AxisDelta( MouseAxis ax )
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
		public bool AxisIsPressed( MouseAxis ax )
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
		public bool AxisJustPressed( MouseAxis ax )
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
		public bool AxisJustReleased( MouseAxis ax )
		{
			return !m_current.AxisIsPressed( ax ) && m_last.AxisIsPressed( ax );
		}

		private MouseState m_current,
						   m_last;
	}
}
