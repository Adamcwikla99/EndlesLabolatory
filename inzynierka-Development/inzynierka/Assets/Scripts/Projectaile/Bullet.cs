using Interface;
using UnityEngine;

namespace Projectaile
{
    /// <summary>
    ///  Class implementing bullet projectile logic
    /// </summary>
    public class Bullet : Projectile
    {
        #region methods

        /// <summary>
        /// methode that fires projectile
        /// </summary>
        public override void Fire(Quaternion rotation, Vector3 position, Vector3 velocity, float bonusDamage =0f , float bonusSpeed =0f)
        {
            ResetProjectile();
            SetProjectileParameters(rotation, position, velocity, bonusDamage, bonusSpeed);
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// methode that determines collision behaviour
        /// </summary>
        protected override void OnHit(Collision collision)
        {
            DamageInterfaceCheck(collision);
        }
        
        #endregion
        
    }
}
