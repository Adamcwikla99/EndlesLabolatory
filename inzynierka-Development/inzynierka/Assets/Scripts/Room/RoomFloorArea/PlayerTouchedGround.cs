using Events.Player;
using UnityEngine;

namespace Room.RoomFloorArea
{
    /// <summary>
    ///  Class that implements logic that check if player touched ground
    /// </summary>
    public class PlayerTouchedGround : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private PlayerMovementEvents MovementEvents { get; set; }

        #endregion
        #region variables

    

        #endregion
        #region unityCallbacks

        /// <summary>
        /// methode that calls jump reanables action 
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            this.MovementEvents.PlayerCanJump?.Invoke(collision.gameObject);

        }

        #endregion

    }
}
