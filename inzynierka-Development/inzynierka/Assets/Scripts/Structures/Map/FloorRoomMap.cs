using System;
using System.Collections.Generic;
using MapGenerator.MapLayoutGeneration;
using Structures.Enums;
using Structures.Map.Room;

namespace Structures.Map
{
	/// <summary>
	///  Class that implements floor room map generation and managment logic
	/// </summary>
	public class FloorRoomMap
	{
		#region Private Variables

		private FloorDimensions floorDimensions;

		private int gridCount;

		private bool mapGenerated;

		private MapGenerationParameters generationParameters;

		private CordsXY middle;

		private CordsXY[] allCordsAroundMiddle;

		private int pointsAroundMiddleCount;

		private FloorEssentialRooms floorEssentialRooms;

		private FloorRoomsNodes roomsNodes;

		private Row[] rows;

		private float mapFilment;

		private FloorInicjalizator mapInicjalizator;

		private BasePathsGenerator basePathsGenerator;

		private BossRoomGenerator bossRoomGenerator;

		private ShopRoomGenerator shopRoomGenerator;

		#endregion
		#region Constructors

		/// <summary>
		/// constructor that initializes class initial values
		/// </summary>
		public FloorRoomMap(int rowsAmount, int columnsAmount)
		{
			this.InitMapConsts(rowsAmount, columnsAmount);
		}

		#endregion
		#region Public Methods

		/// <summary>
		/// method that generates new map layout
		/// </summary>
		public void GenerateNewMapLayout(int rowsAmount, int columnsAmount) => this.InitMapConsts(rowsAmount, columnsAmount);

		/// <summary>
		/// methode responsible for map generation process, clears previous attempt and generates new map
		/// </summary>
		public void GenerateNewMap()
		{
			this.ClearMap();
			this.rows[this.middle.x].SetSelectedColumnType(this.middle.y, RoomType.StartRoom);

			this.Generate();

			this.mapGenerated = true;
			this.DebugFloorDisplay();
		}

		/// <summary>
		/// debuging methode responsible for displaying generated map layout on console
		/// </summary>
		public void DebugFloorDisplay()
		{
			string rowString = "";

			for (int x = 0; x < this.floorDimensions.rowsAmount; x++)
			{
				RoomType[] tempRow = this.rows[x].GetRowColumns();

				for (int y = 0; y < this.floorDimensions.columnsAmount; y++)
				{
					rowString += " " + (int)tempRow[y] + " ";
				}

				rowString += "\n";
			}
		}

		/// <summary>
		/// methode responsible for returning special rooms generated on map
		/// </summary>
		public FloorEssentialRooms GetFloorEssentialRooms() => this.floorEssentialRooms;

		/// <summary>
		/// methode that returns generated map 
		/// </summary>
		public Row[] GetMap() => this.rows;

		/// <summary>
		/// methode that returns generated floor dimensiones
		/// </summary>
		public FloorDimensions GetMapSize() => this.floorDimensions;

		
		/// <summary>
		/// methode that returns coordinates of middle point
		/// </summary>
		public CordsXY GetMiddle() => this.middle;

		/// <summary>
		/// methode that changes good move chance parameter of map generation parameter value
		/// </summary>
		public void AdjustGoodMove(int goodMove) => this.generationParameters.goodMoveChance = goodMove;

		/// <summary>
		/// methode that returns floor dimensions
		/// </summary>
		public FloorDimensions GetFloorDimensions() => this.floorDimensions;

		/// <summary>
		/// methode that allows for adjustments of floor generation parameters
		/// </summary>
		/// <param name="goodMove"></param>
		/// <param name="boosDistance"></param>
		/// <param name="shopDistance"></param>
		/// <param name="maxNodePercentage"></param>
		/// <param name="mapFillPercentage"></param>
		public void AdjustGenerationSettings(int goodMove, float boosDistance, float shopDistance, float maxNodePercentage, float mapFillPercentage)
		{
			this.generationParameters.goodMoveChance = goodMove;
			this.generationParameters.minimalBossDistanceThreshold = boosDistance;
			this.generationParameters.minimalShopDistanceThreshold = shopDistance;
			this.generationParameters.maxNodePercentage = maxNodePercentage;
			this.generationParameters.mapFillPercentage = mapFillPercentage;
		}

		/// <summary>
		/// methode that changes shop room nad boss room spawning distance parameter of map generation parameter value
		/// </summary>
		public void AdjustDistance(float boosDistance, float shopDistance)
		{
			this.generationParameters.minimalBossDistanceThreshold = boosDistance;
			this.generationParameters.minimalShopDistanceThreshold = shopDistance;
		}

		/// <summary>
		/// methode that changes map filment parameter of map generation parameter value
		/// </summary>
		public void AdjustMapFillParameters(float maxNodePercentage, float mapFillPercentage)
		{
			this.generationParameters.maxNodePercentage = maxNodePercentage;
			this.generationParameters.mapFillPercentage = mapFillPercentage;
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode responsible for removing previous attempt traces
		/// </summary>
		private void ClearMap()
		{
			this.floorEssentialRooms.InitAssigned();
			this.roomsNodes.SetEmpty();
			this.mapFilment = 0;
			this.pointsAroundMiddleCount = 0;

			for (int x = 0; x < this.floorDimensions.rowsAmount; x++)
			{
				this.rows[x].SetColumnsType(RoomType.Nothing);
			}

			this.floorEssentialRooms.InitAssigned();
			this.floorEssentialRooms.startRoomCords = this.middle;
			this.rows[this.middle.x].SetSelectedColumnType(this.middle.y, RoomType.StartRoom);
		}

		/// <summary>
		/// methode that initializes class variables 
		/// </summary>
		private void InitMapConsts(int rowsAmount, int columnsAmount)
		{
			this.mapInicjalizator = new FloorInicjalizator();
			this.mapInicjalizator.InitDimensions(rowsAmount, columnsAmount, ref this.floorDimensions, ref this.rows);
			this.mapInicjalizator.InitCordsData(this.floorDimensions, ref this.floorEssentialRooms, ref this.middle, ref this.gridCount);
			this.mapInicjalizator.InitRandomnessVariables(ref this.generationParameters, ref this.roomsNodes);
			this.mapInicjalizator.InitPointsAroundMiddle(ref this.pointsAroundMiddleCount, ref this.allCordsAroundMiddle, this.middle);
			this.mapGenerated = false;
		}

		/// <summary>
		/// methode responsible for whole floor layout generation process - setting special rooms and normal rooms
		/// </summary>
		private void Generate()
		{
			List<(CordsXY cord, float dist)> distanceList = new List<(CordsXY cord, float dist)>();
			this.GenerateBasePaths();
			this.GenerateBossRoom(ref distanceList);
			this.GenerateShopRoom(distanceList);
			this.GenerateItemRoomBasedOnShopRoom();
			FloorEssentialCheck();
		}

		/// <summary>
		/// methode responsible for setting layout base paths 
		/// </summary>
		private void GenerateBasePaths()
		{
			this.basePathsGenerator = new BasePathsGenerator(ref this.rows, this.middle, this.allCordsAroundMiddle, this.floorDimensions, this.generationParameters, ref this.roomsNodes);
			this.basePathsGenerator.GenerateBasePaths();
		}

		/// <summary>
		/// methode responsible for setting boss room on floor layout
		/// </summary>
		private void GenerateBossRoom(ref List<(CordsXY cord, float dist)> distanceList)
		{
			this.bossRoomGenerator = new BossRoomGenerator(this.allCordsAroundMiddle, this.pointsAroundMiddleCount, this.generationParameters, this.middle, this.rows, this.floorDimensions);

			if (this.bossRoomGenerator.AddBossRooms(ref this.roomsNodes, ref this.floorEssentialRooms, ref this.rows, ref distanceList) == false)
			{
				this.GenerateNewMap();
			}
		}

		/// <summary>
		/// methode responsible for setting shop room on floor layout
		/// </summary>
		private void GenerateShopRoom(List<(CordsXY cord, float dist)> distanceList)
		{
			this.shopRoomGenerator = new ShopRoomGenerator(this.allCordsAroundMiddle, this.pointsAroundMiddleCount, this.generationParameters, this.middle, this.rows, this.floorDimensions,
														   distanceList);

			if (this.shopRoomGenerator.AddShopRoom(ref this.roomsNodes, ref this.floorEssentialRooms, ref this.rows) == false)
			{
				this.GenerateNewMap();
			}
		}

		/// <summary>
		/// methode responsible for setting item room on floor layout
		/// </summary>
		private void GenerateItemRoomBasedOnShopRoom()
		{
			if (this.shopRoomGenerator.AddShopRoom(ref this.roomsNodes, ref this.floorEssentialRooms, ref this.rows, RoomType.ItemRoom) == false)
			{
				this.GenerateNewMap();
			}
		}

		/// <summary>
		/// methode check if the generator placed all essential rooms
		/// </summary>
		private void FloorEssentialCheck()
		{
			bool itemRoom = false, shopRoom = false, bossRoom = false;
			
			foreach (var row in rows)
			{
				foreach (var column in row.GetRowColumns())
				{
					IndividualColumnCheck(column, ref itemRoom, ref shopRoom, ref bossRoom);
				}
			}

			if (itemRoom == false || shopRoom == false || bossRoom== false )
			{
				this.GenerateNewMap();
			}
			
		}

		/// <summary>
		/// methode that check if selected cell if of one of special room types
		/// </summary>
		private void IndividualColumnCheck(RoomType column, ref bool itemRoom, ref bool shopRoom, ref bool bossRoom)
		{
			switch (column)
			{
				case RoomType.ShopRoom :
					shopRoom = true;
					break;
				case RoomType.BossRoom :
					bossRoom = true;
					break;
				case RoomType.ItemRoom :
					itemRoom = true;
					break;
				default :
				break;
			}
		}
		
		#endregion
	}
}
