using System;
using System.Collections.Generic;
using System.Linq;
using Events.Room;
using Events.RoomContent;
using Structures.Enums;
using Structures.Map.Room;
using UnityEngine;

namespace Room.RoomContent
{
	/// <summary>
	///  Class that implements floor room logic
	/// </summary>
	public abstract class FloorRoom : MonoBehaviour
	{
		#region Public Propertis
		
		[field: SerializeField]
		public RoomType ThisRoomType { get; set; }
		
		[field: SerializeField]
		public CordsXY RoomCordsPosition { get; private set; }

		[field: SerializeField]
		public bool RoomCleared { get; private set; }
		
		#endregion
		#region Protected Propertis
		
		[field: SerializeField]
		protected RoomEvents IndividualRoomEvents { get; set; }
		
		[field: SerializeField]
		protected GateEvents IndividualRoomGateEvents { get; set; }

		[field: SerializeField]
		protected RoomWalls ThisRoomWalls { get; set; }

		[field: SerializeField]
		protected RoomContentGetter ContentGetter { get; set; }

		[field: SerializeField]
		protected Transform CornerMark { get; set; }

		[field: SerializeField]
		protected List<RoomWall> WorkingRoomGates { get; set; } = new List<RoomWall>();

		[field: SerializeField]
		public bool RoomActive { get; set; }

		#endregion
		#region Protected Variables

		protected Direction[] nonActiveGates;
		protected int florLevel;
		protected Vector3 roomPossition;
		
		#endregion
		#region Public Methodes

		/// <summary>
		/// methode that sets floor room position variables
		/// </summary>
		public void SetRoomPosition(Vector3 roomPossition, CordsXY roomCordsPosition)
		{
			this.roomPossition = roomPossition;
			this.RoomCordsPosition = roomCordsPosition;
			this.InitRoomTunnels();
		}

		/// <summary>
		/// methode that sets whether room is cleared 
		/// </summary>
		public void SetRoomClearState(bool currentState)
		{
			this.RoomCleared = currentState;
		}

		/// <summary>
		/// methode that changes state of all working gates
		/// </summary>
		public void ChangeWorkingGatesState(bool newState)
		{
			foreach (RoomWall wall in this.WorkingRoomGates)
			{
				wall.WallGate.SetActive(newState);
			}
		}

		/// <summary>
		/// methode that activates room entitys
		/// </summary>
		public virtual void ActivateRoomEntitys() {  }
		
		/// <summary>
		/// methode that changes state of selected gate
		/// </summary>
		public void ChangeSpecifiedWorkingGatesState(Structures.Enums.Direction direction, bool newState)
		{
			foreach (RoomWall wall in this.WorkingRoomGates)
			{
				if (wall.WallDirection == direction)
				{
					wall.WallGate.SetActive(newState);
					return;					
				}
			}
		}
		
		#endregion
		#region Protected Methods

		/// <summary>
		/// methode that sets initial room gates state
		/// </summary>
		protected void InitRoomGates()
		{
			AcctivateAllGates();
			if (this.nonActiveGates == null || this.nonActiveGates.Length == 0)
			{
				WorkingRoomGates = this.ThisRoomWalls.walls;
				return;
			}

			DisableGatesWithoutNeighbours();
		}

		/// <summary>
		/// methode that activates all room gates
		/// </summary>
		private void AcctivateAllGates()
		{
			foreach (RoomWall wall in this.ThisRoomWalls.walls)
			{
				wall.ActivateWallWithGate(true);
			}
		}

		/// <summary>
		/// methode that disables gates that doesnt have a neighbour
		/// </summary>
		private void DisableGatesWithoutNeighbours()
		{
			foreach (RoomWall roomWall in this.ThisRoomWalls.walls)
			{
				if (this.nonActiveGates.Any(direction => roomWall.WallDirection == direction))
				{
					roomWall.ActivateWallWithGate(false);
					continue;
				}
				
				WorkingRoomGates.Add(roomWall);
			}
		}

		/// <summary>
		/// methode that sets initial values of all room runels
		/// </summary>
		protected void InitRoomTunnels()
		{
			foreach (RoomWall individualWall in this.ThisRoomWalls.walls)
			{
				DirectionSwitch(individualWall);
			}
		}

		/// <summary>
		/// methode that determines direction of neighbour room tunnel
		/// </summary>
		protected void DirectionSwitch(RoomWall individualWall)
		{
			switch (individualWall.WallDirection)
			{
				case Direction.N :
					individualWall.SetNeighbourAndRoomCord(RoomCordsPosition, new CordsXY(RoomCordsPosition.x-1, RoomCordsPosition.y));
					break;
				case Direction.W :
					individualWall.SetNeighbourAndRoomCord(RoomCordsPosition, new CordsXY(RoomCordsPosition.x, RoomCordsPosition.y-1));
					break;
				case Direction.S :
					individualWall.SetNeighbourAndRoomCord(RoomCordsPosition, new CordsXY(RoomCordsPosition.x+1, RoomCordsPosition.y));
					break;
				case Direction.E :
					individualWall.SetNeighbourAndRoomCord(RoomCordsPosition, new CordsXY(RoomCordsPosition.x, RoomCordsPosition.y+1));
					break;
				default :
					throw new ArgumentOutOfRangeException();
			}
		}
		
		#endregion
	}
}
