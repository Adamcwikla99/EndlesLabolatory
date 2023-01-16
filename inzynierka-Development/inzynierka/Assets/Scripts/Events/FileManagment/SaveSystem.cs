using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Events.FileManagment
{
	/// <summary>
	/// Class responsible for relaying update saving and loading events
	/// </summary>
	[CreateAssetMenu(fileName = "SaveSystem", menuName = "GameEvents/SaveSystem")]
	public class SaveSystem : ScriptableObject
	{
		public Func<List<RoomCellLayout>> GetAllRoomTemplates;
		public Func<string, RoomCellLayout> LoadRoomLayout;
		public Action<RoomCellLayout, string> SaveRoomLayout;
	}
}
