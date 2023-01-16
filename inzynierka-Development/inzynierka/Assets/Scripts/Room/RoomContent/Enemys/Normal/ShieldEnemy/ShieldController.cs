using Animations.Enemys.ShieldEnemy;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.ShieldEnemy
{
    /// <summary>
    ///  Class that implements shield controller logic
    /// </summary>
    public class ShieldController : RoomEntity
    {
        #region properties

        [field: SerializeField]
        private ShieldEnemyAnimator DesolveAnimation { get; set; }

        #endregion
        #region variables

        private const float animationTime = 3f;
        private float currentAnimationTime = 0f;
        private bool startCount = false;

        #endregion
        #region unityCallbacks

        new private void Start()
        {
            this.thisEntity = this.gameObject.GetComponent<ShieldController>();
        }

        private void Update()
        {
            CountTime();
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that applies damage to shield entity  
        /// </summary>
        public void BodyReceivedDamage(float damageValue)
        {
            this.TakeDamage(damageValue);
        }

        /// <summary>
        /// methode that determines if shield entity can deflect projectiles   
        /// </summary>
        public bool DeflectProjectile() => false;
        
        /// <summary>
        /// methode that plays desolve animation
        /// </summary>
        public override void PlayDesolveAnimation()
        {
            DesolveAnimation.PlayAnimation();
            startCount = true;
        }
        
        /// <summary>
        /// methode that starts desolve animation countdown and destroys game object after it was finished
        /// </summary>
        private void CountTime()
        {
            if (startCount == false)
            {
                return;
            }
            
            this.currentAnimationTime += Time.deltaTime;
            if (this.currentAnimationTime > animationTime)
            {
                Destroy(this.gameObject);                
            }
        }
        
        #endregion
    }
}
