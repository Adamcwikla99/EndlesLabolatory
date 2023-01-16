using Unity.VisualScripting;
using UnityEngine;

namespace Animations.Enemys.BaseEnemy
{
    /// <summary>
    ///  class responsible for playing base enemy animation
    /// </summary>
    public class BaseEnemyAnimation : MonoBehaviour
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
        #region unityCallbacks
        
        private void Update()
        {
            CheckAnimationFaze();
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that plays desolve animation
        /// </summary>
        public void PlayAnimation()
        {
            this.BaseEnemy.SetTrigger(start);
            this.startedPlay = true;
        }
        
        /// <summary>
        /// methode that checks if animations has finished
        /// </summary>
        private void CheckAnimationFaze()
        {
            if (!this.startedPlay)
            {
                return;
            }

            if (this.BaseEnemy.GetCurrentAnimatorStateInfo(0).length > this.BaseEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                this.startCount = true;
            }

            CountTime();
        }

        /// <summary>
        /// methode that measures animation time
        /// </summary>
        private void CountTime()
        {
            if (this.startCount == false)
            {
                return;
            }

            this.currentAnimationTime += Time.deltaTime;
            if (this.currentAnimationTime > animationTime)
            {
                Destroy(this.gameObject);                
            }

        }

        #endregion

    }
}
