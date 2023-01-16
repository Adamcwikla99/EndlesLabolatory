using System;
using Structures.Enums;
using UnityEngine;

namespace Structures.Map.Room
{
	/// <summary>
	///  Data wrapper for room cell grid
	/// </summary>
	[Serializable]
	public class RoomCellsGrid
	{
		#region Serialized Fields

		[SerializeField]
		public RoomGridCell[] e;
		[SerializeField]
		public RoomGridCell[,] roomCell = new RoomGridCell[10, 10];

		#endregion

		public int a;
	}
}
