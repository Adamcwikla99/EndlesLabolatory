using Events.Floor;
using Events.Game;
using UnityEngine;

namespace Room.RoomSpecificActions
{
    /// <summary>
    ///  Class that implements floor changing logic
    /// </summary>
    public class EnterNextFloor : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private FloorEvents IndividualFloorEvents { get; set; }
        
        [field: SerializeField]
        private GameScore ManageGameScore { get; set; }

        #endregion
        #region unityCallbacks

        /// <summary>
        /// methode that generates next floor after entering portal generated after defeating the boss 
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Player.Player>() == null)
            {
                return;
            }
            
            this.ManageGameScore.IncreaseBeatenFloorsCount?.Invoke();
            this.IndividualFloorEvents.GenerateNewMap?.Invoke();
        }

        #endregion

    }
}
