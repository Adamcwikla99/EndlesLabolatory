using System;
using FMODUnity;
using UnityEngine;

namespace Sound
{
    /// <summary>
    ///  Class that implements test sound logic
    /// </summary>
    public class TestSoundPlayer : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private StudioEventEmitter Test { get; set; }
        
        #endregion

        #region variables

        public bool Play=false;

        #endregion

        #region unityCallbacks

        private void Start()
        {
            this.Test.AllowFadeout = true;
        }

        /// <summary>
        /// methode that plays sound when value of bool variable was changed in editor
        /// </summary>
        private void Update()
        {
            if (Play == true)
            {
                Play = false;
                Test.Play();
                this.Test.EventInstance.setVolume(0.5f);
            }
        }

        #endregion

        #region methods

    

        #endregion



    }
}
