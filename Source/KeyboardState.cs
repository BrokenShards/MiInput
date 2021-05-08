////////////////////////////////////////////////////////////////////////////////
// KeyboardState.cs 
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
using SFML.Window;

namespace MiInput
{
    /// <summary>
    ///   Key codes
    /// </summary>
    /// <remarks>
    ///   This is just a clone of SFML's Keyboard.Key enum without the unknown and depreciated keys.
    /// </remarks>
    public enum Key
    {
        /// <summary>The A key</summary>
        A = 0,
        /// <summary>The B key</summary>
        B,
        /// <summary>The C key</summary>
        C,
        /// <summary>The D key</summary>
        D,
        /// <summary>The E key</summary>
        E,
        /// <summary>The F key</summary>
        F,
        /// <summary>The G key</summary>
        G,
        /// <summary>The H key</summary>
        H,
        /// <summary>The I key</summary>
        I,
        /// <summary>The J key</summary>
        J,
        /// <summary>The K key</summary>
        K,
        /// <summary>The L key</summary>
        L,
        /// <summary>The M key</summary>
        M,
        /// <summary>The N key</summary>
        N,
        /// <summary>The O key</summary>
        O,
        /// <summary>The P key</summary>
        P,
        /// <summary>The Q key</summary>
        Q,
        /// <summary>The R key</summary>
        R,
        /// <summary>The S key</summary>
        S,
        /// <summary>The T key</summary>
        T,
        /// <summary>The U key</summary>
        U,
        /// <summary>The V key</summary>
        V,
        /// <summary>The W key</summary>
        W,
        /// <summary>The X key</summary>
        X,
        /// <summary>The Y key</summary>
        Y,
        /// <summary>The Z key</summary>
        Z,
        /// <summary>The 0 key</summary>
        Num0,
        /// <summary>The 1 key</summary>
        Num1,
        /// <summary>The 2 key</summary>
        Num2,
        /// <summary>The 3 key</summary>
        Num3,
        /// <summary>The 4 key</summary>
        Num4,
        /// <summary>The 5 key</summary>
        Num5,
        /// <summary>The 6 key</summary>
        Num6,
        /// <summary>The 7 key</summary>
        Num7,
        /// <summary>The 8 key</summary>
        Num8,
        /// <summary>The 9 key</summary>
        Num9,
        /// <summary>The Escape key</summary>
        Escape,
        /// <summary>The left Control key</summary>
        LControl,
        /// <summary>The left Shift key</summary>
        LShift,
        /// <summary>The left Alt key</summary>
        LAlt,
        /// <summary>The left OS specific key: window (Windows and Linux), apple (MacOS X), ...</summary>
        LSystem,
        /// <summary>The right Control key</summary>
        RControl,
        /// <summary>The right Shift key</summary>
        RShift,
        /// <summary>The right Alt key</summary>
        RAlt,
        /// <summary>The right OS specific key: window (Windows and Linux), apple (MacOS X), ...</summary>
        RSystem,
        /// <summary>The Menu key</summary>
        Menu,
        /// <summary>The [ key</summary>
        LBracket,
        /// <summary>The ] key</summary>
        RBracket,
        /// <summary>The ; key</summary>
        Semicolon,
        /// <summary>The , key</summary>
        Comma,
        /// <summary>The . key</summary>
        Period,
        /// <summary>The ' key</summary>
        Quote,
        /// <summary>The / key</summary>
        Slash,
        /// <summary>The \ key</summary>
        Backslash,
        /// <summary>The ~ key</summary>
        Tilde,
        /// <summary>The = key</summary>
        Equal,
        /// <summary>The - key</summary>
        Hyphen,
        /// <summary>The Space key</summary>
        Space,
        /// <summary>The Return key</summary>
        Enter,
        /// <summary>The Backspace key</summary>
        Backspace,
        /// <summary>The Tabulation key</summary>
        Tab,
        /// <summary>The Page up key</summary>
        PageUp,
        /// <summary>The Page down key</summary>
        PageDown,
        /// <summary>The End key</summary>
        End,
        /// <summary>The Home key</summary>
        Home,
        /// <summary>The Insert key</summary>
        Insert,
        /// <summary>The Delete key</summary>
        Delete,
        /// <summary>The + key</summary>
        Add,
        /// <summary>The - key</summary>
        Subtract,
        /// <summary>The * key</summary>
        Multiply,
        /// <summary>The / key</summary>
        Divide,
        /// <summary>Left arrow</summary>
        Left,
        /// <summary>Right arrow</summary>
        Right,
        /// <summary>Up arrow</summary>
        Up,
        /// <summary>Down arrow</summary>
        Down,
        /// <summary>The numpad 0 key</summary>
        Numpad0,
        /// <summary>The numpad 1 key</summary>
        Numpad1,
        /// <summary>The numpad 2 key</summary>
        Numpad2,
        /// <summary>The numpad 3 key</summary>
        Numpad3,
        /// <summary>The numpad 4 key</summary>
        Numpad4,
        /// <summary>The numpad 5 key</summary>
        Numpad5,
        /// <summary>The numpad 6 key</summary>
        Numpad6,
        /// <summary>The numpad 7 key</summary>
        Numpad7,
        /// <summary>The numpad 8 key</summary>
        Numpad8,
        /// <summary>The numpad 9 key</summary>
        Numpad9,
        /// <summary>The F1 key</summary>
        F1,
        /// <summary>The F2 key</summary>
        F2,
        /// <summary>The F3 key</summary>
        F3,
        /// <summary>The F4 key</summary>
        F4,
        /// <summary>The F5 key</summary>
        F5,
        /// <summary>The F6 key</summary>
        F6,
        /// <summary>The F7 key</summary>
        F7,
        /// <summary>The F8 key</summary>
        F8,
        /// <summary>The F9 key</summary>
        F9,
        /// <summary>The F10 key</summary>
        F10,
        /// <summary>The F11 key</summary>
        F11,
        /// <summary>The F12 key</summary>
        F12,
        /// <summary>The F13 key</summary>
        F13,
        /// <summary>The F14 key</summary>
        F14,
        /// <summary>The F15 key</summary>
        F15,
        /// <summary>The Pause key</summary>
        Pause,

        /// <summary>The total number of keyboard keys</summary>
        KeyCount
    };

    /// <summary>
    ///   Represents the state of the keyboard at a given moment.
    /// </summary>
    public class KeyboardState : ICloneable, IEquatable<KeyboardState>
	{
		/// <summary>
		///   Key count.
		/// </summary>
		public const uint KeyCount = (uint)Key.KeyCount;
		
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
		public bool IsPressed( Key key )
		{
			if( key < 0 || key >= Key.KeyCount )
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

		/// <summary>
		///   Checks if this object is equal to another.
		/// </summary>
		/// <param name="other">
		///   The object to check against.
		/// </param>
		/// <returns>
		///   True if this object is considered equal to the given object.
		/// </returns>
		public bool Equals( KeyboardState other )
		{
			for( uint i = 0; i < KeyCount; i++ )
				if( m_keys[ i ] != other.m_keys[ i ] )
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
            return Equals( obj as KeyboardState );
        }
        /// <summary>
		///   Serves the default hash function.
		/// </summary>
		/// <returns>
		///   A hash code for the current object.
		/// </returns>
		public override int GetHashCode()
        {
            return m_keys.GetHashCode();
        }

        private readonly bool[] m_keys;
	}
}
