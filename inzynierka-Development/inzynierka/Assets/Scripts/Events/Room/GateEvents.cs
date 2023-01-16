using Structures.Enums;
using Structures.Map.Room;
using UnityEngine;

namespace Events.Room
{
    /// <summary>
    ///  Class responsible for relaying gate opening and closing events
    /// </summary>
    [CreateAssetMenu(fileName = "GateEvents", menuName = "GateEvents/GateEvents")]
    public class GateEvents : ScriptableObject
    {
        public System.Action<CordsXY, Direction, bool> ChangeChosenGateState;
    }
}
