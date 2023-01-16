using Events.Sound;
using FMODUnity;
using UnityEngine;

namespace Sound
{
    /// <summary>
    ///  Class that implements enemy fire sound logic
    /// </summary>
    public class EnemyFireSound : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private StudioEventEmitter EnemyFireEffect { get; set; }

        [field: SerializeField]
        private EnemyFired FireEvent { get; set; }

        #endregion
        #region unityCallbacks

        private void OnEnable() => this.FireEvent.PlayEnemyFireSound += this.EnemyFireEffect.Play;

        private void OnDisable() => this.FireEvent.PlayEnemyFireSound -= this.EnemyFireEffect.Play;
        
        #endregion

    }
}
