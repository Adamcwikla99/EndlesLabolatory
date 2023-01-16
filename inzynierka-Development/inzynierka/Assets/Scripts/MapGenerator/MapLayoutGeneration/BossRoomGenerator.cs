using System.Collections.Generic;
using System.Linq;
using Structures.Enums;
using Structures.Map;
using Structures.Map.Room;
using UnityEngine;

namespace MapGenerator.MapLayoutGeneration
{
	/// <summary>
	///  class responsible for generating boss room
	/// </summary>
	public class BossRoomGenerator : RoomAdder
	{
		#region Constants

		private const int REQUIRED_EMPTY_SPACES = 3;

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
		public BossRoomGenerator(CordsXY[] allCordsAroundMiddle, int pointsAroundMiddleCount, MapGenerationParameters generationParameters, CordsXY middle, Row[] rows, FloorDimensions floorDimensions)
			: base(allCordsAroundMiddle, pointsAroundMiddleCount, generationParameters, middle, rows, floorDimensions)
		{
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
		public bool AddBossRooms(ref FloorRoomsNodes roomsNodes, ref FloorEssentialRooms floorEssentialRooms, ref Row[] rowsToModyfy, ref List<(CordsXY cord, float dist)> distanceListOut)
		{
			List<(CordsXY cord, float dist)> distanceList = new List<(CordsXY cord, float dist)>();
			List<CordsXY> potentialBossRooms = new List<CordsXY>();

			CalculatePointsDistance(roomsNodes, ref distanceList);
			RemovePointsNotMeetingConditions(ref potentialBossRooms, distanceList);

			return TryAddRoom(ref roomsNodes, ref floorEssentialRooms, potentialBossRooms, ref rowsToModyfy, ref distanceListOut, distanceList);
		}
		
		#endregion
		#region Private Methods

		/// <summary>
		/// methode that calculets point distance form middle
		/// </summary>
		/// <param name="roomsNodes"> spawned rooms </param>
		/// <param name="distanceList"> map generation thresholds limits </param>
		private void CalculatePointsDistance(FloorRoomsNodes roomsNodes, ref List<(CordsXY cord, float dist)> distanceList)
		{
			foreach (CordsXY cord in roomsNodes.AllCords)
			{
				distanceList.Add((cord, Tools.DistanceCounter(this.middle, cord)));
			}

			distanceList = distanceList.OrderBy(x => x.dist).ToList();
		}

		/// <summary>
		/// methode that discards point that doesnt meet generation conditiones
		/// </summary>
		/// <param name="potentialBossRooms"></param>
		/// <param name="distanceList"></param>
		private void RemovePointsNotMeetingConditions(ref List<CordsXY> potentialBossRooms, List<(CordsXY cord, float dist)> distanceList)
		{
			float distanceToCrosse = Mathf.Pow(this.middle.x, 2) * 1 * this.generationParameters.minimalBossDistanceThreshold;
			potentialBossRooms = distanceList.Where(x => x.dist > distanceToCrosse).ToList().Select(y => y.cord).ToList();
			potentialBossRooms = potentialBossRooms.Where(y => this.EmptyNeighbourCount(y) == REQUIRED_EMPTY_SPACES).ToList();
		}

		/// <summary>
		/// methode that trys to add shop room to map
		/// </summary>
		/// <param name="roomsNodes"> spawned rooms</param>
		/// <param name="floorEssentialRooms">floor essential rooms</param>
		/// <param name="potentialBossRooms"> potential boss rooms</param>
		/// <param name="rowsToModyfy"> current map</param>
		/// <param name="distanceListOut"> returned list od points distance to middle</param>
		/// <param name="distanceList">returned list od points distance to middle</param>
		/// <returns></returns>
		private bool TryAddRoom(ref FloorRoomsNodes                  roomsNodes,      ref FloorEssentialRooms          floorEssentialRooms, List<CordsXY> potentialBossRooms, ref Row[] rowsToModyfy,
								ref List<(CordsXY cord, float dist)> distanceListOut, List<(CordsXY cord, float dist)> distanceList)
		{
			if (potentialBossRooms.Count == 0)
			{
				return false;
			}

			this.AddRoom(ref roomsNodes, ref floorEssentialRooms, potentialBossRooms, ref rowsToModyfy, ref distanceListOut, distanceList);

			return true;
		}

		/// <summary>
		///  methode that adds boss room to map
		/// </summary>
		/// <param name="roomsNodes"> spawned rooms</param>
		/// <param name="floorEssentialRooms">floor essential rooms</param>
		/// <param name="potentialBossRooms"> potential boss rooms</param>
		/// <param name="rowsToModyfy"> current map</param>
		/// <param name="distanceListOut"> returned list od points distance to middle</param>
		/// <param name="distanceList">returned list od points distance to middle</param>
		private void AddRoom(ref FloorRoomsNodes                  roomsNodes,      ref FloorEssentialRooms          floorEssentialRooms, List<CordsXY> potentialBossRooms, ref Row[] rowsToModyfy,
							 ref List<(CordsXY cord, float dist)> distanceListOut, List<(CordsXY cord, float dist)> distanceList)
		{
			int randomNUmber = Tools.GetRandomNumberFromRange(0, potentialBossRooms.Count - 1);
			CordsXY newBossRoom = potentialBossRooms[randomNUmber];
			rowsToModyfy[newBossRoom.x].SetSelectedColumnType(newBossRoom.y, RoomType.BossRoom);
			floorEssentialRooms.bossRoomCords = newBossRoom;
			distanceListOut = distanceList;
		}

		#endregion
	}
}
