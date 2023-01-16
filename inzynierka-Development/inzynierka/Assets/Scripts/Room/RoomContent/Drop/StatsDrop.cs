using System;
using Events.Player;
using Events.Projectile;
using Structures.Enums;
using UnityEngine;

namespace Room.RoomContent.Drop
{
    /// <summary>
    ///  Class implementing stats drop logic
    /// </summary>
    public class StatsDrop : Drop
    {
        #region properties

        [field: SerializeField]
        private PlayerStats PlayerStatsType { get; set; }
        
        [field: SerializeField]
        private PlayerStatsEvents PlayerStats { get; set; }
        
        #endregion

        #region variables

    

        #endregion

        #region unityCallbacks
        
        private void Start()
        {
            this.thisDrop = this.gameObject.GetComponent<Drop>();
        }

        /// <summary>
        /// methode that acknowledges drop being picked up by player
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Player.Player>() == null)
            {
                return;
            }
            
            this.PickUpSound.Play();
            this.PlayerStats.AddStat?.Invoke(this.PlayerStatsType, this.DropValue);            
            this.DropQueReturner.ReturnToDropQue?.Invoke(this.DropType, this.thisDrop);
            this.PickUpSound.Play();
            this.gameObject.SetActive(false);
        }

        #endregion

        #region methods

        

        #endregion

    }
}
