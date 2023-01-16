using Events.Projectile;
using UnityEngine;

namespace Room.RoomContent.Drop
{
    /// <summary>
    ///  Class implementing ammunition drop logic
    /// </summary>
    public class AmmunitionDrop : Drop
    {
        #region properties

        [field: SerializeField]
        private Structures.Enums.ProjectileType Type { get; set; }
        
        [field: SerializeField]
        private AmmunitionAdder AddAmmunitionEvent { get; set; }

        #endregion
        #region unityCallbacks

        private void Start()
        {
            this.thisDrop = this.gameObject.GetComponent<Drop>();
        }

        private void OnTriggerEnter(Collider other)
        {
            TryPickupDrop(other);
        }
        
        #endregion
        #region methodes

        /// <summary>
        /// methode that acknowledge ammunition being picked up by player
        /// </summary>
        private void TryPickupDrop(Collider other)
        {
            if (other.gameObject.GetComponent<Player.Player>() == null)
            {
                return;
            }
            
            this.PickUpSound.Play();
            AddAmmunitionEvent.AddAmmunition?.Invoke(this.Type, (int)this.DropValue);
            DropQueReturner.ReturnToDropQue?.Invoke(this.DropType, this.thisDrop);
            this.gameObject.SetActive(false);
        }

        #endregion
    }
}
