using UnityEngine;

namespace Events.Menu
{
    /// <summary>
    ///  Class responsible for relaying minimap manipulation events events
    /// </summary>
    [CreateAssetMenu(fileName = "ToggleMinimapEvent", menuName = "ToggleMinimapEvent/ToggleMinimapEvent")]
    public class ToggleMinimapEvent : ScriptableObject
    {
        public System.Action ToggleMinimap;
        public System.Action<bool> ChangeMinimapState;
    }
}
