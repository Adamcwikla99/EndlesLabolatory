using Structures;
using Structures.Wrapper;
using UnityEngine;

namespace Events.Game
{
    /// <summary>
    ///  Class responsible for relaying enemy floor boost events
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyFloorBoost", menuName = "GameScore/EnemyFloorBoost")]
    public class EnemyFloorBoost : ScriptableObject
    {
        public System.Func<EntityBoost> GetEntityPowerUp;
    }
}
