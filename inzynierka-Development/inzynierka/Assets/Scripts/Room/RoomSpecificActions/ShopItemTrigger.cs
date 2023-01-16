using Events.UI;
using Room.RoomControlers;
using UnityEngine;

namespace Room.RoomSpecificActions
{
    /// <summary>
    ///  Class that implements popup logic
    /// </summary>
    public class ShopItemTrigger : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private ShopItemController ItemController { get; set; }
                
        [field: SerializeField]
        private ShopPopup PopupManagement { get; set; }

        #endregion

        /// <summary>
        /// method that shows shop item popup
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Player.Player>() == null)
            {
                return;
            }
            
            this.ItemController.playerEnteredZone?.Invoke(true);
            this.PopupManagement.PopPopup?.Invoke(this.ItemController.ItemType, this.ItemController.ItemCost, this.ItemController.ItemValue);
        }

        /// <summary>
        /// method that hides shop item popup
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<Player.Player>() == null)
            {
                return;
            }

            this.ItemController.playerEnteredZone?.Invoke(false);
            this.PopupManagement.TurnOffPopup?.Invoke();
        }

    }
}
