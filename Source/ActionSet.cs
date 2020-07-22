////////////////////////////////////////////////////////////////////////////////
// ActionSet.cs 
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;

using SharpLogger;
using SharpID;

namespace SFInput
{
	/// <summary>
	///   A collection of actions.
	/// </summary>
	public class ActionSet : IEnumerable<KeyValuePair<string, Action>>
	{
		/// <summary>
		///   Constructor
		/// </summary>
		public ActionSet()
		{
			m_actions = new Dictionary<string, Action>();
		}
		/// <summary>
		///   Copy constructor.
		/// </summary>
		/// <param name="a"></param>
		public ActionSet( ActionSet a )
		{
			m_actions = a.Empty ? new Dictionary<string, Action>() : new Dictionary<string, Action>( a.Count );
		}

		public Action this[ string a ]
		{
			get { return Get( a ); }
		}

		/// <summary>
		///   If the set contains no actions.
		/// </summary>
		public bool Empty
		{
			get { return Count == 0; }
		}
		/// <summary>
		///   The amount of actions in the set.
		/// </summary>
		public int Count
		{
			get { return m_actions.Count; }
		}
		/// <summary>
		///   if the set contains either the given action, or an action with the same name.
		/// </summary>
		/// <param name="a">
		///   The action to check.
		/// </param>
		/// <returns>
		///   True if the set contains the action and false otherwise.
		/// </returns>
		public bool Contains( Action a )
		{
			if( a == null )
				return false;

			return m_actions.ContainsValue( a ) || Contains( a.Name );
		}
		/// <summary>
		///   if the set contains an action with the given name.
		/// </summary>
		/// <param name="action">
		///   The action string to check.
		/// </param>
		/// <returns>
		///   True if the set contains an action with the given name and false otherwise.
		/// </returns>
		public bool Contains( string action )
		{
			if( action == null )
				return false;

			foreach( var v in m_actions )
				if( v.Key.ToLower() == action.ToLower() )
					return true;

			return false;
		}
		/// <summary>
		///   Gets the action with the given name from the set if it exists.
		/// </summary>
		/// <param name="action">
		///   The name of the action.
		/// </param>
		/// <returns>
		///   The action with the given name in the set if it esists, otherwise false.
		/// </returns>
		public Action Get( string action )
		{
			if( action == null )
				return null;

			foreach( var v in m_actions )
				if( v.Key.ToLower() == action.ToLower() )
					return v.Value;

			return null;
		}

		/// <summary>
		///  Adds an action to the set.
		/// </summary>
		/// <param name="a">
		///   The action to add.
		/// </param>
		/// <param name="replace">
		///   If an action with the same name exists in the set, should it be replaced?
		/// </param>
		/// <returns>
		///   True if the action was successfully added to the set and false otherwise.
		/// </returns>
		public bool Add( Action a, bool replace = false )
		{
			if( a == null || !a.IsValid )
				return false;

			if( Contains( a ) )
			{
				if( !replace )
					return Logger.LogReturn( "Unable to add action to set: An action with the same ID already exists " +
					                         "and replace is false.", false, LogType.Error );

				Remove( a );
			}

			m_actions.Add( a.Name, a );
			return true;
		}

		/// <summary>
		///   Removes either the given action, or an existing action with the same name from the set.
		/// </summary>
		/// <param name="a">
		///   The action to remove.
		/// </param>
		/// <returns>
		///   True if the action existed within the set and was removed successfully and false otherwise.
		/// </returns>
		public bool Remove( Action a )
		{
			return Remove( a?.Name );
		}
		/// <summary>
		///   Removes either the given action, or an existing action with the same name from the set.
		/// </summary>
		/// <param name="action">
		///   The action to remove.
		/// </param>
		/// <returns>
		///   True if the action existed within the set and was removed successfully and false otherwise.
		/// </returns>
		public bool Remove( string action )
		{
			Action a = Get( action );

			if( a == null )
				return false;

			return m_actions.Remove( a.Name );
		}

		/// <summary>
		///   Loads the set from an xml file.
		/// </summary>
		/// <param name="path">
		///   The path of the file to load.
		/// </param>
		/// <returns>
		///   True on success and false on failure.
		/// </returns>
		public bool LoadFromFile( string path = null )
		{
			if( string.IsNullOrWhiteSpace( path ) )
				path = Input.DefaultPath;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load( path );

				XmlElement root = doc.DocumentElement;

				if( root.Name.ToLower() != "action_set" )
					return Logger.LogReturn( "Unable to load action set: root node name must be \"action_set\".", false, LogType.Error );

				foreach( var x in root.SelectNodes( "action" ) )
				{
					Action a = new Action();

					if( !a.LoadFromXml( x as XmlNode ) )
						return false;
					if( !Add( a, true ) )
						return Logger.LogReturn( "Unable to load action set: action loaded successfully but could not be added.", false, LogType.Error );
				}
			}
			catch( Exception e )
			{
				return Logger.LogReturn( "Unable to load action set: " + e.Message + ".", false, LogType.Error );
			}

			return true;
		}

		/// <summary>
		///   Returns the set as it would be in file.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
			sb.AppendLine( "<action_set>" );

			foreach( var ac in m_actions )
				sb.AppendLine( ac.Value.ToString( 1 ) );

			sb.AppendLine( "</action_set>" );
			return sb.ToString();
		}

		/// <summary>
		///   Gets an enumerator over the collection of actions.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<string, Action>> GetEnumerator()
		{
			return ( (IEnumerable<KeyValuePair<string, Action>>)m_actions ).GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ( (IEnumerable<KeyValuePair<string, Action>>)m_actions ).GetEnumerator();
		}

		private Dictionary<string, Action> m_actions;
	}
}
