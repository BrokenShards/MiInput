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
	public sealed class KeyboardManager
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
		public static Keyboard.Key? ToKey( string val )
		{
			if( !IsKey( val ) )
				return null;

			if( !Enum.TryParse( val, true, out Keyboard.Key key ) )
				return (Keyboard.Key)uint.Parse( val );

			return key;
		}


		/// <summary>
		///   Constructs the instance.
		/// </summary>
		public KeyboardManager()
		{
			m_current = new KeyboardState();
			m_last    = new KeyboardState();
		}

		/// <summary>
		///   Updates the keyboard states.
		/// </summary>
		public void Update()
		{
			m_last = new KeyboardState( m_current );
			m_current.Update();
		}

		/// <summary>
		///   Check if the given key is pressed.
		/// </summary>
		/// <param name="key">
		///   The key to check.
		/// </param>
		/// <returns>
		///   True if the given key is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( Keyboard.Key key )
		{
			return m_current.IsPressed( key );
		}
		/// <summary>
		///   Check if the given key is pressed.
		/// </summary>
		/// <param name="key">
		///   The name of the key to check.
		/// </param>
		/// <returns>
		///   True if the given key is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( string key )
		{
			return m_current.IsPressed( key );
		}
		/// <summary>
		///   Check if the given key was just pressed.
		/// </summary>
		/// <param name="key">
		///   The key to check.
		/// </param>
		/// <returns>
		///   True if the given key was just pressed and false otherwise.
		/// </returns>
		public bool JustPressed( Keyboard.Key key )
		{
			return( m_current.IsPressed( key ) && !m_last.IsPressed( key ) );
		}
		/// <summary>
		///   Check if the given key was just pressed.
		/// </summary>
		/// <param name="key">
		///   The key to check.
		/// </param>
		/// <returns>
		///   True if the given key was just pressed and false otherwise.
		/// </returns>
		public bool JustPressed( string key )
		{
			return ( m_current.IsPressed( key ) && !m_last.IsPressed( key ) );
		}
		/// <summary>
		///   Check if the given key was just released.
		/// </summary>
		/// <param name="key">
		///   The key to check.
		/// </param>
		/// <returns>
		///   True if the given key was just released and false otherwise.
		/// </returns>
		public bool JustReleased( Keyboard.Key key )
		{
			return( !m_current.IsPressed( key ) && m_last.IsPressed( key ) );
		}
		/// <summary>
		///   Check if the given key was just released.
		/// </summary>
		/// <param name="key">
		///   The key to check.
		/// </param>
		/// <returns>
		///   True if the given key was just released and false otherwise.
		/// </returns>
		public bool JustReleased( string key )
		{
			return ( !m_current.IsPressed( key ) && m_last.IsPressed( key ) );
		}

		private KeyboardState m_current,
							  m_last;
	}
}
