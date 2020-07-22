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
using System.Collections.Generic;
using SFML.Window;

namespace SFInput
{
	/// <summary>
	///   Represents the state of the keyboard at a given moment.
	/// </summary>
	public class KeyboardState
	{
		/// <summary>
		///   Construct a new state.
		/// </summary>
		public KeyboardState()
		{
			int count = (int)Keyboard.Key.KeyCount;

			m_key = new List<bool>( count );

			for( int i = 0; i < count; i++ )
				m_key.Add( false );
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
			if( state == null )
				throw new ArgumentNullException();

			m_key = new List<bool>( state.m_key );
		}

		/// <summary>
		///   Update to the current state of the keyboard.
		/// </summary>
		public void Update()
		{
			for( int i = 0; i < m_key.Count; i++ )
				m_key[ i ] = Keyboard.IsKeyPressed( (Keyboard.Key)i );					
		}

		/// <summary>
		///   If a given key was pressed on the last call to <see cref="Update"/>.
		/// </summary>
		/// <param name="key">
		///   The key to check.
		/// </param>
		/// <returns>
		///   True if the given key is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( Keyboard.Key key )
		{
			if( key < 0 || key >= Keyboard.Key.KeyCount )
				return false;

			return m_key[ (int)key ];
		}

		/// <summary>
		///   If a given key was pressed on the last call to <see cref="Update"/>.
		/// </summary>
		/// <param name="key">
		///   The name of the key to check.
		/// </param>
		/// <returns>
		///   True if the given key is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( string key )
		{
			if( !KeyboardManager.IsKey( key ) )
				return false;

			return IsPressed( KeyboardManager.ToKey( key ).Value );
		}


		private List<bool> m_key;
	}
}
