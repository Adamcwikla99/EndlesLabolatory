using Structures.Enums;

namespace Structures.Map
{
	/// <summary>
	///  Class that implements map layout row logic
	/// </summary>
	public class Row
	{
		#region Private Fields

		private readonly Column column;

		#endregion
		#region Constructors

		/// <summary>
		/// constructor responsible for initialization of row columns
		/// </summary>
		public Row(int length)
		{
			this.column = new Column(length);
			this.column.SetRowRoomType(RoomType.Nothing);
		}

		#endregion
		#region Public Methods

		/// <summary>
		/// methode that returns row columns
		/// </summary>
		public RoomType[] GetRowColumns() => this.column.GetRowData();

		/// <summary>
		/// method that sets selected column cell types 
		/// </summary>
		public void SetSelectedColumnType(int index, RoomType type) => this.column.SetSelectedRoomType(index, type);

		
		/// <summary>
		/// method that sets column cell types
		/// </summary>
		public void SetColumnsType(RoomType type) => this.column.SetRowRoomType(type);

		#endregion
	}
}
