using UnityEngine;

namespace Animations.Enemys.ShieldEnemy
{
    /// <summary>
    ///  class responsible for playing shield enemy animation
    /// </summary>
    public class ShieldEnemyAnimator : MonoBehaviour
    {
        #region properties

        [field: SerializeField]
        private Animator BaseEnemy { get; set; }

        #endregion
        #region variables

        private const float animationTime = 3f;
        private float currentAnimationTime = 0f;
        private static readonly int start = Animator.StringToHash("Start");
        private bool startedPlay = false;
        private bool startCount = false;        

        #endregion 
        #region methods

        /// <summary>
        /// methode that plays animation
        /// </summary>
        public void PlayAnimation()
        {
            this.BaseEnemy.SetTrigger(start);
            this.startedPlay = true;
        }

        #endregion
    }
}
