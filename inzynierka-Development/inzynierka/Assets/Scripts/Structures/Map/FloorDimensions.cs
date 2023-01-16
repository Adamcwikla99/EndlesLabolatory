namespace Structures.Map
{
	/// <summary>
	/// Data wraper for floor dimensions
	/// </summary>
	public class FloorDimensions
	{
		#region Public Variables

		public int columnsAmount;
		public int rowsAmount;

		#endregion
		#region Constructors

		/// <summary>
		/// constructor that sets floor dimensions values
		/// </summary>
		public FloorDimensions(int rowsAmount, int columnsAmount)
		{
			this.rowsAmount = rowsAmount;
			this.columnsAmount = columnsAmount;
		}

		#endregion
	}
}
