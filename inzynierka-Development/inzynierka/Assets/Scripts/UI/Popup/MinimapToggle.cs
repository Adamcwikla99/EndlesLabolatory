using System;
using Events.Menu;
using Events.UI;
using UnityEngine;

namespace UI.Popup
{
    /// <summary>
    ///  Class that implements minimap toggle logic
    /// </summary>
    public class MinimapToggle : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private GameObject Minimap { get; set; }
        
        [field: SerializeField]
        private GameObject Pointer { get; set; }
        
        [field: SerializeField]
        private ToggleMinimapEvent Toggle { get; set; }
        
        [field: SerializeField]
        private TogglePlayerPointer PointerToggle { get; set; }

        #endregion

        #region variables

        private bool canToggleMinimap = true;

        #endregion
        
        #region unityCallbacks

        private void OnEnable()
        {
            EnableEvents();
        }

        private void OnDisable()
        {
            DisableEvents();
        }

        /// <summary>
        /// methode that assigns actions methods
        /// </summary>
        private void EnableEvents()
        {
            this.Toggle.ToggleMinimap += ShowMinimap;
            this.Toggle.ChangeMinimapState += SetMinimapState;
            PointerToggle.ToggleState += this.TogglePointer;
        }

        /// <summary>
        /// methode that unassigned actions methods
        /// </summary>
        private void DisableEvents()
        {
            this.Toggle.ToggleMinimap -= ShowMinimap;
            this.Toggle.ChangeMinimapState -= SetMinimapState;
            PointerToggle.ToggleState -= this.TogglePointer;
        }
        
        /// <summary>
        /// methode responsible for showing minimap
        /// </summary>
        private void ShowMinimap()
        {
            if (this.canToggleMinimap == false)
            {
                return;
            }
            
            this.Minimap.SetActive(!this.Minimap.active);
        }

        /// <summary>
        /// methode that allows to set minimap state
        /// </summary>
        private void SetMinimapState(bool newState)
        {
            this.Minimap.SetActive(newState);
            this.canToggleMinimap = !this.canToggleMinimap;
        }

        /// <summary>
        /// methode responsible for showing/hiding player pointer
        /// </summary>
        private void TogglePointer(bool newState)
        {
            this.Pointer.SetActive(newState);
        }
        
        #endregion

    }
}
