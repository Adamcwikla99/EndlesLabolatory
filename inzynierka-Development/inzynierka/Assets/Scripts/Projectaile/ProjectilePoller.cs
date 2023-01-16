using System.Collections.Generic;
using Events.Projectile;
using Structures;
using Structures.Enums;
using Structures.Wrapper;
using UnityEngine;

namespace Projectaile
{
    /// <summary>
    ///  Class that stores created and unused projectiles - implement object poll patern
    /// </summary>
    public class ProjectilePoller : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private List<ProjectilePoolData> IndividualPools { get; set; } = new List<ProjectilePoolData>();
        
        [field: SerializeField]
        private ProjectileGetter GetProjectileEvents { get; set; }
        
        [field: SerializeField]
        private ProjectileReturner IndividualProjectileReturner { get; set; }

        [field: SerializeField]
        private int QueSizeIncrise { get; set; } = 10;

        #endregion
        #region variables

        private Dictionary<Structures.Enums.ProjectileType, Queue<Projectile>> projectilePool = new Dictionary<ProjectileType, Queue<Projectile>>();

        #endregion
        #region unityCallbacks

        private void Start()
        {
            InitDictionary();
        }

        private void OnEnable()
        {
            EnableEvents();
        }

        private void OnDisable()
        {
            DisableEvents();
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that initialize projectile object dictionary
        /// </summary>
        private void InitDictionary()
        {
            foreach (ProjectilePoolData projectilePoolData in IndividualPools)
            {
                IndividualProjectileTypeSpawn(projectilePoolData);
            }   
        }

        /// <summary>
        /// methode that spawns projectiles of specified type
        /// </summary>
        private void IndividualProjectileTypeSpawn(ProjectilePoolData projectilePoolData)
        {
            projectilePool.Add(projectilePoolData.projectileType, new Queue<Projectile>());
            for (int i = 0; i < projectilePoolData.initialCount; i++)
            {
                Projectile newProjectile = Instantiate(projectilePoolData.prefab, this.transform).GetComponent<Projectile>();
                projectilePool[projectilePoolData.projectileType].Enqueue(newProjectile);                        
            }     
        }

        /// <summary>
        /// methode that provides projectile of given type for script usage
        /// </summary>
        private Projectile GetProjectile(Structures.Enums.ProjectileType type)
        {
            if (projectilePool[type].Count >0)
            {
                return projectilePool[type].Dequeue();
            }

            GameObject toSpawn = IndividualPools.Find(x => x.projectileType == type).prefab;
            for (int i=0; i<QueSizeIncrise;i++)
            {
                Projectile newProjectile = Instantiate(toSpawn, this.transform).GetComponent<Projectile>();
                projectilePool[type].Enqueue(newProjectile);     
            }
            
            return projectilePool[type].Dequeue();
        }

        /// <summary>
        /// methode that returns projectile object to que
        /// </summary>
        private void ReturnProjectileToQue(Projectile projectile, Structures.Enums.ProjectileType type)
        {
            projectile.gameObject.SetActive(false);
            projectilePool[type].Enqueue(projectile);
        }

        /// <summary>
        /// methode that enables events
        /// </summary>
        private void EnableEvents()
        {
            this.GetProjectileEvents.GetProjectile += GetProjectile;
            this.IndividualProjectileReturner.ReturnToQue += ReturnProjectileToQue;
        }

        /// <summary>
        /// methode that disables events
        /// </summary>
        private void DisableEvents()
        {
            this.GetProjectileEvents.GetProjectile -= GetProjectile;
            this.IndividualProjectileReturner.ReturnToQue -= ReturnProjectileToQue;
        }
        
        #endregion
    }
}
