using Events.Player;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal
{
    /// <summary>
    /// Abstract class that implements basic enemy AI universally used methods
    /// </summary>
    public abstract class EnemyAI : MonoBehaviour
    {
        #region properties
        
        [field: SerializeField]
        public System.Action<GameObject> FoundPlayer { get; private set; }

        [field: SerializeField]
        protected LayerMask Mask { get; set; }

        [field: SerializeField]
        protected PlayerObjectGetter PlayerObject { get; set; }
        
        [field: SerializeField]
        protected GameObject Player { get; set; }
        
        #endregion
        #region variables

        protected bool wasPlayerDetected = false;
        protected GameObject detectedPlayer;

        #endregion
        #region unityCallbacks

        private void Awake()
        {
            this.Player = this.PlayerObject.GetPlayerObject?.Invoke();
            SetFoundPlayer(this.Player);
        }

        private void Update()
        {
            CanSeePlayer();
        }
        
        private void OnEnable()
        {
            EnableEvents();
        }

        private void OnDisable()
        {
            DisableEvents();
        }

        #endregion
        #region methods

        protected abstract void CanSeePlayer();

        protected virtual void EnableEvents()
        {
            this.FoundPlayer += SetFoundPlayer;
        }

        protected virtual void DisableEvents()
        {
            this.FoundPlayer -= SetFoundPlayer;
        }
        
        protected virtual void SetFoundPlayer(GameObject player)
        {
            this.wasPlayerDetected = true;
            this.detectedPlayer = this.Player;

        }

        #endregion

    }
}
