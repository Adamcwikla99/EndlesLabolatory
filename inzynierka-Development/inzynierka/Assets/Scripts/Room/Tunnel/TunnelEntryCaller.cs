using UnityEngine;

namespace Room.Tunnel
{
    /// <summary>
    ///  Class that implements tunnel movement logic
    /// </summary>
    public class TunnelEntryCaller : MonoBehaviour
    {
        #region properties

        [field: SerializeField] 
        private TunnelController ThisTunnelController { get; set; }
        
        #endregion
        #region variables

    

        #endregion
        #region unityCallbacks

        /// <summary>
        /// methode that triggers awakening of room entitys  
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            this.ThisTunnelController.AwakeRoomEntitys();
        }

        #endregion
        #region methods

    

        #endregion

    }
}
