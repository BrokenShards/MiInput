////////////////////////////////////////////////////////////////////////////////
// KeyboardManager.cs 
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
	///   Manages the state of the Keyboard.
	/// </summary>
	public class KeyboardManager
	{
		/// <summary>
		///   Checks if the given string represents a valid key.
		/// </summary>
		/// <remarks>
		///   A valid key string is either a case-insensitive <see cref="Keyboard.Key"/> name or value.
		/// </remarks>
		/// <param name="val">
		///   The string to check.
		/// </param>
		/// <returns>
		///   True if the given string represents a valid keyboard key and false otherwise.
		/// </returns>
		public static bool IsKey( string val )
		{
			if( string.IsNullOrEmpty( val ) )
				return false;

			if( !Enum.TryParse( val, true, out Keyboard.Key key ) )
			{
				if( uint.TryParse( val, out uint b ) )
				{
					if( b >= (uint)Keyboard.Key.KeyCount )
						return false;
				}
				else
					return false;
			}

			return true;
		}
		/// <summary>
		///   Parses the given string to its keyboard key representation.
		/// </summary>
		/// <param name="val">
		///   The string to parse.
		/// </param>
		/// <returns>
		///   The keyboard key parsed from the string on success or null on failure.
		/// </returns>
		public static Keyboard.Key ToKey( string val )
		{
			if( Enum.TryParse( val, true, out Keyboard.Key key ) )
				return key;
			if( uint.TryParse( val, out uint k ) && k < (uint)Keyboard.Key.KeyCount )
				return (Keyboard.Key)k;

			return Keyboard.Key.Unknown;
		}
		
		/// <summary>
		///   Constructor.
		/// </summary>
		public KeyboardManager()
		{
			m_last    = new KeyboardState();
			m_current = new KeyboardState();
		}
		/// <summary>
		///   Copy constructor.
		/// </summary>
		public KeyboardManager( KeyboardManager km )
		{
			m_last    = new KeyboardState( km.m_last );
			m_current = new KeyboardState( km.m_current );
		}

		/// <summary>
		///   Updates the managed device states.
		/// </summary>
		public void Update()
		{
			m_last = new KeyboardState( m_current );
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
		///   If the key is pressed.
		/// </summary>
		/// <param name="key">
		///   The index of the key.
		/// </param>
		/// <returns>
		///   True if the key index is valid and the key is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( Keyboard.Key key )
		{
			return m_current.IsPressed( key );
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
		///   If the key has just been pressed.
		/// </summary>
		/// <param name="key">
		///   The index of the key.
		/// </param>
		/// <returns>
		///   True if the key index is valid and the key has just been pressed, otherwise false.
		/// </returns>
		public bool JustPressed( Keyboard.Key key )
		{
			return m_current.IsPressed( key ) && !m_last.IsPressed( key );
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
		///   If the key has just been released.
		/// </summary>
		/// <param name="key">
		///   The index of the key.
		/// </param>
		/// <returns>
		///   True if the key index is valid and the key has just been released, otherwise false.
		/// </returns>
		public bool JustReleased( Keyboard.Key key )
		{
			return !m_current.IsPressed( key ) && m_last.IsPressed( key );
		}

		private KeyboardState m_current,
		                      m_last;
	}
}
