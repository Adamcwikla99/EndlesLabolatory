using Projectaile;
using Room.RoomContent.Enemys.Normal;
using UnityEngine;

namespace Room.RoomContent.Enemys.Boss
{
    /// <summary>
    ///  Class that implements boss enemy attack AI logic
    /// </summary>
    public class BossEnemyAttackAi : EnemyAttackAi
    {
        #region properties

        [field: SerializeField]
        protected BossEnemy ThisEnemyObject { get; set; }

        [field: SerializeField]
        protected float TestValue { get; set; } = 1;

        #endregion

        private void Start()
        {
            ThisEnemyObject = this.gameObject.GetComponent<BossEnemy>();
        }

        /// <summary>
        /// methode that implements fire at player boss action
        /// </summary>
        protected override void FireAtPlayer()
        {
            for (int i=0; i<10;i++)
            {
                Projectile toFire = this.GetProjectileEvents.GetProjectile?.Invoke(Structures.Enums.ProjectileType.Enemy);
                this.transform.LookAt(this.playerObject.transform);
                toFire.Fire(this.transform.rotation, this.transform.position + this.transform.forward * TestValue + this.GetAccuracyError(), this.transform.forward, this.bonusDamage, this.bonusSpeed);
                this.ThisEnemyObject?.PlayFireSound();
            }

            this.isReoladed = false;
        }

    }
}
