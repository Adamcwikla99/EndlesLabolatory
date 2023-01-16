using Events.Player;
using Events.Projectile;
using Room.RoomContent;
using Room.RoomContent.Enemys.Normal;
using Room.RoomContent.Enemys.Normal.BaseEnemy;
using UnityEngine;

namespace Animations.Enemys.BaseEnemy
{
    /// <summary>
    /// Class implementing base enemy AI
    /// </summary>
    public class BaseEnemyAI : EnemyAI
    {
        #region properties

        [field: SerializeField]
        private BaseEnemyAttackAI EnemyAttackController { get; set; }
        
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
        /// method that check if enemy can see player
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
        /// methode that enable enemy fire action 
        /// </summary>
        /// <param name="hit"> ray that hit player </param>
        /// <param name="playerDirection"> player direction </param>
        private void ToggleFire(RaycastHit hit, Vector3 playerDirection)
        {
            if (hit.collider.gameObject == this.Player)
            {
                this.EnemyAttackController.AllowFire?.Invoke(true);
                TryFollowPlayer(playerDirection);
                return;
            }

            this.EnemyAttackController.AllowFire?.Invoke(false);
        }

        /// <summary>
        /// methode that trys to move enemy to player
        /// </summary>
        /// <param name="playerDirection"> player directione</param>
        private void TryFollowPlayer(Vector3 playerDirection)
        {
            if ((this.Player.transform.position - this.transform.position).magnitude < this.StopFollowDistance)
            {
                return;
            }

            FollowPlayer(playerDirection);
        }

        /// <summary>
        ///  methode that moves enemy to player if conditions are met
        /// </summary>
        /// <param name="playerDirection"> player directione</param>
        private void FollowPlayer(Vector3 playerDirection)
        {
            playerDirection.y = 0;
            this.gameObject.transform.position += playerDirection * this.movmentSpeed;
        }

        /// <summary>
        /// methode that sets found player
        /// </summary>
        /// <param name="player">game player object</param>
        protected override void SetFoundPlayer(GameObject player)
        {
            base.SetFoundPlayer(player);
            this.EnemyAttackController.SetPlayer(detectedPlayer);
        } 

        #endregion
    }
}
