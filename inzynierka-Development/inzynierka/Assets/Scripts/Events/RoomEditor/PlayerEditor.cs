using System;
using UnityEngine;

namespace Events.RoomEditor
{
	/// <summary>
	/// Class responsible for relaying template map creation events
	/// </summary>
	[CreateAssetMenu(fileName = "PlayerEditorEvent", menuName = "GameEvents/PlayerEditorEvent")]
	public class PlayerEditor : ScriptableObject
	{
		public Action<RaycastHit> ChangeMarkerType;
		public Action MouseClick;
		public Action MousePosition;
		public Action NextCell;
		public Action PreviousCell;
		public Action<Vector2> ReadMove;
		public Action<float> ReadZoom;
	}
}
