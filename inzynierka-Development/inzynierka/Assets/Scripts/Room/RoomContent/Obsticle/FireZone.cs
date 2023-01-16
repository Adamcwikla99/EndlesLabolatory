using System.Collections;
using Interface;
using UnityEngine;

namespace Room.RoomContent.Obsticle
{
    /// <summary>
    ///  Class that implements fire zone logic
    /// </summary>
    public class FireZone : RoomObstacle
    {
        #region properties
        
        [field: SerializeField]
        private float ZoneDamage { get; set; }

        #endregion
        #region variables

        private GameObject playerGameobject;
        private IDamage playerDamageInterface;
        private bool playerInZone = false;
        private Coroutine damageCoroutine;
    
        #endregion
        #region unityCallbacks

        private void OnTriggerEnter(Collider other)
        {
            EnteredEntityCheck(other);
        }

        private void OnTriggerExit(Collider other)
        {
            ExitEntityCheck(other);
        }

        private void Update()
        {
            TryApplyPlayerDamage();
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that apples damage to player if player have had entered the zone 
        /// </summary>
        private void EnteredEntityCheck(Collider other)
        {
            if (other.GetComponent<Player.Player>() == null)
            {
                return;
            }

            this.playerGameobject = other.gameObject;
            this.playerDamageInterface = this.playerGameobject.GetComponent<IDamage>();
            this.playerInZone = true;
        }

        /// <summary>
        /// methode that stops applying damage to player if have left the zone 
        /// </summary>
        private void ExitEntityCheck(Collider other)
        {
            if (other.gameObject != this.playerGameobject)
            {
                return;
            }

            this.playerInZone = false;
        }

        /// <summary>
        /// methode that tries to apply damage to player
        /// </summary>
        private void TryApplyPlayerDamage()
        {
            if (this.playerInZone == false)
            {
                return;
            }

            this.damageCoroutine ??= this.StartCoroutine(this.DamageTick());
        }
        
        /// <summary>
        /// methode responsible for making damage tick 
        /// </summary>
        private IEnumerator DamageTick()
        {
            while (this.playerInZone == true)
            {
                this.playerDamageInterface.TakeDamage(this.ZoneDamage);
                yield return new WaitForSeconds(.1f);
            }
        }

        #endregion

    }
}
