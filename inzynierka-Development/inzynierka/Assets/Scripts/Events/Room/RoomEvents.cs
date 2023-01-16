using System;
using System.Collections.Generic;
using Structures.Enums;
using Structures.Map.Room;
using UnityEngine;

namespace Events.Room
{
	/// <summary>
	/// Class responsible for relaying room template reallying data events
	/// </summary>
	[CreateAssetMenu(fileName = "RoomEvents", menuName = "RoomEvents/RoomEvents")]
	public class RoomEvents : ScriptableObject
	{
		public Func<int, Dictionary<RoomGridCell, List<RoomZones>>> GetRoomGridTemplate;
		public Func<Dictionary<RoomGridCell, List<RoomZones>>> GetRandomRoomGridTemplate;
		
	}
}
