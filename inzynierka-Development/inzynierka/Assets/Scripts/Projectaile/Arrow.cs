using UnityEngine;

namespace Projectaile
{
    /// <summary>
    ///  Class implementing arrow projectile logic
    /// </summary>
    public class Arrow : Projectile
    {
        #region methods

        /// <summary>
        /// methode that fires projectile
        /// </summary>
        public override void Fire(Quaternion rotation, Vector3 position, Vector3 velocity, float bonusDamage =0f , float bonusSpeed =0f)
        {
            ResetProjectile();
            Vector3 a = rotation.eulerAngles + new Vector3(0, 90, 0);
            Quaternion newQuaterion = Quaternion.Euler(a.x,a.y,a.z);
            
            SetProjectileParameters(newQuaterion, position, velocity, bonusDamage, bonusSpeed);
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
