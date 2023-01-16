using System;
using Newtonsoft.Json;
using Structures.Enums;
using UnityEngine;

namespace Data
{
	/// <summary>
	/// class responsible for representing room template layout 
	/// </summary>
	[Serializable]
	public class RoomCellLayout
	{
		#region Serialized Fields

		[SerializeField]
		private CellRowWrapper[] enumsToInt;

		#endregion
		#region Public variabels
		
		public RoomGridCell[,] forLoad;
		public RoomGridCell[][] roomCell;
		
		#endregion
		#region Constructors

		/// <summary>
		/// JsonConstructor required constructor
		/// </summary>
		public RoomCellLayout()
		{

		}

		/// <summary>
		/// JsonConstructor custorm constructor that allows saving to json file format
		/// </summary>
		/// <param name="roomCell"></param>
		[JsonConstructor]
		public RoomCellLayout(CellRowWrapper[] roomCell)
		{
			this.enumsToInt = roomCell;
			this.enumsToInt = new CellRowWrapper[roomCell.Length];
			this.forLoad = new RoomGridCell[roomCell.Length, roomCell.Length];

			for (int x = 0; x < roomCell.Length; x++)
			{
				for (int y = 0; y < roomCell.Length; y++)
				{
					this.forLoad[x, y] = (RoomGridCell)roomCell[x].cells[y];
				}
			}
		}

		/// <summary>
		/// constructor responsible for setting room template cell data
		/// </summary>
		/// <param name="roomCell"></param>
		public RoomCellLayout(RoomGridCell[,] roomCell)
		{
			this.enumsToInt = new CellRowWrapper[roomCell.GetLength(0)];
			this.roomCell = new RoomGridCell[roomCell.GetLength(0)][];

			for (int x = 0; x < roomCell.GetLength(0); x++)
			{
				this.roomCell[x] = new RoomGridCell[roomCell.GetLength(1)];
			}

			for (int x = 0; x < roomCell.GetLength(0); x++)
			{
				for (int y = 0; y < roomCell.GetLength(1); y++)
				{
					this.roomCell[x][y] = roomCell[x, y];
				}

				this.enumsToInt[x] = new CellRowWrapper(this.roomCell[x]);
			}
		}

		#endregion
	}
}
