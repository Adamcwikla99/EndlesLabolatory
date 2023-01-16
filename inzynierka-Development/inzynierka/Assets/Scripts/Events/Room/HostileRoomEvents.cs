using Structures.Map.Room;
using UnityEngine;

namespace Events.Room
{
    /// <summary>
    ///  Class responsible for relaying room clear confirmation events
    /// </summary>
    [CreateAssetMenu(fileName = "HostileRoomEvents", menuName = "HostileRoomEvents/HostileRoomEvents")]
    public class HostileRoomEvents : ScriptableObject
    {
        public System.Action<CordsXY> ClearedRoom;
    }
}
