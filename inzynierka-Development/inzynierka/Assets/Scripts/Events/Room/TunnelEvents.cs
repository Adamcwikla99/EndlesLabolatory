using Structures.Map.Room;
using UnityEngine;

namespace Events.Room
{
    /// <summary>
    /// Class responsible for relaying tunnel traveling events
    /// </summary>
    [CreateAssetMenu(fileName = "TunnelEvents", menuName = "TunnelEvents/TunnelEvents")]
    public class TunnelEvents : ScriptableObject
    {
        public System.Action<CordsXY> AwakeRoomEntity;
        public System.Action<CordsXY> KillAllRoomEntity;
    }
}
