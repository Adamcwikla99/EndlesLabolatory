using UnityEngine;

namespace Animations.Enemys.TurretEnemy
{
    /// <summary>
    ///  class responsible for playing shield enemy animation
    /// </summary>
    public class TurretEnemyAnimator : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private Animator TurretEnemy { get; set; }

        #endregion
        #region variables

        private static readonly int start = Animator.StringToHash("Start");
        
        #endregion
        #region methods

        /// <summary>
        /// methode that plays the animations
        /// </summary>
        public void PlayAnimation()
        {
            this.TurretEnemy.SetTrigger(start);
        }

        #endregion
    }
}
