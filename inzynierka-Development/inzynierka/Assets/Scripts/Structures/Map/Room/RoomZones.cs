using System.Collections.Generic;
using Structures.Enums;

namespace Structures.Map.Room
{
	/// <summary>
	///  Class implementing room zones finding logic
	/// </summary>
	public class RoomZones
	{
		#region Private Variables

		private RoomGridCell zoneType;
		private List<CordsXY> zoneCords;
		private int gridSize;

		#endregion
		#region Constructors

		/// <summary>
		/// constructor that sets zone type and list of zone cords
		/// </summary>
		public RoomZones(RoomGridCell zoneType, int gridSize)
		{
			this.zoneType = zoneType;
			this.zoneCords = new List<CordsXY>();
			this.gridSize = gridSize;
		}

		/// <summary>
		/// constructor that sets zone type and list of zone cords
		/// </summary>
		public RoomZones(RoomZones toCopy)
		{
			this.gridSize = toCopy.gridSize;
			this.zoneCords = new List<CordsXY>(toCopy.zoneCords);
			this.zoneType = toCopy.zoneType;
		}

		#endregion
		#region Public Methods

		/// <summary>
		/// method that check if zone contains cord
		/// </summary>
		public bool ZoneContainsCord(CordsXY toCheck)
		{
			if (toCheck.x < 0 || toCheck.x > this.gridSize - 1 || toCheck.y < 0 || toCheck.y > this.gridSize - 1)
			{
				return false;
			}

			foreach (CordsXY cord in this.zoneCords)
			{
				if (cord.x == toCheck.x && cord.y == toCheck.y)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// methode thar returns zone type
		/// </summary>
		/// <returns></returns>
		public RoomGridCell GetZoneType() => this.zoneType;

		/// <summary>
		/// methode that adds new cord to zone
		/// </summary>
		/// <param name="newCord"></param>
		public void AddNewCord(CordsXY newCord) => this.zoneCords.Add(newCord);

		
		/// <summary>
		/// methode that returns all zone cords
		/// </summary>
		/// <returns></returns>
		public List<CordsXY> GetRoomZoneCords() => this.zoneCords.ConvertAll(cord => new CordsXY(cord.x, cord.y));

		/// <summary>
		/// methode that joins two zones into one
		/// </summary>
		/// <param name="toJoin"></param>
		public void JoinZoneCords(RoomZones toJoin)
		{
			foreach (CordsXY cord in toJoin.GetRoomZoneCords())
			{
				this.zoneCords.Add(cord);
			}

		}

		#endregion
	}
}
