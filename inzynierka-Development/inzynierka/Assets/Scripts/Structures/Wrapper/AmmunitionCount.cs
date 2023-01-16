using UnityEngine;

namespace Structures.Wrapper
{
    /// <summary>
    /// data wraper for ammunition type information
    /// </summary>
    [System.Serializable]
    public class AmmunitionCount
    {
        [SerializeField]
        public Structures.Enums.ProjectileType type;

        [SerializeField]
        
        public int count;
        
        [SerializeField]
        public bool consumable;

    }
}
