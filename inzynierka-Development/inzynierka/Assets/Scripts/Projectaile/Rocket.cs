using Interface;
using UnityEngine;

namespace Projectaile
{
    /// <summary>
    ///  Class implementing rocket projectile logic
    /// </summary>
    public class Rocket : Projectile
    {
        #region properties

        [field: SerializeField]
        private ParticleSystem SmokeParticles { get; set; }
        
        [field: SerializeField]
        private ParticleSystem FireParticles { get; set; }
        
        [field: SerializeField]
        private GameObject ExplosionParticles { get; set; }

        [field: SerializeField]
        private GameObject ExplosionParticleSystem { get; set; }

        #endregion
        #region variables

        private System.Action returnAfterDelay;

        #endregion
        #region unityCallbacks

        private void OnEnable()
        {
            EnableEvents();
        }

        private void OnDisable()
        {
            this.DisableEvents();
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that fires projectile
        /// </summary>
        public override void Fire(Quaternion rotation, Vector3 position, Vector3 velocity, float bonusDamage  =0f , float bonusSpeed =0f)
        {
            ResetProjectile();
            ActivateParticles();
            Vector3 a = rotation.eulerAngles + new Vector3(0, 270, 0);
            Quaternion newQuaterion = Quaternion.Euler(a.x,a.y,a.z);
            SetProjectileParameters(newQuaterion, position, velocity, bonusDamage, bonusSpeed);
            this.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// methode that determines collision behaviour
        /// </summary>
        protected override void OnHit(Collision collision)
        {
            RocketHitCheck(collision);
        }
        
        /// <summary>
        /// methode that activate particles emission
        /// </summary>
        private void ActivateParticles()
        {
            SmokeParticles.Play();
            FireParticles.Play();
        }
        
        /// <summary>
        /// methode that deactivate particles emission
        /// </summary>
        private void DeactivateParticles()
        {
            SmokeParticles.Stop();
            FireParticles.Stop();
            ExplosionParticles.SetActive(false);
        }

        /// <summary>
        /// methode that checks if rocket hit any obect
        /// </summary>
        private void RocketHitCheck(Collision collision)
        {
            IDamage objectInterface = collision.gameObject.GetComponent<IDamage>();
            if (objectInterface == null)
            {
                Instantiate(ExplosionParticleSystem.transform, this.gameObject.transform.position, this.gameObject.transform.rotation);
                ReturnRocket();
                return;
            }

            if (objectInterface.DeflectProjectile() == true)
            {
                return;
            }
            
            Instantiate(ExplosionParticleSystem.transform, this.gameObject.transform.position, this.gameObject.transform.rotation);
            objectInterface.TakeDamage(this.BaseDamage + this.bonusDamage);
            ReturnRocket();
        }

        /// <summary>
        /// methode that returns rocket object to object pool
        /// </summary>
        private void ReturnRocket()
        {
            DeactivateParticles();
            this.QueReturnEvents.ReturnToQue?.Invoke(this.thisProjectile, this.ThisType);
        }

        /// <summary>
        /// methode that enables events
        /// </summary>
        private void EnableEvents()
        {
            returnAfterDelay += ReturnRocket;
        }

        /// <summary>
        /// methode that disables events
        /// </summary>
        private void DisableEvents()
        {
            returnAfterDelay -= ReturnRocket;
        }
        
        #endregion
        
    }
}
