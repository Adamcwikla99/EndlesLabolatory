using UnityEngine;

namespace Animations.Effects
{
    /// <summary>
    ///  Class that implements explosion effect logic
    /// </summary>
    public class ExplodeEffect : MonoBehaviour
    {
        private System.Action returnAfterDelay;
    
        #region unityCallbacks

        private void Start()
        {
            this.Explode();
        }

        /// <summary>
        /// methode that plays explosion efect 
        /// </summary>
        private void Explode()
        {
            this.returnAfterDelay += this.RemoveRocket;
            this.Invoke(this.returnAfterDelay.Method.Name, 0.5f);
        }
    
        /// <summary>
        /// methode that destroys rocket gameobject
        /// </summary>
        private void RemoveRocket()
        {
            Destroy(this.gameObject);
        }
    
        #endregion
    }
}
