using UnityEngine;

namespace Events.UI
{
    /// <summary>
    ///  Class responsible for relaying modification of displayed player stats events
    /// </summary>
    [CreateAssetMenu(fileName = "DisplayedUIStats", menuName = "DisplayedUIStats/DisplayedUIStats")]
    public class DisplayedUIStats : ScriptableObject
    {
        public System.Action<int, int, int> InitialAmmunitionSetup;
        public System.Action<float, float, float> InitialDurabilitySetup;
        public System.Action<int> SetArrows;
        public System.Action<int> SetGranades;
        public System.Action<int> SetRockets;
        public System.Action<float> Hp;
        public System.Action<float> MaxHp;
        public System.Action<float> Money;
    }
}
