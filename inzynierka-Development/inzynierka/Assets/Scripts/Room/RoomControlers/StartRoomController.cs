using Room.RoomContent;
using Structures.Enums;

namespace Room.RoomControlers
{
	/// <summary>
	///  Class that implements start room logic
	/// </summary>
	public class StartRoomController : FloorRoom
	{
		#region Unity Callbacks


		#endregion
		#region Public Methods

		/// <summary>
		/// methode that initializes start room state
		/// </summary>
		public void Init(Direction[] nonActiveGates = null, int florLevel = 1)
		{
			this.InitRoomVariables(nonActiveGates, florLevel);
			this.InitRoomGates();
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that initializes class variables
		/// </summary>
		/// <param name="nonActiveGates"></param>
		/// <param name="florLevel"></param>
		private void InitRoomVariables(Direction[] nonActiveGates, int florLevel)
		{
			this.nonActiveGates = nonActiveGates;
			this.florLevel = florLevel;
			this.ThisRoomType = RoomType.StartRoom;
		}

		#endregion
	}
}
