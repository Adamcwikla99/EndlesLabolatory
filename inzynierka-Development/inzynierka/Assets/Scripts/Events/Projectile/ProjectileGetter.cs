using Structures.Enums;
using UnityEngine;

namespace Events.Projectile
{
    /// <summary>
    ///  Class responsible for relaying providing projectile from poll events
    /// </summary>
    [CreateAssetMenu(fileName = "ProjectileGetter", menuName = "Projectile/ProjectileGetter")]
    public class ProjectileGetter : ScriptableObject
    {
        public System.Func<ProjectileType, Projectaile.Projectile> GetProjectile;
    }
}
