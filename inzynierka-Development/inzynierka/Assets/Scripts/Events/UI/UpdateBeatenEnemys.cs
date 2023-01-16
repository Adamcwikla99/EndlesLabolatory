using UnityEngine;

namespace Events.UI
{
    /// <summary>
    ///  Class responsible for relaying update beaten enemy count events
    /// </summary>
    [CreateAssetMenu(fileName = "UpdateBeatenEnemys", menuName = "DisplayedUIStats/UpdateBeatenEnemys")]
    public class UpdateBeatenEnemys : ScriptableObject
    {
        public System.Action<int> RelayNewBeatenEnemysCount;
    }
}
