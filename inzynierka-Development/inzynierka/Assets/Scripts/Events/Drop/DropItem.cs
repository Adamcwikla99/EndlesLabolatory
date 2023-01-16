using UnityEngine;

namespace Events.Drop
{
    /// <summary>
    ///  Class responsible for relaying drop item events
    /// </summary>
    [CreateAssetMenu(fileName = "DropItem", menuName = "DropItem/DropItem")]
    public class DropItem: ScriptableObject
    {
        public System.Action<Vector3> DropLoot;

    }
}
