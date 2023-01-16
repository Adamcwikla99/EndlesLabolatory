using System.Collections.Generic;
using UnityEngine;

namespace Room.RoomContent.Enemys
{
	/// <summary>
	///  Data wraper for entity party - contains area that party covers and it members
	/// </summary>
	public class EntityParty : MonoBehaviour
	{
		#region Public Properties

		[field: SerializeField]
		public List<RoomEntity> EntityPartyList { get; private set; }

		[field: SerializeField]
		public int SizeX { get; set; }

		[field: SerializeField]
		public int SizeY { get; set; }

		#endregion
	}
}
