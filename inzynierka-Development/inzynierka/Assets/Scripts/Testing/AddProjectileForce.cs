using UnityEngine;

namespace Testing
{
    /// <summary>
    ///  Class that implements projectile adding force logic - for testing, not used in main project
    /// </summary>
    public class AddProjectileForce : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private Rigidbody AddForceTarget { get; set; }

        [field: SerializeField]
        private Vector3 AddForceValue { get; set; }
        
        #endregion

        #region variables

        private bool addForce = false;

        #endregion

        #region unityCallbacks

        private void Update()
        {
            if (this.addForce == true)
            {
                this.addForce = false;
                AddForceToTarget();
            }
        }

        #endregion

        #region methods

        private void AddForceToTarget()
        {
            this.AddForceTarget.AddForce(this.AddForceValue, ForceMode.Impulse);
        }

        #endregion



    }
}
