using Events.Player;
using Room.RoomContent.Enemys.Normal.BaseEnemy;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.TuretEnemy
{
    /// <summary>
    ///  Class that implements turret enemy AI logic
    /// </summary>
    public class TurretEnemyAI : EnemyAI
    {
        #region properties

        [field: SerializeField]
        private TuretEnemyAttackAi EnemyAttackController { get; set; }
        
        [field: SerializeField]
        private float FireDistance { get; set; }
        
        #endregion
        #region variables

        private float reloadCountdown;
        private bool lastSeenStatus = false;
        private bool newSeenStatus = false;
        
        #endregion
        #region unityCallbacks


        #endregion
        #region methods
        
        /// <summary>
        /// methode that determins if entity can see player game object
        /// </summary>
        protected override void CanSeePlayer()
        {
            Vector3 playerDirection = this.transform.position;
            RaycastHit hit;
            playerDirection = this.Player.transform.position - this.transform.position;
            playerDirection = playerDirection.normalized;
            
            if (Physics.Raycast(this.transform.position, playerDirection, out hit, this.FireDistance, this.Mask) == false)
            {
                return;
            }

            ToggleFire(hit, playerDirection);
        }
        
        /// <summary>
        /// methode that allows for performing fire action
        /// </summary>
        private void ToggleFire(RaycastHit hit, Vector3 playerDirection)
        {
            if (hit.collider.gameObject == this.Player)
            {
                this.EnemyAttackController.AllowFire?.Invoke(true);
                return;
            }

            this.EnemyAttackController.AllowFire?.Invoke(false);
        }
        
        /// <summary>
        /// methode that sets reference to player game object
        /// </summary>
        protected override void SetFoundPlayer(GameObject player)
        {
            base.SetFoundPlayer(player);
            this.EnemyAttackController.SetPlayer(detectedPlayer);
        } 
        
        #endregion
    }
}
