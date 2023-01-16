using Events.Projectile;
using Events.Sound;
using FMODUnity;
using Interface;
using Structures.Enums;
using UnityEngine;

namespace Projectaile
{
    /// <summary>
    ///  Class implementing projectile base methods
    /// </summary>
    public abstract class Projectile : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        public ProjectileType ThisType { get; set; }
        
        [field: SerializeField]
        public float BaseSpeed { get; protected set; }
        
        [field: SerializeField]
        public float BaseDamage { get; protected set; }
        
        [field: SerializeField]
        protected ProjectileReturner QueReturnEvents { get; set; }
        
        [field: SerializeField]
        protected Rigidbody ProjectileRigidbody { get; set; }

        [field: SerializeField]
        protected float PlacementOffset { get; set; } = 0.5f;
        
        [field: SerializeField]
        protected EntityHit ObjectHitEffect { get; set; }
        
        #endregion
        #region Variables

        protected Projectile thisProjectile;
        protected float bonusDamage;

        #endregion
        #region unityCallbacks
        
        protected void Start()
        {
            thisProjectile = this.gameObject.GetComponent<Projectile>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnHit(collision);
        }
        
        #endregion
        #region methods

        /// <summary>
        /// methode that fires projectile
        /// </summary>
        public abstract void Fire(Quaternion rotation, Vector3 position, Vector3 velocity, float bonusDamage =0f , float bonusSpeed =0f);
        
        /// <summary>
        /// methode tha rest projectile variables and state
        /// </summary>
        protected void ResetProjectile()
        {
            this.ProjectileRigidbody.velocity = new Vector3(0f,0f,0f); 
            this.ProjectileRigidbody.angularVelocity = new Vector3(0f,0f,0f);
            this.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
        }

        /// <summary>
        /// methode that determines collision behaviour
        /// </summary>
        protected abstract void OnHit(Collision collision);

        /// <summary>
        /// methode that sets projectile parameters 
        /// </summary>
        protected virtual void SetProjectileParameters(Quaternion rotation, Vector3 position, Vector3 velocity, float bonusDamage = 0f , float bonusSpeed =0f)
        {
            this.bonusDamage = bonusDamage;
            this.transform.rotation = rotation;
            this.transform.position = position + (velocity*PlacementOffset);
            this.ProjectileRigidbody.velocity = (velocity * this.BaseSpeed);
        }
        
        /// <summary>
        /// methode that check if collision object can receive damage
        /// </summary>
        protected void DamageInterfaceCheck(Collision collision)
        {
            this.ObjectHitEffect.PlayHitSound?.Invoke();
            IDamage objectInterface = collision.gameObject.GetComponent<IDamage>();
            if (objectInterface == null)
            {
                this.QueReturnEvents.ReturnToQue?.Invoke(this.thisProjectile, this.ThisType);
                return;
            }

            if (objectInterface.DeflectProjectile() == true)
            {
                return;
            }
            
            objectInterface.TakeDamage(this.BaseDamage + this.bonusDamage);
            this.QueReturnEvents.ReturnToQue?.Invoke(this.thisProjectile, this.ThisType);
        }
        
        #endregion
    }
}
