using System;
using UnityEngine;

namespace Structures.Map.Room
{
	/// <summary>
	///  Data wrapper for x y cords
	/// </summary>
	[Serializable]
	public class CordsXY
	{
		#region Serialized Fields

		[SerializeField]
		public int x;

		[SerializeField]
		public int y;

		#endregion
		#region Constructors

		/// <summary>
		/// constructor that sets cord x y position  
		/// </summary>
		public CordsXY(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		#endregion
	}
}
