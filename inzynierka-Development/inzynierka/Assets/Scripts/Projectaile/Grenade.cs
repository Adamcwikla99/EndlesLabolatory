using UnityEngine;

namespace Projectaile
{
    /// <summary>
    ///  Class implementing granade projectile logic
    /// </summary>
    public class Grenade : Projectile
    {
        #region properties

        [field: SerializeField]
        private GameObject ParticleSystem { get; set; }

        #endregion
        #region variables

        private System.Action explosionDelay;
        private System.Action explosion;

        #endregion
        #region unityCallbacks

        private void OnEnable()
        {
            EnableEvents();
        }

        private void OnDisable()
        {
            DisableEvents();
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that fires projectile
        /// </summary>
        public override void Fire(Quaternion rotation, Vector3 position, Vector3 velocity, float bonusDamage =0f , float bonusSpeed =0f)
        {
            this.gameObject.SetActive(true);
            ResetProjectile();
            SetProjectileParameters(rotation, position, velocity, bonusDamage, bonusSpeed);
            Invoke(explosionDelay.Method.Name, 5f);
        }
        
        /// <summary>
        /// methode that sets projectile parameters 
        /// </summary>
        protected override void SetProjectileParameters(Quaternion rotation, Vector3 position, Vector3 velocity, float bonusDamage =0f , float bonusSpeed =0f)
        {
            this.bonusDamage = bonusDamage;
            this.transform.rotation = rotation;
            this.transform.position = position + (velocity*PlacementOffset);
            this.ProjectileRigidbody.velocity = (velocity * (this.BaseSpeed * 50f));
        }
        
        /// <summary>
        /// methode that determines collision behaviour
        /// </summary>
        protected override void OnHit(Collision collision) { }
        
        /// <summary>
        /// methode implementing explosion logick
        /// </summary>
        private void Explode()
        {
            ParticleSystem.SetActive(true);
            Invoke(explosion.Method.Name, 0.5f);
        }

        /// <summary>
        /// methode that returns grande object to object pool
        /// </summary>
        private void ReturnActions()
        {
            ParticleSystem.SetActive(false);
            this.QueReturnEvents.ReturnToQue?.Invoke(this.thisProjectile, this.ThisType);
        }

        /// <summary>
        /// methode that enables events
        /// </summary>
        private void EnableEvents()
        {
            explosionDelay += Explode;
            explosion += ReturnActions;
        }

        /// <summary>
        /// methode that disables events
        /// </summary>
        private void DisableEvents()
        {
            explosionDelay -= Explode;
            explosion -= ReturnActions;
        }
        
        #endregion
    }
}
