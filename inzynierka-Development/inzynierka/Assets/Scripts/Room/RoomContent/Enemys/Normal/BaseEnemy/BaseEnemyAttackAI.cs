using Projectaile;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.BaseEnemy
{
    /// <summary>
    ///  Class that implements base enemy attack ai logic
    /// </summary>
    public class BaseEnemyAttackAI : EnemyAttackAi
    {
        #region properties

        [field: SerializeField]
        protected BaseEnemy ThisEnemyObject { get; set; }

        #endregion

        private void Start()
        {
            this.ThisEnemyObject = this.gameObject.GetComponent<BaseEnemy>();
        }

        /// <summary>
        /// methode responsible for performing fire at player action
        /// </summary>
        protected override void FireAtPlayer()
        {
            Projectile toFire = this.GetProjectileEvents.GetProjectile?.Invoke(Structures.Enums.ProjectileType.Enemy);
            this.isReoladed = false;
            this.transform.LookAt(this.playerObject.transform);
            toFire.Fire(this.transform.rotation, this.transform.position, this.transform.forward + this.GetAccuracyError(), this.bonusDamage, this.bonusSpeed);
            this.ThisEnemyObject?.PlayFireSound();
        }
    }
}
