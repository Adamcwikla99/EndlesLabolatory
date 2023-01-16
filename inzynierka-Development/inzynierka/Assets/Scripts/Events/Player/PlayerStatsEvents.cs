using UnityEngine;

namespace Events.Player
{
    /// <summary>
    ///  Class responsible for relaying player stats modyfication events
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerStatsEvents", menuName = "Player/PlayerStatsEvents")]
    public class PlayerStatsEvents : ScriptableObject
    {
        public System.Action<Structures.Enums.PlayerStats,float> AddStat;
        public System.Func<Structures.Enums.PlayerStats,float> GetStat;
        
    }
}
