using Animations.Enemys.BaseEnemy;
using UnityEngine;

namespace Room.RoomContent.Enemys
{
    /// <summary>
    ///  Class that implements player detector logic
    /// </summary>
    public class PlayerDetector : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private BaseEnemyAI EnemyAI { get; set; }

        [field: SerializeField]
        private bool AlreadyDetectedPlayer { get; set; } = false;
    
        #endregion
        #region unityCallbacks

        private void OnTriggerEnter(Collider other)
        {
            this.CheckIfDetectedPlayer(other.gameObject);
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that check if gameobject entering the detection sphere is a player
        /// </summary>
        /// <param name="potentialPlayer"></param>
        private void CheckIfDetectedPlayer(GameObject potentialPlayer)
        {
            if (this.AlreadyDetectedPlayer == false && potentialPlayer.GetComponent<Player.Player>() == null)
            {
                return;
            }

            this.AlreadyDetectedPlayer = true;
            this.EnemyAI.FoundPlayer?.Invoke(potentialPlayer);

        }

        #endregion

    }
}
