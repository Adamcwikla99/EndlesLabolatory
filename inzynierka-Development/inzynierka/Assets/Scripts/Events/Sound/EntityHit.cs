using UnityEngine;

namespace Events.Sound
{
    /// <summary>
    ///  
    /// </summary>
    [CreateAssetMenu(fileName = "EntityHit", menuName = "Sound/EntityHit")]
    public class EntityHit : ScriptableObject
    {
        public System.Action PlayHitSound;
    }
}
