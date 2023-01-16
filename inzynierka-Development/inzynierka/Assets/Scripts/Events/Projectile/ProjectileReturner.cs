using Structures.Enums;
using UnityEngine;

namespace Events.Projectile
{
    /// <summary>
    ///  Class responsible for relaying item return to que events
    /// </summary>
    [CreateAssetMenu(fileName = "ProjectileReturner", menuName = "Projectile/ProjectileReturner")]
    public class ProjectileReturner : ScriptableObject
    {
        public System.Action<Projectaile.Projectile, ProjectileType> ReturnToQue;
    }
}
