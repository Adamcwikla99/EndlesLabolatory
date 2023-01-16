using System.Collections.Generic;
using Room.RoomContent;
using UnityEngine;

namespace Events.Floor
{
    /// <summary>
    ///  Class responsible for relaying floor events
    /// </summary>
    [CreateAssetMenu(fileName = "FloorEvents", menuName = "FloorEvents/FloorEvents")]
    public class FloorEvents : ScriptableObject
    {
        public System.Action GenerateNewMap;
        public System.Action<List<FloorRoom>> ReleyGeneratedMap;
        
    }
}
