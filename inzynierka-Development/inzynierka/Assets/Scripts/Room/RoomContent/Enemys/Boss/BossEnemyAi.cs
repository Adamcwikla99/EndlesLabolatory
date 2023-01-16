using Room.RoomContent.Enemys.Normal;
using UnityEngine;

namespace Room.RoomContent.Enemys.Boss
{
    /// <summary>
    ///  Class that implements boss enemy AI logic
    /// </summary>
    public class BossEnemyAi : EnemyAI
    {
        #region properties


        
        [field: SerializeField]
        private BossEnemyAttackAi EnemyAttackController { get; set; }
        

        
        [field: SerializeField]
        private float FireDistance { get; set; }
        
        [field: SerializeField]
        private float StopFollowDistance { get; set; }
        
        [field: SerializeField]
        private float movmentSpeed { get; set; }
        
        #endregion
        #region variables

        private float reloadCountdown;
        private bool lastSeenStatus = false;
        private bool newSeenStatus = false;
        
        #endregion
        #region methods

        /// <summary>
        /// methode that determins if boss can see player
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

            this.ToggleFire(hit, playerDirection);
        }
        
        /// <summary>
        /// methode that allows for performing fire action if conditions are meet 
        /// </summary>
        private void ToggleFire(RaycastHit hit, Vector3 playerDirection)
        {
            if (hit.collider.gameObject == this.Player)
            {
                this.EnemyAttackController.AllowFire?.Invoke(true);
                this.TryFollowPlayer(playerDirection);
                return;
            }

            this.EnemyAttackController.AllowFire?.Invoke(false);
        }

        /// <summary>
        /// methode that trys to move boss to player gameobject 
        /// </summary>
        private void TryFollowPlayer(Vector3 playerDirection)
        {
            if ((this.Player.transform.position - this.transform.position).magnitude < this.StopFollowDistance)
            {
                return;
            }

            this.FollowPlayer(playerDirection);
        }

        /// <summary>
        /// methode that makes boss follows player
        /// </summary>
        private void FollowPlayer(Vector3 playerDirection)
        {
            playerDirection.y = 0;
            this.gameObject.transform.position += playerDirection * this.movmentSpeed;
        }

        /// <summary>
        /// methode that sets reference to player gameobject
        /// </summary>
        protected override void SetFoundPlayer(GameObject player)
        {
            base.SetFoundPlayer(player);
            this.EnemyAttackController.SetPlayer(this.detectedPlayer);
        } 

        #endregion

    }
}
