using System.Collections.Generic;
using Events.RoomContent;
using Room.RoomContent;
using Room.RoomContent.Enemys;
using Room.RoomContent.Obsticle;
using UnityEngine;

namespace ScenesManager
{
	/// <summary>
	///  Class that oversees room content
	/// </summary>
	public class RoomContentManager : MonoBehaviour
	{
		#region Private Properties

		[field: SerializeField]
		private List<EntityParty> EntityList { get; set; }

		[field: SerializeField]
		private List<RoomObstacle> ObsticleList { get; set; }

		[field: SerializeField]
		private RoomContentGetter ContentGetter { get; set; }
		
		#endregion
		#region Unity Callbacks

		private void OnEnable()
		{
			EnableEvents();
		}

		private void OnDisable()
		{
			DisableEvents();
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// method that return random room entity from entity list
		/// </summary>
		private EntityParty GetRandomRoomEntity() => GetRoomEntity(Tools.GetRandomNumberFromRange(0, this.EntityList.Count-1));

		/// <summary>
		/// method that returns room entity list
		/// </summary>
		private List<EntityParty> GetRoomEntityList() => this.EntityList;
		
		/// <summary>
		/// method that returns room entity 
		/// </summary>
		private EntityParty GetRoomEntity(int id) => this.EntityList[id];

		/// <summary>
		/// method that returns room obstacle 
		/// </summary>
		private RoomObstacle GetRoomObstacle(int id) => this.ObsticleList[id];
		
		/// <summary>
		/// method that returns random room obstacle 
		/// </summary>
		private RoomObstacle GetRandomRoomObstacle() => this.GetRoomObstacle(Tools.GetRandomNumberFromRange(0, this.ObsticleList.Count-1));
		
		/// <summary>
		/// method that returns room obstacles list
		/// </summary>
		private List<RoomObstacle> GetRoomObstacleList() => this.ObsticleList;
		
		/// <summary>
		/// methode that enables room content list methods
		/// </summary>
		private void EnableEvents()
		{
			this.ContentGetter.GetEnemyList += GetRoomEntityList;
			this.ContentGetter.GetRandomEnemyParty += GetRandomRoomEntity;
			this.ContentGetter.GetEnemyParty += GetRoomEntity;
			this.ContentGetter.GetObstacle += GetRoomObstacle;
			this.ContentGetter.GetRandomObstacle += GetRandomRoomObstacle;
			this.ContentGetter.GetObstacleList += GetRoomObstacleList;
		}

		/// <summary>
		/// methode that disables room content list methods
		/// </summary>
		private void DisableEvents()
		{
			this.ContentGetter.GetEnemyList -= GetRoomEntityList;
			this.ContentGetter.GetRandomEnemyParty -= GetRandomRoomEntity;
			this.ContentGetter.GetEnemyParty -= GetRoomEntity;
			this.ContentGetter.GetObstacle -= this.GetRoomObstacle;
			this.ContentGetter.GetRandomObstacle -= this.GetRandomRoomObstacle;
			this.ContentGetter.GetObstacleList -= this.GetRoomObstacleList;
		}

		#endregion
	}
}
