using Events.Room;
using Structures.Map.Room;
using UnityEngine;

namespace Room.Tunnel
{
    /// <summary>
    ///  Class that implements tunnel logic
    /// </summary>
    public class TunnelController : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        public CordsXY AlignedRoom { get; set; }
        
        [field: SerializeField]
        public CordsXY TunnelRoom { get; set; }
        
        [field: SerializeField]
        public System.Action PlayerEneredCollider { get; set; }

        [field: SerializeField]
        private Transform ForceFieldStartPosition { get; set; }
        
        [field: SerializeField]
        private Transform ForceFieldEndPosition { get; set; }
        
        [field: SerializeField]
        private TunnelEvents RoomTunnelEvents { get; set; }
        
        #endregion
        #region variables

    

        #endregion
        #region unityCallbacks


        #endregion
        #region methods

        /// <summary>
        /// methode that sets tunnel aligned rooms
        /// </summary>
        public void SetAlignedRoom(CordsXY alignedRoom) => this.AlignedRoom = alignedRoom;

        /// <summary>
        /// methode that sets tunnel aligned tunnels
        /// </summary>
        public void SetTunnelRoom(CordsXY tunnelRoom) => this.TunnelRoom = tunnelRoom;
        
        /// <summary>
        /// methode that initializes room awakening process
        /// </summary>
        public void AwakeRoomEntitys()
        {
            this.RoomTunnelEvents.AwakeRoomEntity?.Invoke(this.TunnelRoom);
        }
        
        #endregion

    }
}
