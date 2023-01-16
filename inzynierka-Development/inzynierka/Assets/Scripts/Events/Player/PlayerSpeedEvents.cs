using UnityEngine;

namespace Events.Player
{
    /// <summary>
    ///  Class responsible for player movement speed manipulation events
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerSpeedEvents", menuName = "Player/PlayerSpeedEvents")]
    public class PlayerSpeedEvents : ScriptableObject
    {
        public System.Action<bool> AddSpeedUp;
        public System.Action<bool> AddSpeedDown;

    }
}
