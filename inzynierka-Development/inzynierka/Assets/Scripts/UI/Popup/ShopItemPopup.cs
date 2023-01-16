using Events.UI;
using TMPro;
using UnityEngine;

namespace UI.Popup
{
    /// <summary>
    ///  class responsible for setting shop item value and price 
    /// </summary>
    public class ShopItemPopup : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private TMP_Text Item { get; set; }
        
        [field: SerializeField]
        private TMP_Text Price { get; set; }

        [field: SerializeField]
        private TMP_Text Value { get; set; }
        
        [field: SerializeField]
        private TogglePlayerPointer PointerToggle { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// methode responsible for setting displayed shop item information
        /// </summary>
        public void SetNewValues(string itemText, string priceText, string valueText)
        {
            this.Item.text = itemText;
            this.Price.text = priceText;
            this.Value.text = valueText;
        }

        /// <summary>
        /// methode that hides player pointer
        /// </summary>
        private void OnEnable()
        {
            PointerToggle.ToggleState?.Invoke(false);
        }

        /// <summary>
        /// methode that shows player pointer
        /// </summary>
        private void OnDisable()
        {
            PointerToggle.ToggleState?.Invoke(true);
        }

        #endregion



    }
}
