using Interface;
using UnityEngine;

namespace Room.RoomContent.Obsticle
{
    /// <summary>
    ///  Class that implements deflection obstacle logic
    /// </summary>
    public class DeflectionObstacle : RoomObstacle, IDamage
    {
        #region unity callbacs

        private void OnCollisionEnter(Collision collision)
        {
            Deflection(collision);
        }

        #endregion
        #region methodes

        /// <summary>
        /// method responsible for applying damage to deflection obstacle
        /// </summary>
        public void TakeDamage(float damageValue) { }
        
        /// <summary>
        /// methode that determins if obstacle can deflect projectiles 
        /// </summary>
        public bool DeflectProjectile() => true;

        /// <summary>
        /// methode that deflects game object that came in contact with it
        /// </summary>
        private void Deflection(Collision collision)
        {
            GameObject projectileGameObject = collision.gameObject; 
            Rigidbody projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>() ;
            float speed = projectileRigidbody.velocity.magnitude;
            Vector3 newDirection = Vector3.Reflect(projectileRigidbody.velocity.normalized, collision.contacts[0].normal);
            projectileRigidbody.velocity = newDirection * 10f;
        }
        
        #endregion

    }
}
