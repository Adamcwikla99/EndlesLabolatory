using System;
using Events.Projectile;
using Structures.Enums;
using UnityEngine;

namespace Room.RoomSpecificActions
{
    /// <summary>
    ///  Class that implements item manager logic
    /// </summary>
    public class ItemManager : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private GameObject ReloadItem { get; set; }

        [field: SerializeField]
        private GameObject DamageItem { get; set; }

        [field: SerializeField]
        private GameObject SpeedItem { get; set; }
    
        [field: SerializeField]
        private BulletsStatsEvents BulletStats { get; set; }
    
        #endregion

        #region variables

        public System.Action<BulletStatType, float> pickupItem;

        #endregion

        #region unityCallbacks

        private void OnEnable()
        {
            this.pickupItem += this.PlayerPickupItem;
        }

        private void OnDisable()
        {
            this.pickupItem -= this.PlayerPickupItem;
        }

        #endregion

        #region methods

        /// <summary>
        /// method that triggers adding bullet stats 
        /// </summary>
        private void PlayerPickupItem(BulletStatType type, float value)
        {
            this.AddStat(type, value);
            this.RemoveItems();
        }

        /// <summary>
        /// methode that adds stat according to picked up item  
        /// </summary>
        private void AddStat(BulletStatType type, float value)
        {
            switch (type)
            {
                case BulletStatType.Damage :
                    this.BulletStats.IncreaseDamage?.Invoke(value);    
                    break;
                case BulletStatType.Speed :
                    this.BulletStats.IncreaseSpeed?.Invoke(value);  
                    break;
                case BulletStatType.Reload :
                    this.BulletStats.DecreaseReload?.Invoke(value);  
                    break;
                default :
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void RemoveItems()
        {
            Destroy(this.ReloadItem);
            Destroy(this.DamageItem);
            Destroy(this.SpeedItem);
        }

        #endregion



    }
}
