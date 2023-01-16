using UnityEngine;

namespace Events.Player
{
	/// <summary>
	/// Class responsible for relaying player object getting events
	/// </summary>
	[CreateAssetMenu(fileName = "PlayerObjectGetter", menuName = "Player/PlayerObjectGetter")]
	public class PlayerObjectGetter : ScriptableObject
	{
		public System.Func<GameObject> GetPlayerObject;
	}
}
