using System;
using Structures.Enums;
using UnityEngine;

namespace Events.Menu
{
	/// <summary>
	///  Class responsible for relaying switch menu events
	/// </summary>
	[CreateAssetMenu(fileName = "SwitchMenuEvent", menuName = "GameEvents/SwitchMenuEvent")]
	public class SwitchMenu : ScriptableObject
	{
		public Action<MenuType> switchMenu;
	}
}
