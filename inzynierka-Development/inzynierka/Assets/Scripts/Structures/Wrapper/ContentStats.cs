using UnityEngine.Serialization;

namespace Structures.Wrapper
{
    /// <summary>
    /// data wraper for object durability stats information
    /// </summary>
    [System.Serializable]
    public class ContentDurabilityStats 
    {
        #region variables

        [FormerlySerializedAs("startHealth")]
        public float maxHealth;
        public float currentHealth;
        public bool canBeDestroyed;
        
        #endregion


    }
}
