using Interface;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.ShieldEnemy
{
    /// <summary>
    ///  Class that implements shield body logic
    /// </summary>
    public class ShieldBody : MonoBehaviour, IDamage
    {
        #region properties

        [field: SerializeField]
        private ShieldController ThisTurretController { get; set; }

        #endregion

        #region variables

    

        #endregion

        #region unityCallbacks

        private void Start()
        {
        
        }

        private void Update()
        {
        
        }

        #endregion

        #region methods

    

        #endregion

        /// <summary>
        /// methode that applies damage to shield enemy entity  
        /// </summary>
        public void TakeDamage(float damageValue)
        {
            this.ThisTurretController.BodyReceivedDamage(damageValue);
        }
    
    
        /// <summary>
        /// methode that determines if shield enemy can deflect projectiles   
        /// </summary>
        public bool DeflectProjectile() => false;
    }
}
