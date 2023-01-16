using UnityEngine;

namespace Structures.Wrapper
{
    /// <summary>
    ///  data wraper for projectile pool data informations
    /// </summary>
    [System.Serializable]
    public class ProjectilePoolData
    {
        #region variables

        [SerializeField]
        public GameObject prefab;

        [SerializeField]
        public Structures.Enums.ProjectileType projectileType;

        [SerializeField]
        public int initialCount;

        #endregion
    }
}
