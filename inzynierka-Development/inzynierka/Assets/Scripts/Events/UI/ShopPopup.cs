using Structures.Enums;
using UnityEngine;

namespace Events.UI
{
    /// <summary>
    ///  Class responsible for relaying showing and hiding events
    /// </summary>
    [CreateAssetMenu(fileName = "ShopPopup", menuName = "Popup/ShopPopup")]
    public class ShopPopup : ScriptableObject
    {
        public System.Action<ShopItemType, float, float> PopPopup;
        public System.Action TurnOffPopup;
    }
}
