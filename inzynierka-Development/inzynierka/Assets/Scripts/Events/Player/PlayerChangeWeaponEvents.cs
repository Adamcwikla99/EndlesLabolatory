using UnityEngine;

namespace Events.Player
{
    /// <summary>
    ///  Class responsible for relaying player change weapon events
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerChangeWeaponEvents", menuName = "Player/PlayerChangeWeaponEvents")]
    public class PlayerChangeWeaponEvents : ScriptableObject
    {
        public System.Action<bool> ChangeWeaponType;
    }
}
