using System.Collections.Generic;
using System.Linq;
using Structures.Enums;
using Structures.Map;
using Structures.Map.Room;
using UnityEngine;

namespace MapGenerator.MapLayoutGeneration
{
	/// <summary>
	/// class responsible for generating shop room
	/// </summary>
	public class ShopRoomGenerator : RoomAdder
	{
		#region Private Fields

		private List<(CordsXY cord, float dist)> distanceList = new List<(CordsXY cord, float dist)>();

		#endregion
		#region Constructors

		/// <summary>
		/// constructor setting generation parameters
		/// </summary>
		/// <param name="allCordsAroundMiddle"> allowed filed cells next start room</param>
		/// <param name="pointsAroundMiddleCount">allowed filed cells around start room</param>
		/// <param name="generationParameters"> generation parameters</param>
		/// <param name="middle"> middle points cords</param>
		/// <param name="rows"> amount of map rows</param>
		/// <param name="floorDimensions">floor dimensions</param>
		/// <param name="distanceList"> map generation thresholds limits </param>
		public ShopRoomGenerator(CordsXY[] allCordsAroundMiddle, int pointsAroundMiddleCount, MapGenerationParameters generationParameters, CordsXY middle, Row[] rows, FloorDimensions floorDimensions,
								 List<(CordsXY cord, float dist)> distanceList) : base(allCordsAroundMiddle, pointsAroundMiddleCount, generationParameters, middle, rows, floorDimensions)
		{
			this.distanceList = distanceList;
		}

		#endregion
		#region Public Methods

		/// <summary>
		/// methode initializing addition process
		/// </summary>
		/// <param name="roomsNodes"> spawned rooms </param>
		/// <param name="floorEssentialRooms">spawned essential rooms </param>
		/// <param name="rowsToModify"> shop room cords candidate</param>
		/// <param name="roomType"> type of room to spawn</param>
		/// <returns>true if room was added successfully</returns>
		public bool AddShopRoom(ref FloorRoomsNodes roomsNodes, ref FloorEssentialRooms floorEssentialRooms, ref Row[] rowsToModify, Structures.Enums.RoomType roomType = RoomType.ShopRoom)
		{
			List<CordsXY> potentialShopRooms = new List<CordsXY>();

			SelectPotentialCords(ref potentialShopRooms);
			SkipEssentialRooms(ref potentialShopRooms, ref floorEssentialRooms);
			SkipPointsAroundBossRoom(ref potentialShopRooms, ref floorEssentialRooms);

			return TryAddRoom(potentialShopRooms, ref floorEssentialRooms, ref rowsToModify, roomType);
		}
		
		#endregion
		#region Private Methods

		/// <summary>
		/// methode responsible for finding cords that meet generation parameters
		/// </summary>
		/// <param name="potentialShopRooms">list of potential shop room cords</param>
		private void SelectPotentialCords(ref List<CordsXY> potentialShopRooms)
		{
			float distanceToCrosseShop = Mathf.Pow(this.middle.x, 2) * this.generationParameters.minimalShopDistanceThreshold;
			potentialShopRooms = this.distanceList.Where(x => x.dist > distanceToCrosseShop).ToList().Select(y => y.cord).ToList();
		}

		/// <summary>
		/// methode that exclude special rooms cords form potential cords list
		/// </summary>
		/// <param name="potentialShopRooms"> list of potential shop room cords</param>
		/// <param name="floorEssentialRooms"> spawned essential rooms </param>
		private void SkipEssentialRooms(ref List<CordsXY> potentialShopRooms, ref FloorEssentialRooms floorEssentialRooms)
		{
			_ = potentialShopRooms.Remove(floorEssentialRooms.bossRoomCords);
			_ = potentialShopRooms.Remove(floorEssentialRooms.startRoomCords);
		}

		/// <summary>
		/// methode that removes cords around boos room
		/// </summary>
		/// <param name="potentialShopRooms"> list of potential shop room cords</param>
		/// <param name="floorEssentialRooms"> spawned essential rooms </param>
		private void SkipPointsAroundBossRoom(ref List<CordsXY> potentialShopRooms, ref FloorEssentialRooms floorEssentialRooms)
		{
			CheckIndividualCord(ref potentialShopRooms, ref floorEssentialRooms, new CordsXY(-1, 0));
			CheckIndividualCord(ref potentialShopRooms, ref floorEssentialRooms, new CordsXY(0, -1));
			CheckIndividualCord(ref potentialShopRooms, ref floorEssentialRooms, new CordsXY(1, 0));
			CheckIndividualCord(ref potentialShopRooms, ref floorEssentialRooms, new CordsXY(0, 1));
		}

		/// <summary>
		/// methode that checks in individual cords mees generation conditions
		/// </summary>
		/// <param name="potentialShopRooms">list of potential shop room cords</param>
		/// <param name="floorEssentialRooms">spawned essential rooms</param>
		/// <param name="offset">offset from middle cord</param>
		private void CheckIndividualCord(ref List<CordsXY> potentialShopRooms, ref FloorEssentialRooms floorEssentialRooms, CordsXY offset)
		{
			if (IsElementInList(potentialShopRooms, floorEssentialRooms.bossRoomCords, offset) == false)
			{
				return;
			}

			RemoveCordFromList(ref potentialShopRooms, new CordsXY(floorEssentialRooms.bossRoomCords.x + offset.x, floorEssentialRooms.bossRoomCords.y + offset.y));
		}

		/// <summary>
		/// methode that check if cord has already assigned room type
		/// </summary>
		/// <param name="potentialShopRooms">list of potential shop room cords</param>
		/// <param name="cordToCheck">cord of room cord to check</param>
		/// <param name="offset">offset from middle cord</param>
		/// <returns> if elements is in list</returns>
		private bool IsElementInList(List<CordsXY> potentialShopRooms, CordsXY cordToCheck, CordsXY offset) =>
			potentialShopRooms.Any(cord => cord.x == cordToCheck.x + offset.x && cord.y == cordToCheck.y + offset.y);

		/// <summary>
		/// methode that trys to add shop room to map
		/// </summary>
		/// <param name="potentialShopRooms">list of potential shop room cords</param>
		/// <param name="floorEssentialRooms">spawned essential rooms</param>
		/// <param name="rowsToModify">array of rows to modyfy</param>
		/// <param name="roomType">spawned room type</param>
		/// <returns>information if room adding succeeded</returns>
		private bool TryAddRoom(List<CordsXY> potentialShopRooms, ref FloorEssentialRooms floorEssentialRooms, ref Row[] rowsToModify, Structures.Enums.RoomType roomType)
		{
			if (potentialShopRooms.Count == 0)
			{
				return false;
			}

			AddRoom(potentialShopRooms, ref floorEssentialRooms, ref rowsToModify, roomType);

			return true;
		}

		/// <summary>
		/// methode responsible for adding shop room to list map
		/// </summary>
		/// <param name="potentialShopRooms"></param>
		/// <param name="floorBeneficialRooms"></param>
		/// <param name="rowsToModify"></param>
		/// <param name="roomType"></param>
		private void AddRoom(List<CordsXY> potentialShopRooms, ref FloorEssentialRooms floorBeneficialRooms, ref Row[] rowsToModify, Structures.Enums.RoomType roomType)
		{
			int randomNUmberS = Tools.GetRandomNumberFromRange(0, potentialShopRooms.Count - 1);
			CordsXY newShopRoom = potentialShopRooms[randomNUmberS];
			rowsToModify[newShopRoom.x].SetSelectedColumnType(newShopRoom.y, roomType);
			floorBeneficialRooms.shopRoomCords = newShopRoom;
		}

		#endregion
	}
}
