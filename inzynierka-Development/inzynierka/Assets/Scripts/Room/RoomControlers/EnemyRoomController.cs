using System.Collections.Generic;
using Events.Room;
using Room.RoomContent;
using Room.RoomContent.Enemys;
using Room.RoomContent.Obsticle;
using Structures.Enums;
using Structures.Map.Room;
using UnityEngine;

namespace Room.RoomControlers
{
	/// <summary>
	///  Class that implements enemy room controller logic
	/// </summary>
	public class EnemyRoomController : FloorRoom
	{
		#region Public Properties

		[field: SerializeField]
		public List<RoomObstacle> RoomSpawnedObstacles { get; private set; }

		[field: SerializeField]
		public List<RoomEntity> RoomSpawnedEntitys { get; private set; }

		#endregion
		#region Private Properties
		[field: SerializeField]
		private GameObject ObstaclesHolder { get; set; }

		[field: SerializeField]
		private GameObject EntityHolder { get; set; }
		
		[field: SerializeField]
		private HostileRoomEvents HostileRoom { get; set; }
		#endregion
		#region Private Variables

		public bool forceClear = false;
		
		private int templateNumber;
		private float wallZoneDenyPercent;
		private float enemyZoneDenyPercent;
		private float enemyCombinedZonesFilmentPercent;
		private Dictionary<RoomGridCell, List<RoomZones>> allRoomZones;
		private List<EntityParty> roomEntitiesParty;
		private List<RoomObstacle> roomObstacles;
		private EnemyRoomController thisRoomController;

		#endregion
		#region Unity Callbacks

		private void Update()
		{
			this.UpdateActions();
		}
		
		#endregion
		#region Public Methods

		/// <summary>
		/// methode responsible for activation of room entity's
		/// </summary>
		public override void ActivateRoomEntitys()
		{
			if (this.RoomActive == true)
			{
				return;
			}

			this.AcctivateEnemys();
			this.AcctivateWalls();
		}
		
		/// <summary>
		/// methode that initializes initial enemy room state 
		/// </summary>
		public void Init( Direction[] nonActiveGates = null, float enemyCombinedZonesFilmentPercent = 0.01f, int florLevel = 1, int templateNumber = 1, float wallZoneDenyPercent = 0.7f, float enemyZoneDenyPercent = 0.3f
						  )
		{
			this.InitRoomVariables(nonActiveGates, florLevel, templateNumber, wallZoneDenyPercent, enemyZoneDenyPercent, enemyCombinedZonesFilmentPercent);
			this.InitRoomGates();
			this.InitRoomZones();
			this.GetRoomContent();
			this.SpawnRoomContent();
		}

		/// <summary>
		///  methode that destroys selected room entity
		/// </summary>
		public void DestroyRoomEntity(RoomEntity toDestroy)
		{
			this.RoomSpawnedEntitys.Remove(toDestroy);
			
			toDestroy.PlayDesolveAnimation();
			if (this.RoomSpawnedEntitys.Count == 0)
			{
				this.HostileRoom.ClearedRoom?.Invoke(this.RoomCordsPosition);
			}
		}

				
		/// <summary>
		///  methode that destroys selected room obstacle
		/// </summary>
		public void DestroyRoomObstacle(RoomObstacle toDestroy)
		{
			this.RoomSpawnedObstacles.Remove(toDestroy);
			Destroy(toDestroy.gameObject);
		}
		
		#endregion
		#region Private Methods

		/// <summary>
		/// methode that initializes enemy room variables and lists
		/// </summary>
		private void InitRoomVariables(Direction[] nonActiveGates, int florLevel, int templateNumber, float wallZoneDenyPercent, float enemyZoneDenyPercent, float enemyCombinedZonesFilmentPercent)
		{
			this.nonActiveGates = nonActiveGates;
			this.florLevel = florLevel;
			this.templateNumber = templateNumber;
			this.wallZoneDenyPercent = wallZoneDenyPercent;
			this.enemyZoneDenyPercent = enemyZoneDenyPercent;
			
			this.allRoomZones = this.IndividualRoomEvents.GetRandomRoomGridTemplate?.Invoke();
			
			this.roomObstacles = new List<RoomObstacle>();
			this.roomEntitiesParty = new List<EntityParty>();
			this.RoomSpawnedEntitys = new List<RoomEntity>();
			this.RoomSpawnedObstacles = new List<RoomObstacle>();
			this.enemyCombinedZonesFilmentPercent = enemyCombinedZonesFilmentPercent;
			this.ThisRoomType = RoomType.NormalRoom;
			this.thisRoomController = this.gameObject.GetComponent<EnemyRoomController>();
		}

		/// <summary>
		/// methode that initializes room obstacles and enemy's zones
		/// </summary>
		private void InitRoomZones()
		{
			this.DisableSpecyfiedZones(RoomGridCell.Enemy, this.enemyZoneDenyPercent);
			this.DisableSpecyfiedZones(RoomGridCell.Wall, this.wallZoneDenyPercent);
		}

		/// <summary>
		/// methode that removes selected zone from generation process
		/// </summary>
		private void DisableSpecyfiedZones(RoomGridCell zonesType, float toDisable)
		{
			int toDisableCount = (int)(this.allRoomZones[zonesType].Count * toDisable);
			List<int> toDisableList = this.SelectZonesToDisable(zonesType, toDisableCount);

			toDisableList.Sort();
			// potential operationalization - skip Revers by using for loop and iterating from end 
			toDisableList.Reverse();
			foreach (int disableIndex in toDisableList)
			{
				this.allRoomZones[zonesType].RemoveAt(disableIndex);
			}

		}

		/// <summary>
		/// methode that selects zone to disable
		/// </summary>
		private List<int> SelectZonesToDisable(RoomGridCell zonesType, int toDisableCount)
		{
			List<int> toDisableList = new List<int>();
			int zonesCount = this.allRoomZones[zonesType].Count;

			for (int i = 0; i < toDisableCount; i++)
			{
				this.ChoseRandomZone(ref toDisableList, ref zonesCount);
			}

			return toDisableList;
		}

		/// <summary>
		/// methode that selects random zone form zones list
		/// </summary>
		private void ChoseRandomZone(ref List<int> toDisableList, ref int zonesCount)
		{
			int random = Tools.GetRandomNumberFromRange(0, zonesCount - 1);
			while (toDisableList.Contains(random))
			{
				random = Tools.GetRandomNumberFromRange(0, zonesCount - 1);
			}

			toDisableList.Add(random);
		}
		
		/// <summary>
		/// methode that recives content to fill up room with
		/// </summary>
		private void GetRoomContent()
		{
			// TODO: later room content based on level
			this.roomEntitiesParty = this.ContentGetter.GetEnemyList?.Invoke();
			this.roomObstacles = this.ContentGetter.GetObstacleList?.Invoke();
		}

		/// <summary>
		/// methode that spawns room content
		/// </summary>
		private void SpawnRoomContent()
		{
			Vector3 roomCornerPosition = this.CornerMark.localPosition;
			this.SpawnWalls(roomCornerPosition);
			this.SpawnEnemys(roomCornerPosition);
		}

		/// <summary>
		/// methode that spawns room walls in given zone
		/// </summary>
		private void SpawnWalls(Vector3 roomCoronerPosition)
		{
			foreach (RoomZones zone in this.allRoomZones[RoomGridCell.Wall])
			{
				int chosenObsticleID = Tools.GetRandomNumberFromRange(0, this.roomObstacles.Count - 1);
				this.SpawnAllZoneWalls(zone, roomCoronerPosition, chosenObsticleID);
			}
		}

		/// <summary>
		/// methode that spawns initializes individual obstacles objects in wall zone
		/// </summary>
		private void SpawnAllZoneWalls(RoomZones zone, Vector3 roomCoronerPosition, int chosenObsticleID)
		{
			foreach (CordsXY cords in zone.GetRoomZoneCords())
			{
				this.IndividualWallSpawn(cords, zone, roomCoronerPosition, chosenObsticleID);
			}
		}
		
		/// <summary>
		/// methode responsible for individual wall object spawn
		/// </summary>
		private void IndividualWallSpawn(CordsXY cords, RoomZones zone, Vector3 roomCoronerPosition, int chosenObsticleID)
		{
			Vector3 roomCorrnerPosition = this.roomPossition;
			roomCorrnerPosition += new Vector3(roomCoronerPosition.x + cords.x, this.transform.position.y + this.roomObstacles[chosenObsticleID].transform.position.y, roomCoronerPosition.z + cords.y);
			GameObject spawnedMark = Instantiate(this.roomObstacles[chosenObsticleID].gameObject,
												 roomCorrnerPosition,
												 Quaternion.identity);

			spawnedMark.transform.SetParent(this.ObstaclesHolder.transform);
			RoomObstacle spawnedObstacle = spawnedMark.GetComponent<RoomObstacle>();
			spawnedObstacle.parentRoom = this.thisRoomController;
			this.RoomSpawnedObstacles.Add(spawnedObstacle);
		}

		/// <summary>
		/// methode responsible for spawning enemys in room 
		/// </summary>
		private void SpawnEnemys(Vector3 roomCoronerPosition)
		{
			int cellsToCover = this.CalculateEnemyCellsToCoverCount();
			int currentFilment = 0;
			List<RoomEnemyZone> enemysZones = new List<RoomEnemyZone>();
			this.InitEnemysZonesList(ref enemysZones);
			this.SpawningLoop(ref currentFilment,ref enemysZones , cellsToCover);
		}

		/// <summary>
		/// enemy spawning loop
		/// </summary>
		private void SpawningLoop(ref int currentFilment, ref List<RoomEnemyZone> enemysZones, int cellsToCover)
		{
			int safetyBreakLimit = 9000;
			int safetyBreakCount = 0;
			
			while (currentFilment < cellsToCover)
			{
				this.SpawningProcess(ref enemysZones, ref currentFilment);

				safetyBreakCount++;
				if (safetyBreakCount > safetyBreakLimit)
				{
					break;
				}
			}
		}
		
		/// <summary>
		/// methode that calculate how many cells are already covered with enemys 
		/// </summary>
		private int CalculateEnemyCellsToCoverCount()
		{
			int cellsToCover = 0;

			foreach (RoomZones zone in this.allRoomZones[RoomGridCell.Enemy])
			{
				cellsToCover += zone.GetRoomZoneCords().Count;
			}

			return (int)(cellsToCover * this.enemyCombinedZonesFilmentPercent);
		}

		/// <summary>
		/// methode that initializes enemys zones list 
		/// </summary>
		private void InitEnemysZonesList(ref List<RoomEnemyZone> enemysZones)
		{
			int i = 0;

			foreach (RoomZones zone in this.allRoomZones[RoomGridCell.Enemy])
			{
				this.IndividualEnemyZoneInit(zone, ref enemysZones, ref i);
			}
		}

		/// <summary>
		/// methode that initializes indivudual enemys zone form zones list
		/// </summary>
		private void IndividualEnemyZoneInit(RoomZones zone, ref List<RoomEnemyZone> enemysZones, ref int i)
		{
			enemysZones.Add(new RoomEnemyZone());

			foreach (CordsXY currentCord in zone.GetRoomZoneCords())
			{
				enemysZones[i].cells.Add(new RoomEnemyZoneCell(currentCord, false));
			}

			i++;
		}

		/// <summary>
		/// methode responsible for spawning process of enemys in room
		/// </summary>
		private void SpawningProcess(ref List<RoomEnemyZone> enemysZones, ref int currentFilment)
		{
			int randomZone = Tools.GetRandomNumberFromRange(0, enemysZones.Count - 1);
			int randomEnemy = Tools.GetRandomNumberFromRange(0, this.roomEntitiesParty.Count - 1);

			this.FindNewGenerationCord(ref enemysZones, ref randomZone, ref randomEnemy);
			this.SpawnEnemyParty(ref enemysZones, ref randomZone, ref randomEnemy, ref currentFilment);
		}

		/// <summary>
		/// methode that finds nex enemy generations cord
		/// </summary>
		private void FindNewGenerationCord(ref List<RoomEnemyZone> enemysZones, ref int randomZone, ref int randomEnemy)
		{
			EntityParty entityToSpawn = this.roomEntitiesParty[randomEnemy];
			int safetyBreakLimit = 300;
			int safetyBreakCount = 0;

			while (enemysZones[randomZone].TryAddEnemy(entityToSpawn.SizeX, entityToSpawn.SizeY) == false)
			{
				this.DrawNewNumbers(enemysZones, ref entityToSpawn, ref randomZone, ref randomEnemy);

				safetyBreakCount++;
				if (safetyBreakCount > safetyBreakLimit)
				{
					break;
				}
			}
		}

		/// <summary>
		/// methode that draws random zone form list and selects random enemy party to put in it
		/// </summary>
		private void DrawNewNumbers(List<RoomEnemyZone> enemysZones, ref EntityParty entityToSpawn, ref int randomZone, ref int randomEnemy)
		{
			randomZone = Tools.GetRandomNumberFromRange(0, enemysZones.Count - 1);
			randomEnemy = Tools.GetRandomNumberFromRange(0, this.roomEntitiesParty.Count - 1);
			entityToSpawn = this.roomEntitiesParty[randomEnemy];
		}

		/// <summary>
		/// methode that spawns enemy party in zone
		/// </summary>
		private void SpawnEnemyParty(ref List<RoomEnemyZone> enemysZones, ref int randomZone, ref int randomEnemy, ref int currentFilment)
		{
			GameObject spawnedMark = Instantiate(this.roomEntitiesParty[randomEnemy].gameObject, this.GetEnemyInRoomPosition(enemysZones, randomZone, randomEnemy), Quaternion.identity);
			spawnedMark.transform.SetParent(this.EntityHolder.transform);
			EntityParty entityParty = spawnedMark.GetComponent<EntityParty>();

			foreach (RoomEntity entity in entityParty.EntityPartyList)
			{
				this.RoomSpawnedEntitys.Add(entity);
				entity.parentRoom = this.thisRoomController;
				
			}

			currentFilment += this.roomEntitiesParty[randomEnemy].SizeX * this.roomEntitiesParty[randomEnemy].SizeY;
		}
		
		/// <summary>
		/// methode that returns world position of enemy in room
		/// </summary>
		private Vector3 GetEnemyInRoomPosition(List<RoomEnemyZone> enemysZones, int randomZone, int randomEnemy)
		{
			Vector3 roomCorrnerPosition = this.CornerMark.localPosition + this.roomPossition;
			float offsetX = (this.roomEntitiesParty[randomEnemy].SizeX - 1) * 0.5f;
			float offsetY = (this.roomEntitiesParty[randomEnemy].SizeY - 1) * 0.5f;

			return new Vector3(roomCorrnerPosition.x + enemysZones[randomZone].lastSpawnLocation.x + offsetX, this.transform.position.y + this.roomEntitiesParty[randomEnemy].transform.position.y,
							   roomCorrnerPosition.z + enemysZones[randomZone].lastSpawnLocation.y + offsetY);
		}

		/// <summary>
		/// methode that destroys all enemys in room
		/// </summary>
		private void ForceClearRoom()
		{
			for (int i = this.RoomSpawnedEntitys.Count-1; i >= 0; i--)
			{
				Destroy(this.RoomSpawnedEntitys[i].gameObject);
				this.RoomSpawnedEntitys.RemoveAt(i);
			}

			this.HostileRoom.ClearedRoom?.Invoke(this.RoomCordsPosition);
		}
		
		/// <summary>
		/// methode that activates all enemys in room
		/// </summary>
		private void AcctivateEnemys()
		{
			foreach (RoomEntity roomSpawnedEntity in this.RoomSpawnedEntitys)
			{
				roomSpawnedEntity.gameObject.SetActive(true);
			}
		}

		
		/// <summary>
		/// methode that activates all obstacles in room
		/// </summary>
		private void AcctivateWalls()
		{
			foreach (RoomObstacle roomSpawnedObsticle in this.RoomSpawnedObstacles)
			{
				roomSpawnedObsticle.gameObject.SetActive(true);
			}
		}
		
		/// <summary>
		/// editor metode to allow force clear methode call
		/// </summary>
		private void UpdateActions()
		{
			if (this.forceClear == true)
			{
				this.forceClear = false;
				this.ForceClearRoom();
			}
		}
		
		#endregion
	}
}
