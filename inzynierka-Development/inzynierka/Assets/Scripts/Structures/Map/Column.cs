using System;
using Structures.Enums;

namespace Structures.Map
{
	/// <summary>
	///  Class that implements map layout column logic
	/// </summary>
	public class Column
	{
		#region Private Fields

		private readonly RoomType[] RowData;

		private readonly int RowLength;

		#endregion
		#region Constructors

		/// <summary>
		/// constructor responsible for setting arrays of column cells
		/// </summary>
		public Column(int length)
		{
			this.RowData = new RoomType[length];
			this.RowLength = length;
		}

		#endregion
		#region Public Methods

		/// <summary>
		/// methode that restarts row
		/// </summary>
		public void ResetRow() => Array.Clear(this.RowData, 0, this.RowData.Length);

		/// <summary>
		/// methode that sets room type of selected column cell
		/// </summary>
		public void SetSelectedRoomType(int index, RoomType type) => this.RowData[index] = type;

		/// <summary>
		/// sets all columns cell type to selected type 
		/// </summary>
		public void SetRowRoomType(RoomType type)
		{
			for (int i = 0; i < this.RowData.Length; i++)
			{
				this.RowData[i] = type;
			}
		}

		/// <summary>
		/// methode that returns arrays of column cells
		/// </summary>
		/// <returns></returns>
		public RoomType[] GetRowData() => this.RowData;

		/// <summary>
		/// methode that returns length of column array
		/// </summary>
		/// <returns></returns>
		public int GetLength() => this.RowLength;

		#endregion
	}
}
