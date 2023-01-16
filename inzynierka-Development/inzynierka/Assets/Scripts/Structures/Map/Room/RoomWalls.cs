using System;
using System.Collections.Generic;
using UnityEngine;

namespace Structures.Map.Room
{
	/// <summary>
	///  Data wrapper for room walls
	/// </summary>
	[Serializable]
	public class RoomWalls
	{
		#region Serialized Fields

		[SerializeField]
		public List<RoomWall> walls;

		#endregion
	}
}
