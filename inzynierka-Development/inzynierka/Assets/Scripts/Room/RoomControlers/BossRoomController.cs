using System.Collections.Generic;
using Events.Room;
using Room.RoomContent;
using Room.RoomContent.Enemys;
using Room.RoomContent.Enemys.Boss;
using Structures.Enums;
using UnityEngine;

namespace Room.RoomControlers
{
	/// <summary>
	///  Class that implements boss room controller logic
	/// </summary>
	public class BossRoomController : FloorRoom
	{
		#region properties
		
		[field: SerializeField]
		private Transform ExitGate { get; set; }
		
		[field: SerializeField]
		private List<RoomEntity> PossibleBossedList { get; set; }
		
		[field: SerializeField]
		private List<RoomEntity> SpawnedEntitys { get; set; }
		
		[field: SerializeField]
		private HostileRoomEvents HostileRoom { get; set; }
		
		[field: SerializeField]
		private GameObject ExitPortal { get; set; }

		#endregion
		
		#region Public Methods

		/// <summary>
		/// methode that initializes boss room initial state
		/// </summary>
		public void Init(Direction[] nonActiveGates = null, int florLevel = 1)
		{
			this.InitRoomVariables(nonActiveGates, florLevel);
			this.InitRoomGates();
			this.SpawnBoss();
		}
		
		/// <summary>
		/// methode that activates boss room entitys
		/// </summary>
		public override void ActivateRoomEntitys()
		{
			if (this.RoomActive == true)
			{
				return;
			}

			this.ActivateEnemy();
		}

		/// <summary>
		/// methode that initializes boss defeat logic - spawning reward and portal to next floor
		/// </summary>
		public void BossDefeat()
		{
			this.HostileRoom.ClearedRoom?.Invoke(this.RoomCordsPosition);
			this.ActivateExit();

		}

		/// <summary>
		/// methode that creates portal to next floor
		/// </summary>
		private void ActivateExit()
		{
			this.ExitPortal.SetActive(true);
		}
		
		/// <summary>
		/// methode that activates enemys entities
		/// </summary>
		private void ActivateEnemy()
		{
			foreach (RoomEntity roomSpawnedEntity in this.SpawnedEntitys)
			{
				roomSpawnedEntity.gameObject.SetActive(true);
			}
		}
		
		#endregion
		#region Private Methods

		/// <summary>
		/// methode that sets room position and type informations
		/// </summary>
		private void InitRoomVariables(Direction[] nonActiveGates, int florLevel)
		{
			this.nonActiveGates = nonActiveGates;
			this.florLevel = florLevel;
			this.ThisRoomType = RoomType.BossRoom;
		}

		/// <summary>
		/// methode that spawns room boss entity
		/// </summary>
		private void SpawnBoss()
		{
			RoomEntity selectedBoss = this.PossibleBossedList[Tools.GetRandomNumberFromRange(0, this.PossibleBossedList.Count - 1)];
			Vector3 bossRoomPosition = this.gameObject.transform.localPosition + new Vector3(0, 5f, 0);
			BossEnemy spawnedBoss = Instantiate(selectedBoss, bossRoomPosition, Quaternion.identity, this.gameObject.transform).GetComponent<BossEnemy>();
			spawnedBoss.SetBossRoomController(this.gameObject.GetComponent<BossRoomController>());
			this.SpawnedEntitys.Add(spawnedBoss);
		}
		
		#endregion
	}
}
