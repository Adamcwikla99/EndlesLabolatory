using UnityEngine;

namespace Events.Sound
{
    /// <summary>
    ///  
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyFired", menuName = "Sound/EnemyFired")]
    public class EnemyFired : ScriptableObject
    {
        public System.Action PlayEnemyFireSound;
    }
}
