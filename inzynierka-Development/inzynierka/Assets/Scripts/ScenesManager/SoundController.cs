using UnityEngine;

namespace ScenesManager
{
    /// <summary>
    ///  Class that implements sound playing logic
    /// </summary>
    public class SoundController : MonoBehaviour
    {
        #region properties

        public bool applySound;
        public float soundValue;
        public float mValue;
    

        #endregion
        #region variables

        private FMOD.Studio.Bus Master;
        private FMOD.Studio.Bus Effects;
        private FMOD.Studio.Bus Musics;

        #endregion
        #region unityCallbacks

        private void Awake()
        {
            this.Init();
        }

        private void Update()
        {
            this.AdjustSound();
        }

        #endregion
        #region methods

        /// <summary>
        /// method that initialize sound bus variables
        /// </summary>
        private void Init()
        {
            this.Master = FMODUnity.RuntimeManager.GetBus("bus:/");
            this.Musics = FMODUnity.RuntimeManager.GetBus("bus:/Master/Soundtracks");
            this.Effects = FMODUnity.RuntimeManager.GetBus("bus:/Master/Effects");
        }

        /// <summary>
        /// methode that sets sound volume to selected by user value
        /// </summary>
        private void AdjustSound()
        {
            this.Master.setVolume(this.soundValue);
            this.Musics.setVolume(this.mValue);
        }

        #endregion

    }
}
