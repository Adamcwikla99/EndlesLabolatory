using UnityEngine;

namespace Animations.Enemys.WallEnemy
{
    /// <summary>
    ///  class responsible for playing wall enemy animation
    /// </summary>
    public class WallEnemyAnimator : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private Animator WallEnemy { get; set; }

        #endregion
    }
}
