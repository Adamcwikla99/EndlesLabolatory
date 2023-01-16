using UnityEngine;

namespace Events.UI
{
	/// <summary>
	/// Class responsible for relaying activation and deactivation of player pointer events
	/// </summary>
	[CreateAssetMenu(fileName = "TogglePlayerPointer", menuName = "DisplayedUIStats/TogglePlayerPointer")]
	public class TogglePlayerPointer : ScriptableObject
	{
		public System.Action<bool> ToggleState;

	}
}
