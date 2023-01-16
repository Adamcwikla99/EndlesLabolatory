using Events.Drop;
using FMODUnity;
using Structures.Enums;
using UnityEngine;

namespace Room.RoomContent.Drop
{
    /// <summary>
    ///  Class implementing drop logic
    /// </summary>
    public abstract class Drop : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        public float DropValue { get; private set; } = 0;
        
        [field: SerializeField]
        public DropType DropType { get; private set; }

        [field: SerializeField]
        public bool AmmunitionDrop { get; private set; } = true;
        
        [field: SerializeField]
        protected ReturnDropToQue DropQueReturner { get; set; }
        
        [field: SerializeField]
        protected StudioEventEmitter PickUpSound { get; set; }
        
        #endregion
        #region Variables

        protected Drop thisDrop;

        #endregion
        #region methods

        /// <summary>
        /// methode that sets drop value 
        /// </summary>
        public void SetDropValue(float newValue)
        {
            this.DropValue = newValue;
        }

        /// <summary>
        /// methode that sets drop type
        /// </summary>
        public void SetDropType(bool isAmmunition)
        {
            this.AmmunitionDrop = isAmmunition;
        }

        /// <summary>
        /// methode that initializes drop values 
        /// </summary>
        public void InitDrop(Vector3 newPosition)
        {
            this.gameObject.SetActive(true);
            this.transform.position = newPosition;
        }
        
        #endregion

    }
}
