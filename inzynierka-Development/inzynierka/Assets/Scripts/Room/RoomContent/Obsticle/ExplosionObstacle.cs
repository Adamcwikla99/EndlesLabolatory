using Interface;
using UnityEngine;

namespace Room.RoomContent.Obsticle
{
    /// <summary>
    ///  Class that implements explosion obstacle logic
    /// </summary>
    public class ExplosionObstacle : RoomObstacle, IDamage
    {
        #region properties

        [field: SerializeField]
        private ParticleSystem Particles { get; set; }

        [field: SerializeField]
        private float ParticleDamageToObstacle { get; set; } = 10f;

        #endregion
        #region variables

    

        #endregion
        #region unityCallbacks

        private void Awake()
        {
            Particles.Stop();
        }

        #endregion
        #region methods

        /// <summary>
        /// method responsible for applying damage to explosion obstacle
        /// </summary>
        public void TakeDamage(float damageValue)
        {
            if (this.durabilityStats.canBeDestroyed == false || this.DeflectProjectile() == true)
            {
                return;
            }

            CheckIfSurvived(damageValue);
        }
        
        /// <summary>
        /// methode that determins if obstacle can deflect projectiles 
        /// </summary>
        public bool DeflectProjectile() => false;

        /// <summary>
        /// methode that checks if obstacle survived damage application 
        /// </summary>
        private void CheckIfSurvived(float damageValue)
        {
            if (this.durabilityStats.currentHealth <0)
            {
                Destroy(this.gameObject, 2f);
                return;
            }
			
            this.durabilityStats.currentHealth -= damageValue;
            if (this.durabilityStats.currentHealth <= 0f)
            {
                Particles.Play();
            }
        }
        
        /// <summary>
        /// methode that ensures that obstacle takes particle damage
        /// </summary>
        protected void OnParticleCollision(GameObject other)
        {
            TakeDamage(ParticleDamageToObstacle);
        }

        #endregion
        

        
    }
}
