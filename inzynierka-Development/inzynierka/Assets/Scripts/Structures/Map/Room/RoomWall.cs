using Room;
using Room.Tunnel;
using Structures.Enums;
using UnityEngine;

namespace Structures.Map.Room
{
	/// <summary>
	///  Class that implements room wall logic
	/// </summary>
	public class RoomWall : MonoBehaviour
	{
		#region Public Properties

		[field: SerializeField]
		public GameObject Wall { get; private set; }

		[field: SerializeField]
		public GameObject WallWithGate { get; private set; }
	
		[field: SerializeField]
		public GameObject WallGate { get; private set; }
	
		[field: SerializeField]
		public Direction WallDirection { get; private set; }
		
		[field: SerializeField]
		private TunnelController WallTunnel { get; set; }

		#endregion
		#region Unity Callbacks

		#endregion
		#region Public Methods

		/// <summary>
		/// method that set wall room neighbour 
		/// </summary>
		public void SetNeighbourAndRoomCord(CordsXY thisRoom, CordsXY neighbour)
		{
			this.WallTunnel.AlignedRoom = neighbour;
			this.WallTunnel.TunnelRoom = thisRoom;
		}
		
		/// <summary>
		/// methode that activates gate 
		/// </summary>
		/// <param name="state"></param>
		public void ActivateWallWithGate(bool state)
		{
			if (state)
			{
				this.WallWithGate.SetActive(true);
				this.Wall.SetActive(false);

				return;
			}

			this.WallWithGate.SetActive(false);
			this.Wall.SetActive(true);

		}

		#endregion
	}
}
