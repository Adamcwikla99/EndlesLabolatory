using Animations.Enemys.TurretEnemy;
using Room.RoomContent.Enemys.Normal.BaseEnemy;
using Shaders;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.TuretEnemy
{
    /// <summary>
    ///  Class that implements turret controller logic
    /// </summary>
    public class TurretController : RoomEntity
    {
        #region properties

        [field: SerializeField]
        private EnemyMaterialAdder BodyMaterial { get; set; }
        
        [field: SerializeField]
        private EnemyMaterialAdder CanonMaterial { get; set; }

        [field: SerializeField]
        private TurretEnemyAnimator BodyAnimator { get; set; }
        
        [field: SerializeField]
        private TurretEnemyAnimator CanonAnimator { get; set; }
        
        [field: SerializeField]
        private TuretEnemyAttackAi EntityAttackStats { get; set; }
        
        #endregion
        #region variables

        private const float animationTime = 3f;
        private float currentAnimationTime = 0f;
        private bool startCount = false;

        #endregion
        #region unityCallbacks

        new private void Start()
        {
            this.thisEntity = this.gameObject.GetComponent<TurretController>();
            EntityAttackStats.SetBonusStats(this.attackStats.projectilePower, this.attackStats.projectileSpeed);
        }

        private void Update()
        {
            CountTime();
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that applies damage to turret entity  
        /// </summary>
        public void BodyReceivedDamage(float damageValue)
        {
            TakeDamage(damageValue);
        }

        /// <summary>
        /// methode that adjusts material outline intensity
        /// </summary>
        protected override void AdjustOutline()
        {
            BodyMaterial.ChangeOutlineIntensity(CalculateNewOutlineIntensity());
            CanonMaterial.ChangeOutlineIntensity(CalculateNewOutlineIntensity());
        }
        
        /// <summary>
        /// methode that plays desolve animation
        /// </summary>
        public override void PlayDesolveAnimation()
        {
            BodyAnimator.PlayAnimation();
            CanonAnimator.PlayAnimation();
            startCount = true;
        }

        /// <summary>
        /// methode that starts desolve animation countdown
        /// </summary>
        private void CountTime()
        {
            if (startCount == false)
            {
                return;
            }

            ProcessAnimationTime();
        }

        /// <summary>
        /// methode that checks if animation should be stoped
        /// </summary>
        private void ProcessAnimationTime()
        {
            this.currentAnimationTime += Time.deltaTime;
            if (this.currentAnimationTime > animationTime)
            {
                Destroy(this.gameObject);                
            }
        }
        
        #endregion
    }
}
