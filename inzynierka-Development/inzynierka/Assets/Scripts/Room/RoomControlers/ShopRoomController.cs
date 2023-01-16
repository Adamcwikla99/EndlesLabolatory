using Room.RoomContent;
using Structures.Enums;

namespace Room.RoomControlers
{
	/// <summary>
	///  Class that implements shop room logic
	/// </summary>
	public class ShopRoomController : FloorRoom
	{
		#region Unity Callbacks


		#endregion
		#region Public Methods

		/// <summary>
		/// methode that initializes shop room state
		/// </summary>
		public void Init(Direction[] nonActiveGates = null, int florLevel = 1)
		{
			this.InitRoomVariables(nonActiveGates, florLevel);
			this.InitRoomGates();
		}

		#endregion
		#region Private Methods

		/// <summary>
		/// methode that initializes shop room variables
		/// </summary>
		private void InitRoomVariables(Direction[] nonActiveGates, int florLevel)
		{
			this.nonActiveGates = nonActiveGates;
			this.florLevel = florLevel;
			this.ThisRoomType = RoomType.ShopRoom;
		}

		#endregion
	}
}
