using UnityEngine;

namespace Events.Player
{
    /// <summary>
    ///  Class responsible for relaying player fire weapon events
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerFireEvents", menuName = "Player/PlayerFireEvents")]
    public class PlayerFireEvents : ScriptableObject
    {
        public System.Action PlayerFire;
    }
}
