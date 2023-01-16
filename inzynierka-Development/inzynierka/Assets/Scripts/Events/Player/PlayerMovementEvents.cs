using UnityEngine;

namespace Events.Player
{
    /// <summary>
    ///  Class responsible for relaying re-enable player jump event
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerMovementEvents", menuName = "Player/PlayerMovementEvents")]
    public class PlayerMovementEvents : ScriptableObject
    {
        public System.Action<GameObject> PlayerCanJump;
    }
}
