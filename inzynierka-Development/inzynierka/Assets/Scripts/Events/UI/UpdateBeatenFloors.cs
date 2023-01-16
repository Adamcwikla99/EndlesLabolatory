using UnityEngine;

namespace Events.UI
{
    /// <summary>
    ///  Class responsible for relaying update beaten floors count events
    /// </summary>
    [CreateAssetMenu(fileName = "UpdateBeatenFloors", menuName = "DisplayedUIStats/UpdateBeatenFloors")]
    public class UpdateBeatenFloors : ScriptableObject
    {
        public System.Action<int> RelayNewBeatenFloorsCount;
    }
}
