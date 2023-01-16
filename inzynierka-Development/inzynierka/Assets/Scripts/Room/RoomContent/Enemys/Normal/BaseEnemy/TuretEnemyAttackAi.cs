using Events.Sound;
using Projectaile;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.BaseEnemy
{
    /// <summary>
    ///  Class that implements turret enemy attack AI logic
    /// </summary>
    public class TuretEnemyAttackAi : EnemyAttackAi
    {
        #region properties

        [field: SerializeField]
        protected EnemyFired EnemyFireEffect { get; set; }

        #endregion
        #region variables



        #endregion
        #region unityCallbacks

        #endregion
        #region methods



        #endregion

        /// <summary>
        /// method responsible for initialization of fire at player action
        /// </summary>
        protected override void FireAtPlayer()
        {
            Projectile toFire = this.GetProjectileEvents.GetProjectile?.Invoke(Structures.Enums.ProjectileType.Enemy);
            this.isReoladed = false;
            this.transform.LookAt(this.playerObject.transform);
            toFire.Fire(this.transform.rotation, this.transform.position, this.transform.forward + this.GetAccuracyError(), this.bonusDamage, this.bonusSpeed);
            this.transform.Rotate(0,90f,0);
            this.EnemyFireEffect.PlayEnemyFireSound?.Invoke();
        }

    }
}
