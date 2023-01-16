using System.Collections.Generic;
using Structures.Map.Room;

namespace Structures.Map
{
	/// <summary>
	///  data wraper for map room individual nodes and all nodes
	/// </summary>
	public class FloorRoomsNodes
	{
		#region Public Variables

		public List<CordsXY> AllCords;
		public List<CordsXY> eastNode;
		public List<CordsXY> northNode;
		public List<CordsXY> southNode;
		public List<CordsXY> westNode;		

		#endregion
		#region Public Methods

		/// <summary>
		/// methode that clears all class lists
		/// </summary>
		public void Clear()
		{
			this.AllCords.Clear();
			this.northNode.Clear();
			this.southNode.Clear();
			this.westNode.Clear();
			this.eastNode.Clear();
		}

		/// <summary>
		/// method that initializes all class lists
		/// </summary>
		public void SetEmpty()
		{
			this.AllCords = new List<CordsXY>();
			this.northNode = new List<CordsXY>();
			this.southNode = new List<CordsXY>();
			this.westNode = new List<CordsXY>();
			this.eastNode = new List<CordsXY>();
		}

		#endregion
	}
}
