namespace Structures.Map.Room
{
	/// <summary>
	///  Data wrapper for room enemy zone cell
	/// </summary>
	public class RoomEnemyZoneCell
	{
		#region Public Variables

		public bool occupyed;
		public CordsXY position;	

		#endregion
		#region Constructors
	
		/// <summary>
		/// constructor setting enemy zone cell position and occupation state  
		/// </summary>
		public RoomEnemyZoneCell(CordsXY position, bool occupyed)
		{
			this.position = position;
			this.occupyed = occupyed;
		}

		#endregion
	}
}
