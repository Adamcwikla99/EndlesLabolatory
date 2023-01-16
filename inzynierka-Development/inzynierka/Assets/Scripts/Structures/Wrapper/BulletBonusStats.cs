namespace Structures.Wrapper
{
    /// <summary>
    ///  Data wraper for bullet stats information
    /// </summary>
    [System.Serializable]
    public class BulletBonusStats 
    {
        #region variables

        public float bulletBonusDamage;
        public float bulletBonusSpeed;
        public float bulletBonusReloadTime;

        #endregion
        #region methods

        public BulletBonusStats()
        {
            this.bulletBonusDamage = 0f;
            this.bulletBonusSpeed = 0f;
            this.bulletBonusReloadTime = 0f;
        }

        #endregion
    }
}
