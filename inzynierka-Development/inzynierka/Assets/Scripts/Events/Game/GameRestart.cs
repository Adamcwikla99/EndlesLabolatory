using UnityEngine;

namespace Events.Game
{
    /// <summary>
    ///  Class responsible for relaying game restart event
    /// </summary>
    [CreateAssetMenu(fileName = "GameRestart", menuName = "Game/GameRestart")]
    public class GameRestart : ScriptableObject
    {
        public System.Action RestartScene;
    }
}
