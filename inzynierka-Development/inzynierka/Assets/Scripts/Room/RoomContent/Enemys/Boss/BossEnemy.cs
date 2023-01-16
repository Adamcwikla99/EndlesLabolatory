using Room.RoomControlers;
using Structures;
using Structures.Wrapper;
using UnityEngine;

namespace Room.RoomContent.Enemys.Boss
{
    /// <summary>
    ///  Class that implements boss enemy logic
    /// </summary>
    public class BossEnemy : RoomEntity
    {
        #region properties

        [field: SerializeField]
        private BossRoomController ThisBossRoomController { get; set; }
        
        [field: SerializeField]
        private BossEnemyAttackAi EntityAttackStats { get; set; }

        #endregion
        #region variables

    

        #endregion
        #region unityCallbacks

        new private void Start()
        {
            this.thisEntity = this.gameObject.GetComponent<BossEnemy>();
            EntityBoost value = this.ManageEnemyFloorBoost.GetEntityPowerUp?.Invoke();
            PowerUp(value);
            EntityAttackStats.SetBonusStats(this.attackStats.projectilePower, this.attackStats.projectileSpeed);
        }
        
        #endregion
        #region methods

        /// <summary>
        /// methode that assignes reference to boss room controler that boss enemy is located in 
        /// </summary>
        public void SetBossRoomController(BossRoomController controller)
        {
            ThisBossRoomController = controller;
        }
        
        /// <summary>
        /// methode that applies damage to boss entity  
        /// </summary>
        public override void TakeDamage(float damageValue)
        {
            if (this.durabilityStats.canBeDestroyed == false || this.DeflectProjectile() == true)
            {
                return;
            }

            BossSurviveCheck(damageValue);
            AdjustOutline();
        }
        
        /// <summary>
        /// methode that boss up boss entity
        /// </summary>
        private void PowerUp(EntityBoost boostValue)
        {
            this.durabilityStats.maxHealth += boostValue.BonusMaxHP;
            this.durabilityStats.currentHealth = this.durabilityStats.maxHealth;
            this.attackStats.projectilePower += boostValue.BonusProjectilePower;
            this.attackStats.projectileSpeed += boostValue.BonusProjectileSpeed;
        }
        
        /// <summary>
        /// methode that plays firing sound
        /// </summary>
        public void PlayFireSound()
        {
            this.EnemyFireEffect.PlayEnemyFireSound?.Invoke();
        }
        
        /// <summary>
        /// methode that check if boss survived taking damage 
        /// </summary>
        private void BossSurviveCheck(float damageValue)
        {
            if (this.durabilityStats.currentHealth <0)
            {
                return;
            }
			
            this.durabilityStats.currentHealth -= damageValue;
            if (this.durabilityStats.currentHealth <= 0f)
            {
                this.ThisBossRoomController.BossDefeat();
                Destroy(this.gameObject);
            }
        }

        #endregion
    }
}
