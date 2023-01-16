using Animations.Enemys.BaseEnemy;
using UnityEngine;

namespace Room.RoomContent.Enemys.Normal.WallEnemy
{
    /// <summary>
    ///  Class that implements wall enemy logic
    /// </summary>
    public class WallEnemy : RoomEntity
    {
        #region properties

        [field: SerializeField]
        private BaseEnemyAnimation DesolveAnimation { get; set; }

        #endregion
        #region variables

    

        #endregion
        #region unityCallbacks

        new private void Start()
        {
            this.thisEntity = this.gameObject.GetComponent<WallEnemy>();
        }

        #endregion
        #region methods

        /// <summary>
        /// methode that plays desolve animation
        /// </summary>
        public override void PlayDesolveAnimation()
        {
            this.DesolveAnimation.PlayAnimation();
        }

        #endregion

    }
}
