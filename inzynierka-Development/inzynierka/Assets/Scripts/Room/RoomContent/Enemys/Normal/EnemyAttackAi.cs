using Events.Projectile;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal
{
    /// <summary>
    ///  Abstract class that implements basic enemy attack AI universally used methods
    /// </summary>
    public abstract class EnemyAttackAi : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        public System.Action<bool> AllowFire { get; private set; }
    
        [field: SerializeField]
        protected ProjectileGetter GetProjectileEvents { get; set; }

        [field: SerializeField]
        protected float FireDeley { get; set; }
        
        [field: SerializeField]
        protected float Accuracy { get; set; }
    
        #endregion
        #region variables

        protected bool canFire = false;
        protected float currentReloadTime = 0f;
        protected bool isReoladed = true;
        protected GameObject playerObject;
        protected float bonusDamage = 0;
        protected float bonusSpeed = 0;
    
        #endregion
        #region unityCallbacks

        private void OnEnable()
        {
            this.AllowFire += this.ChangeCanFireState;
        }

        private void OnDisable()
        {
            this.AllowFire -= this.ChangeCanFireState;
        }

        private void Update()
        {
            this.UpdateActions();
        }

        #endregion
        #region methods

        /// <summary>
        /// method that assigns player game object reference to variable
        /// </summary>
        public void SetPlayer(GameObject playerObject) => this.playerObject = playerObject;

        /// <summary>
        /// methode that boosts up enemy entity stats 
        /// </summary>
        public void SetBonusStats(float bonusDamage, float bonusSpeed)
        {
            this.bonusDamage = bonusDamage;
            this.bonusSpeed = bonusSpeed;
        }
        
        /// <summary>
        /// methode that determines if enemy can fire at player 
        /// </summary>
        protected void ChangeCanFireState(bool newState)
        {
            this.canFire = newState;
        }

        /// <summary>
        /// fire cycle methode - fire at player if all conditions are met
        /// </summary>
        protected void UpdateActions()
        {
            if (this.canFire == false)
            {
                return;
            }

            this.UpdateReloadTime();
        
        }

        /// <summary>
        /// reload and fire cycle methode - fire bullet at player or reloads the weapon if it's not ready to fire
        /// </summary>
        protected void UpdateReloadTime()
        {
            if (this.isReoladed == true)
            {
                this.FireAtPlayer();
                return;
            }

            this.currentReloadTime += Time.deltaTime;
            if (this.currentReloadTime > this.FireDeley)
            {
                this.isReoladed = true;
                this.currentReloadTime = 0;
            }
        
        }

        /// <summary>
        /// method implementing fire at player logick
        /// </summary>
        protected abstract void FireAtPlayer();


        /// <summary>
        /// methode that determines accuracy error 
        /// </summary>
        protected Vector3 GetAccuracyError()
        {
            return new Vector3(Random.Range(-this.Accuracy, this.Accuracy),Random.Range(-this.Accuracy, this.Accuracy),Random.Range(-this.Accuracy, this.Accuracy));
        }
    
        #endregion
        
    }
}
