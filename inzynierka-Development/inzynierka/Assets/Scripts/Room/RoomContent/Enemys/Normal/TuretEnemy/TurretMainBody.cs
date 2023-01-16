using Interface;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.TuretEnemy
{
    /// <summary>
    ///  Class that implements turret main body logic
    /// </summary>
    public class TurretMainBody : MonoBehaviour, IDamage
    {
        #region properties

        [field: SerializeField]
        private TurretController ThisTurretController { get; set; }

        #endregion
        #region variables

    

        #endregion
        #region unityCallbacks


        #endregion
        #region methods

        /// <summary>
        /// methode that applies damage to  
        /// </summary>
        public void TakeDamage(float damageValue)
        {
            ThisTurretController.BodyReceivedDamage(damageValue);
        }
        
        /// <summary>
        /// methode that determines if turret main body can deflect projectiles   
        /// </summary>
        public bool DeflectProjectile() => false;

        #endregion
        
    }
}
