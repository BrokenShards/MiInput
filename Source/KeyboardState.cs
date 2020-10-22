////////////////////////////////////////////////////////////////////////////////
// KeyboardState.cs 
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
	///   Represents the state of the keyboard at a given moment.
	/// </summary>
	public class KeyboardState : ICloneable
	{
		/// <summary>
		///   Key count.
		/// </summary>
		public const uint KeyCount = (uint)Keyboard.Key.KeyCount;
		
		/// <summary>
		///   Construct a new state.
		/// </summary>
		public KeyboardState()
		{
			m_keys = new bool[ KeyCount ];
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
		public KeyboardState( KeyboardState state )
		{
			m_keys = new bool[ KeyCount ];

			for( int i = 0; i < KeyCount; i++ )
				m_keys[ i ] = state.m_keys[ i ];
		}

		/// <summary>
		///   Updates the object to the current state of the device.
		/// </summary>
		public void Update()
		{
			for( int i = 0; i < KeyCount; i++ )
				m_keys[ i ] = Keyboard.IsKeyPressed( (Keyboard.Key)i );					
		}

		/// <summary>
		///   Reset state values.
		/// </summary>
		public virtual void Reset()
		{
			if( m_keys != null )
				for( uint i = 0; i < KeyCount; i++ )
					m_keys[ i ] = false;
		}

		/// <summary>
		///   If the key is pressed.
		/// </summary>
		/// <param name="key">
		///   The index of the key.
		/// </param>
		/// <returns>
		///   True if the key index is within range and the key is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( uint key )
		{
			if( key >= KeyCount )
				return false;

			return m_keys[ key ];
		}
		/// <summary>
		///   If the key is pressed.
		/// </summary>
		/// <param name="key">
		///   The key to check.
		/// </param>
		/// <returns>
		///   True if the key is within range and is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( Keyboard.Key key )
		{
			if( key < 0 || key >= Keyboard.Key.KeyCount )
				return false;

			return IsPressed( (uint)key );
		}
		/// <summary>
		///   If the key with the given name is pressed.
		/// </summary>
		/// <param name="key">
		///   The name of the key.
		/// </param>
		/// <returns>
		///   True if the key name is valid and the key is pressed, otherwise false.
		/// </returns>
		public bool IsPressed( string key )
		{
			if( !KeyboardManager.IsKey( key ) )
				return false;

			return IsPressed( (uint)KeyboardManager.ToKey( key ) );
		}

		/// <summary>
		///   Deep coppies the object.
		/// </summary>
		/// <returns>
		///   A deep copy of the object.
		/// </returns>
		public object Clone()
		{
			return new KeyboardState( this );
		}

		private bool[] m_keys;
	}
}
