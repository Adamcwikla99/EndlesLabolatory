using Structures.Map.Room;
using UnityEngine;

namespace Structures.Map
{
	/// <summary>
	/// Data wraper for floor essential rooms
	/// </summary>
	public class FloorEssentialRooms
	{
		#region Public Variables

		public CordsXY bossRoomCords;
		public CordsXY shopRoomCords;
		public CordsXY startRoomCords;	

		#endregion
		#region Constructors

		/// <summary>
		/// methode that sets all essential rooms cords
		/// </summary>
		public FloorEssentialRooms(CordsXY bossRoomCords, CordsXY shopRoomCords, CordsXY startRoomCords)
		{
			this.bossRoomCords = bossRoomCords;
			this.shopRoomCords = shopRoomCords;
			this.startRoomCords = startRoomCords;
		}

		public FloorEssentialRooms() { }

		#endregion
		#region Public Methods

		/// <summary>
		/// methode that check if all essential rooms has been assigned
		/// </summary>
		public bool AllAssigned()
		{
			bool correctData = true;

			if (this.bossRoomCords.x == -1 && this.bossRoomCords.y == -1)
			{
				Debug.LogError("Boss room position not asigned");
				correctData = false;
			}

			if (this.shopRoomCords.x == -1 && this.shopRoomCords.y == -1)
			{
				Debug.LogError("Shop room position not asigned");
				correctData = false;
			}

			if (this.startRoomCords.x == -1 && this.startRoomCords.y == -1)
			{
				Debug.LogError("Start room position not asigned");
				correctData = false;
			}

			return correctData;
		}

		/// <summary>
		/// method that sets initial cords value of essential rooms
		/// </summary>
		public void InitAssigned()
		{
			this.bossRoomCords = new CordsXY(-1, -1);
			this.shopRoomCords = new CordsXY(-1, -1);
			this.startRoomCords = new CordsXY(-1, -1);
		}

		#endregion
	}
}
