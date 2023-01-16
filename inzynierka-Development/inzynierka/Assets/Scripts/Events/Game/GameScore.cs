using UnityEngine;

namespace Events.Game
{
    /// <summary>
    ///  Class responsible for relaying updating score events events
    /// </summary>
    [CreateAssetMenu(fileName = "GameScore", menuName = "GameScore/GameScore")]
    public class GameScore : ScriptableObject
    {
        public System.Action IncreaseBeatenFloorsCount;
        public System.Action IncreaseBeatenEnemysCount;
        public System.Func<int> GetBeatenFloors;
    }
}
