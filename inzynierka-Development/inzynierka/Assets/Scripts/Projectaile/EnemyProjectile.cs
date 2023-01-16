using Interface;
using UnityEngine;

namespace Projectaile
{
    /// <summary>
    ///  Class implementing enemy projectile projectile logic
    /// </summary>
    public class EnemyProjectile : Projectile
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
            IDamage objectInterface = collision.gameObject.GetComponent<IDamage>();
            objectInterface?.TakeDamage(this.BaseDamage + this.bonusDamage);

            this.QueReturnEvents.ReturnToQue?.Invoke(thisProjectile, this.ThisType);
        }

        /// <summary>
        /// methode that sets projectile parameters 
        /// </summary>
        protected override void SetProjectileParameters(Quaternion rotation, Vector3 position, Vector3 velocity, float bonusDamage =0f , float bonusSpeed =0f)
        {
            this.bonusDamage = bonusDamage;
            this.transform.rotation = rotation;
            this.transform.position = position + (velocity);
            this.ProjectileRigidbody.velocity = (velocity * (this.BaseSpeed * bonusSpeed));
        }
        
        #endregion
    }
}
