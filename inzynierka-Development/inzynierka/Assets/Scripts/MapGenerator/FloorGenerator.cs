using System.Collections.Generic;
using Events.Floor;
using Events.Game;
using Events.Room;
using Room;
using Room.RoomContent;
using Room.RoomControlers;
using Structures.Enums;
using Structures.Map;
using Structures.Map.Room;
using UnityEngine;

namespace MapGenerator
{
	/// <summary>
	/// methode that generates new game floor map and initialize it
	/// </summary>
	public class FloorGenerator : MonoBehaviour
	{
		#region Serialized Fields

		[field: SerializeField]
		private GameObject NormalRoom { get; set; }

		[field: SerializeField]
		private GameObject ShopRoom { get; set; }

		[field: SerializeField]
		private GameObject BossRoom { get; set; }

		[field: SerializeField]
		private GameObject StartRoom { get; set; }

		[field: SerializeField]
		private GameObject ItemRoom { get; set; }
		
		[field: SerializeField]
		private Transform GenerationStart { get; set; }

		[field: SerializeField]
		private FloorRoomMap LayoutGenerator { get; set; }
		
		[field: SerializeField]
		private FloorEvents IndividualFloorEvents { get; set; }
		
		[field: SerializeField]
		private int MapWidthLength { get; set; }
		
		[field: SerializeField]
		private float RoomWithGateLength { get; set; }
		
		[field: SerializeField]
		private Player.Player GamePlayer { get; set; }
		
		[field: SerializeField]
		private GameScore ManageGameScore { get; set; }

		#endregion
		#region Private Variables

		private float currentX;
		private float currentZ;
		private List<FloorRoom> spawnedMapRooms = new List<FloorRoom>();
		
		#endregion
		#region Unity Callbacks

		private void OnEnable()
		{
			this.IndividualFloorEvents.GenerateNewMap += GenerateMap;
		}

		private void OnDisable()
		{
			this.IndividualFloorEvents.GenerateNewMap -= GenerateMap;
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode responsible for starting new map generation process
		/// </summary>
		private void GenerateMap()
		{
			this.GamePlayer.SetPlayerRoomPosition(new CordsXY(5, 5));
			ClearPreviousFloor();
			GenerateNewLayout();
			GenerateMapFromLayout();
			this.IndividualFloorEvents.ReleyGeneratedMap?.Invoke(this.spawnedMapRooms);
		}

		/// <summary>
		/// methode that sets new map layout
		/// </summary>
		private void GenerateNewLayout()
		{
			this.LayoutGenerator = new FloorRoomMap(this.MapWidthLength, this.MapWidthLength);
			this.LayoutGenerator.GenerateNewMap();
		}

		/// <summary>
		/// methode that spawns room base on floor layout
		/// </summary>
		private void GenerateMapFromLayout()
		{
			Row[] mapRows = this.LayoutGenerator.GetMap();
			for (int i = 0; i < this.MapWidthLength; i++)
			{
				RowSpawningProcess(mapRows, i);
			}
		}

		/// <summary>
		/// methode responsible for rooms spawn
		/// </summary>
		private void RowSpawningProcess(Row[] mapRows, int i)
		{
			RoomType[] columns = mapRows[i].GetRowColumns();
			for (int j = 0; j <this.MapWidthLength; j++)
			{
				IndividualRoomSpawn(i, j, mapRows, columns);
			}
		}
		
		/// <summary>
		/// methode responsible for removing previously generated map
		/// </summary>
		private void ClearPreviousFloor()
		{
			for (int i = this.spawnedMapRooms.Count - 1; i >= 0; i--)
			{
				Destroy(spawnedMapRooms[i].gameObject);
				this.spawnedMapRooms.RemoveAt(i);
			}
		}
		
		/// <summary>
		/// methode responsible for individual room spawn
		/// </summary>
		private void IndividualRoomSpawn(int i, int j, Row[] mapRows, RoomType[] columns)
		{
			float roomCenterAddjuster = 750f;
			Vector3 positionAdjuster = new Vector3(roomCenterAddjuster, 0, roomCenterAddjuster);
			Vector3 spawnedRoomPosition = new Vector3(this.RoomWithGateLength * i + this.GenerationStart.position.x, 0, this.RoomWithGateLength * j + this.GenerationStart.position.y);
			spawnedRoomPosition -= positionAdjuster;
			GameObject spawned = null;
			CordsXY currentCord = new CordsXY(i, j);
			List<Direction> roomDirections = GetGatesToDisable(i, j, mapRows);
			SpawnCorrectRoom(spawned, columns[j], spawnedRoomPosition, currentCord, roomDirections);
		}

		/// <summary>
		/// methode that spawns room of correct type base of map cell layout
		/// </summary>
		private void SpawnCorrectRoom(GameObject spawned, RoomType column, Vector3 spawnedRoomPosition, CordsXY currentCord, List<Direction> roomDirections)
		{
			switch (column)
			{
				case RoomType.Nothing:
					break;
				case RoomType.StartRoom :
					SpawnStartRoom(spawned, column, spawnedRoomPosition, currentCord, roomDirections);
					break;
				case RoomType.ShopRoom :
					SpawnShopRoom(spawned, column, spawnedRoomPosition, currentCord, roomDirections);
					break;
				case RoomType.BossRoom :
					SpawnBossRoom(spawned, column, spawnedRoomPosition, currentCord, roomDirections);
					break;
				case RoomType.NormalRoom :
					SpawnNormalRoom(spawned, column, spawnedRoomPosition, currentCord, roomDirections);
					break;
				case RoomType.ItemRoom :
					SpawnItemRoom(spawned, column, spawnedRoomPosition, currentCord, roomDirections);
					break;
				case RoomType.NoNeighbor :
					break;
				default :
					break;
			}
		}

		/// <summary>
		/// methode responsible for spawning start room 
		/// </summary>
		private void SpawnStartRoom(GameObject spawned, RoomType column, Vector3 spawnedRoomPosition, CordsXY currentCord, List<Direction> roomDirections)
		{
			spawned = Instantiate(this.StartRoom, spawnedRoomPosition, Quaternion.identity);
			spawned.transform.SetParent(this.GenerationStart.transform);
			StartRoomController spawnedRoomController = spawned.GetComponent<StartRoomController>();
			spawnedRoomController.SetRoomPosition(spawnedRoomPosition, currentCord);
			spawnedRoomController.Init(roomDirections.ToArray());
			this.spawnedMapRooms.Add(spawnedRoomController);
		}
		
		/// <summary>
		/// methode responsible for spawning shop room 
		/// </summary>
		private void SpawnShopRoom(GameObject spawned, RoomType column, Vector3 spawnedRoomPosition, CordsXY currentCord, List<Direction> roomDirections)
		{
			spawned = Instantiate(this.ShopRoom, spawnedRoomPosition, Quaternion.identity);
			spawned.transform.SetParent(this.GenerationStart.transform);
			ShopRoomController spawnedRoomController = spawned.GetComponent<ShopRoomController>();
			spawnedRoomController.SetRoomPosition(spawnedRoomPosition, currentCord);
			spawnedRoomController.Init(roomDirections.ToArray());
			this.spawnedMapRooms.Add(spawnedRoomController);
		}
		
		/// <summary>
		/// methode responsible for spawning boss room 
		/// </summary>

		private void SpawnBossRoom(GameObject spawned, RoomType column, Vector3 spawnedRoomPosition, CordsXY currentCord, List<Direction> roomDirections)
		{
			spawned = Instantiate(this.BossRoom, spawnedRoomPosition, Quaternion.identity);
			spawned.transform.SetParent(this.GenerationStart.transform);
			BossRoomController spawnedRoomController = spawned.GetComponent<BossRoomController>();
			spawnedRoomController.SetRoomPosition(spawnedRoomPosition, currentCord);
			spawnedRoomController.Init(roomDirections.ToArray());
			this.spawnedMapRooms.Add(spawnedRoomController);
		}

		/// <summary>
		/// methode responsible for spawning enemy room 
		/// </summary>

		private void SpawnNormalRoom(GameObject spawned, RoomType column, Vector3 spawnedRoomPosition, CordsXY currentCord, List<Direction> roomDirections)
		{
			spawned = Instantiate(this.NormalRoom, spawnedRoomPosition, Quaternion.identity);
			spawned.transform.SetParent(this.GenerationStart.transform);
			EnemyRoomController spawnedRoomController = spawned.GetComponent<EnemyRoomController>();
			spawnedRoomController.SetRoomPosition(spawnedRoomPosition, currentCord);
			int beatenFloors = this.ManageGameScore.GetBeatenFloors?.Invoke() ?? 0;
			float filment = beatenFloors > 1 ? ((float)beatenFloors)/100 : 0.01f;
			spawnedRoomController.Init(roomDirections.ToArray(), filment);
			this.spawnedMapRooms.Add(spawnedRoomController);
		}

		/// <summary>
		/// methode responsible for spawning item room 
		/// </summary>

		private void SpawnItemRoom(GameObject spawned, RoomType column, Vector3 spawnedRoomPosition, CordsXY currentCord, List<Direction> roomDirections)
		{
			spawned = Instantiate(this.ItemRoom, spawnedRoomPosition, Quaternion.identity);
			spawned.transform.SetParent(this.GenerationStart.transform);
			ItemRoomController spawnedRoomController = spawned.GetComponent<ItemRoomController>();
			spawnedRoomController.SetRoomPosition(spawnedRoomPosition, currentCord);
			spawnedRoomController.Init(roomDirections.ToArray());
			this.spawnedMapRooms.Add(spawnedRoomController);
		}

		/// <summary>
		/// methode chooses with room gate are supposed to be closed 
		/// </summary>

		private List<Direction> GetGatesToDisable(int i, int j, Row[] MapRows)
		{
			List<Direction> roomDirections = new List<Direction>();
			bool borderNorthCondition, borderEastCondition, borderSouthCondition, borderWestCondition;
			bool removeNorthCondition, removeEastCondition, removeSouthCondition, removeWestCondition;
			InitBorderConditions(out borderNorthCondition, out borderEastCondition, out borderSouthCondition, out borderWestCondition, i, j);
			InitRemoveDirectionConditions(out removeNorthCondition, out removeEastCondition, out removeSouthCondition, out removeWestCondition,i, j, MapRows );

			ConditionalDirectionAdder(borderNorthCondition, removeNorthCondition, Direction.N, ref roomDirections);
			ConditionalDirectionAdder(borderEastCondition, removeEastCondition, Direction.E, ref roomDirections);
			ConditionalDirectionAdder(borderSouthCondition, removeSouthCondition, Direction.S, ref roomDirections);
			ConditionalDirectionAdder(borderWestCondition, removeWestCondition, Direction.W, ref roomDirections);

			return roomDirections;
		}

		/// <summary>
		/// methode that check if room is nest to map border
		/// </summary>
		private void InitBorderConditions(out bool borderNorthCondition, out bool borderEastCondition, out bool borderSouthCondition, out bool borderWestCondition, int i, int j)
		{
			borderNorthCondition = i == 1;
			borderEastCondition = j == this.MapWidthLength-1;
			borderSouthCondition = i == this.MapWidthLength-1;
			borderWestCondition = j == 1;
		}

		/// <summary>
		/// methode that return information about room gates not connected to next room gate
		/// </summary>
		private void InitRemoveDirectionConditions(out bool removeNorthCondition, out bool removeEastCondition, out bool removeSouthCondition, out bool removeWestCondition, int i, int j, Row[] MapRows)
		{
			removeNorthCondition =i-1>=0 && MapRows[i-1].GetRowColumns()[j] == RoomType.Nothing;
			removeEastCondition =j+1<=this.MapWidthLength-1 && MapRows[i].GetRowColumns()[j+1] == RoomType.Nothing;
			removeSouthCondition =i+1<=this.MapWidthLength-1 && MapRows[i+1].GetRowColumns()[j] == RoomType.Nothing;
			removeWestCondition =j-1>=0 && MapRows[i].GetRowColumns()[j-1] == RoomType.Nothing;
		}

		/// <summary>
		/// methode that adds room gates direction's that should be open
		/// </summary>
		private void ConditionalDirectionAdder(bool baseCondition, bool advancedCondition, Direction direction ,ref List<Direction> roomDirections)
		{
			if (baseCondition || advancedCondition)
			{
				roomDirections.Add(direction);
			}
		}
		
		#endregion
	}
}
