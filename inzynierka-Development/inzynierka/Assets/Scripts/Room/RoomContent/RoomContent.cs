using Interface;
using Room.RoomControlers;
using Structures;
using Structures.Wrapper;
using UnityEngine;

namespace Room.RoomContent
{
    /// <summary>
    /// Data wraper for room content stats - its health and if it can be destoryes
    /// </summary>
    public class RoomContent : MonoBehaviour
    {
        #region properties

        
        #endregion
        #region variables

        public ContentDurabilityStats durabilityStats;
        public EnemyRoomController parentRoom;
        
        #endregion
        #region unityCallbacks

        protected void Start()
        {
            Init();
        }

        #endregion
        #region methods

        
        protected virtual void Init()
        {
            this.durabilityStats.maxHealth = 50f;
            this.durabilityStats.currentHealth = 50f;
            this.durabilityStats.canBeDestroyed = true;
        }

        #endregion

    }
}
