using System;
using System.Collections.Generic;
using Room.RoomContent;
using Room.RoomContent.Enemys;
using Room.RoomContent.Obsticle;
using UnityEngine;

namespace Events.RoomContent
{
	/// <summary>
	/// Class responsible for relaying whole possible room content
	/// </summary>
	[CreateAssetMenu(fileName = "RoomContentGetter", menuName = "RoomContentGetter/RoomContentGetter")]
	public class RoomContentGetter : ScriptableObject
	{
		public Func<int, EntityParty> GetEnemyParty;
		public Func<EntityParty> GetRandomEnemyParty;
		public Func<List<EntityParty>> GetEnemyList;
		public Func<List<RoomObstacle>> GetObstacleList;
		public Func<int, RoomObstacle> GetObstacle;
		public Func<RoomObstacle> GetRandomObstacle;
	
	
	}
}
