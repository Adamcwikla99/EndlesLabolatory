using System;
using Events.Player;
using Events.Projectile;
using Events.Room;
using Events.UI;
using FMODUnity;
using Structures.Enums;
using UnityEngine;

namespace Room.RoomControlers
{
    /// <summary>
    ///  Class that implements shop item logic
    /// </summary>
    public class ShopItemController : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        public ShopItemType ItemType { get; private set; }
        
        [field: SerializeField]
        public float ItemCost { get; private set; } = 0;
        
        [field: SerializeField]
        public float ItemValue { get; private set; } = 0;
        
        [field: SerializeField]
        private PlayerStatsEvents StatsEvents { get; set; }
        
        [field: SerializeField]
        private AmmunitionAdder AmmunitionEvents { get; set; }
        
        [field: SerializeField]
        private BuyItemEvent BuyItem { get; set; }

        [field: SerializeField]
        private DisplayedUIStats StatsChangeEvents { get; set; }
        
        [field: SerializeField]
        private StudioEventEmitter BuySound { get; set; }
        
        [field: SerializeField]
        private ShopPopup PopupManagement { get; set; }
        
        #endregion

        #region variables

        public System.Action<bool> playerEnteredZone;
        private ShopZoneBuy buyAction = null;
        private ShopZoneBuy actionMethode = null;
        
        #endregion

        private delegate void ShopZoneBuy();
        
        #region unityCallbacks

        private void OnEnable()
        {
            this.playerEnteredZone += this.ShopZoneInteraction;
            this.BuyItem.TryBuyItem += this.TryBuy;
        }

        private void OnDisable()
        {
            this.playerEnteredZone -= this.ShopZoneInteraction;
            this.BuyItem.TryBuyItem -= this.TryBuy;
        }

        private void Start()
        {
            this.buyAction = this.PlayerNotInZone;
        }

        #endregion
        
        #region methods

        /// <summary>
        /// method that initializes buy action
        /// </summary>
        private void TryBuy()
        {
            this.buyAction();
        }
        
        /// <summary>
        /// method that conducts buy process if player if in buy zone 
        /// </summary>
        private void ShopZoneInteraction(bool newState)
        {
            if (newState == true)
            {
                this.buyAction = this.PlayerInZone;
                return;
            }
            
            this.buyAction = this.PlayerNotInZone;
        }

        /// <summary>
        /// methode that acknowledge player being in item buy zone 
        /// </summary>
        private void PlayerInZone()
        {
            if (this.StatsEvents.GetStat?.Invoke(PlayerStats.Money) < this.ItemCost)
            {
                return;
            }

            this.DecideItemAction();
        }
        
        /// <summary>
        /// methode that calls action appropriate action to shop item
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void DecideItemAction()
        {
            this.BuySound.Play();
            this.StatsEvents.AddStat?.Invoke(PlayerStats.Money, -this.ItemCost);
            switch (this.ItemType)
            {
                case ShopItemType.BuyHp :
                    this.StatsEvents.AddStat?.Invoke(PlayerStats.Health, this.ItemValue);
                    break;
                case ShopItemType.BuyMaxHp :
                    this.StatsEvents.AddStat?.Invoke(PlayerStats.MaxHealth, this.ItemValue);
                    break;
                case ShopItemType.BuyArrows :
                    this.AmmunitionEvents.AddAmmunition?.Invoke(ProjectileType.Arrow, (int)this.ItemValue);
                    break;
                case ShopItemType.BuyGrenades :
                    this.AmmunitionEvents.AddAmmunition?.Invoke(ProjectileType.Granade, (int)this.ItemValue);
                    break;
                case ShopItemType.BuyRockets :
                    this.AmmunitionEvents.AddAmmunition?.Invoke(ProjectileType.Rocket, (int)this.ItemValue);
                    break;
                default :
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// action when player isn't in zone
        /// </summary>
        private void PlayerNotInZone() { }
        
        #endregion



    }
}
