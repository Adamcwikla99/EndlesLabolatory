namespace Structures.Wrapper
{
    /// <summary>
    ///  data wraper for player effects information
    /// </summary>
    [System.Serializable]
    public class PlayerEffects 
    {
        #region variables

        public bool SpeedUp;
        public bool SpeedDown;
        public bool HealthUp;
        public bool HealthDown;

        #endregion
        #region methods

        public PlayerEffects()
        {
            this.SpeedUp = false;
            this.SpeedDown = false;
            this.HealthUp = false;
            this.HealthDown = false;
        }

        #endregion

    }
}
