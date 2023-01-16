using UnityEngine;

namespace Events.Room
{
    /// <summary>
    ///  Class responsible for relaying item buy events
    /// </summary>
    [CreateAssetMenu(fileName = "BuyItemEvent", menuName = "RoomEvents/BuyItemEvent")]
    public class BuyItemEvent : ScriptableObject
    {
        public System.Action TryBuyItem;
    }
}
