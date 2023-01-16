using Events.Player;
using UnityEngine;

namespace Room.RoomContent.Obsticle
{
    /// <summary>
    ///  Class that implements slow zone logic
    /// </summary>
    public class SlowZone : RoomObstacle
    {
        #region properties

        [field: SerializeField]
        private PlayerSpeedEvents SpeedEEffects { get; set; }

        #endregion
        #region Variabels

        private GameObject playerGameobject;

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

        #endregion
        #region methods

        /// <summary>
        /// methode that apples slow effect if player have had entered the zone 
        /// </summary>
        private void EnteredEntityCheck(Collider other)
        {
            if (other.GetComponent<Player.Player>() == null)
            {
                return;
            }

            playerGameobject = other.gameObject;
            SpeedEEffects.AddSpeedDown?.Invoke(true);
        }

        /// <summary>
        /// methode that removes slow effect if player have had exited the zone 
        /// </summary>
        private void ExitEntityCheck(Collider other)
        {
            if (other.gameObject != this.playerGameobject)
            {
                return;
            }

            SpeedEEffects.AddSpeedDown?.Invoke(false);
        }

        #endregion

    }
}
