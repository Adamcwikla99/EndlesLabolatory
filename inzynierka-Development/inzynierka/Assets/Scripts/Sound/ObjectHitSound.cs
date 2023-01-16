using Events.Sound;
using FMODUnity;
using UnityEngine;

namespace Sound
{
    /// <summary>
    ///  Class that implements object hit sound logic
    /// </summary>
    public class ObjectHitSound : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private StudioEventEmitter ObjectHitEffect { get; set; }
        
        [field: SerializeField]
        private EntityHit EnemyFireEffect { get; set; }

        #endregion
        #region unityCallbacks

        private void OnEnable() => this.EnemyFireEffect.PlayHitSound += this.ObjectHitEffect.Play;

        private void OnDisable() => this.EnemyFireEffect.PlayHitSound -= this.ObjectHitEffect.Play;
        
        #endregion

    }
}
