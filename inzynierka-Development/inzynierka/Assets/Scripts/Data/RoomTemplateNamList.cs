using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	/// <summary>
	/// Data wraper for list of RoomNameTemplatePari
	/// </summary>
	[Serializable]
	public class RoomTemplateNamList
	{
		#region Serialized Fields

		[SerializeField]
		public List<RoomNameTemplatePari> roomNameTemplateParis = new List<RoomNameTemplatePari>();

		#endregion
	}
}
