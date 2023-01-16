using System;
using Structures.Enums;
using UnityEngine;

namespace Data
{
	/// <summary>
	/// class responsible for storing row cells
	/// </summary>
	[Serializable]
	public class CellRowWrapper
	{
		#region Serialized Fields

		[SerializeField]
		public int[] cells;

		#endregion
		#region Constructors

		/// <summary>
		/// clas constructor
		/// </summary>
		/// <param name="cells">array of row cells</param>
		public CellRowWrapper(RoomGridCell[] cells)
		{
			this.cells = new int[cells.Length];

			for (int x = 0; x < cells.Length; x++)
			{
				this.cells[x] = (int)cells[x];
			}

		}

		#endregion
	}
}
