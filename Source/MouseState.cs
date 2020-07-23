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
using System.Collections.Generic;
using SFML.System;
using SFML.Window;

namespace SFInput
{
	/// <summary>
	///   Represents the state of the mouse at a given moment.
	/// </summary>
	public class MouseState
	{
		/// <summary>
		///   Construct a new state.
		/// </summary>
		public MouseState()
		{
			Position = new Vector2i();
			int count = (int)Mouse.Button.ButtonCount;
			m_but = new List<bool>( count );

			for( int i = 0; i < count; i++ )
				m_but.Add( false );
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

			Position = state.Position;
			m_but = new List<bool>( state.m_but );
		}

		/// <summary>
		///   Mouse's desktop position.
		/// </summary>
		public Vector2i Position
		{
			get; private set;
		}
		
		/// <summary>
		///   Update to the current state of the mouse.
		/// </summary>
		public void Update()
		{
			Position = Mouse.GetPosition();

			for( int i = 0; i < m_but.Count; i++ )
				m_but[ i ] = Mouse.IsButtonPressed( (Mouse.Button)i );
		}

		/// <summary>
		///   If a given button was pressed on the last call to <see cref="Update"/>.
		/// </summary>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( Mouse.Button but )
		{
			if( but < 0 || but >= Mouse.Button.ButtonCount )
				return false;

			return m_but[ (int)but ];
		}
		/// <summary>
		///   If a given button is pressed on the last call to <see cref="Update"/>.
		/// </summary>
		/// <param name="but">
		///   The button to check.
		/// </param>
		/// <returns>
		///   True if the given button is pressed and false otherwise.
		/// </returns>
		public bool IsPressed( string but )
		{
			if( !MouseManager.IsButton( but ) )
				return false;

			return IsPressed( MouseManager.ToButton( but ).Value );
		}

		/// <summary>
		///   If a given axis is classed as pressed on the last call to <see cref="Update"/>.
		/// </summary>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis is pressed and false otherwise.
		/// </returns>
		public bool IsAxisPressed( MouseAxis axis )
		{
			if( axis == MouseAxis.XPosition )
				return Position.X >= Input.AxisPressThreshold;
			else if( axis == MouseAxis.YPosition )
				return Position.Y >= Input.AxisPressThreshold;

			return false;
		}
		/// <summary>
		///   If a given axis is pressed on the last call to <see cref="Update"/>.
		/// </summary>
		/// <param name="axis">
		///   The axis to check.
		/// </param>
		/// <returns>
		///   True if the given axis is pressed and false otherwise.
		/// </returns>
		public bool IsAxisPressed( string axis )
		{
			if( !MouseManager.IsAxis( axis ) )
				return false;

			return IsAxisPressed( MouseManager.ToAxis( axis ).Value );
		}

		private List<bool> m_but;
	}
}
