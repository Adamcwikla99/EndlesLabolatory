using System.Collections.Generic;

namespace Structures.Map.Room
{
	/// <summary>
	/// class implementing compare operation for cords class
	/// </summary>
	public class RoomPositionComparer : IEqualityComparer<CordsXY>
	{
		#region Public Methods

		/// <summary>
		/// methode that check if cords are equal
		/// </summary>
		public bool Equals(CordsXY cordA, CordsXY cordB) => cordA.x == cordB.x && cordA.y == cordB.y;

		/// <summary>
		/// required hascode definition
		/// </summary>
		public int GetHashCode(CordsXY obj) => obj.x.GetHashCode() ^ obj.y.GetHashCode();

		#endregion
	}
}
