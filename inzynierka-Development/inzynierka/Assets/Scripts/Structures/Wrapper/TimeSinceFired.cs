namespace Structures.Wrapper
{
    /// <summary>
    ///  data wraper for time since last fire informations
    /// </summary>
    public class TimeSinceFired
    {
        public bool fired;
        public float timeSinceLastFired;

        public TimeSinceFired()
        {
            this.fired = false;
            this.timeSinceLastFired = 0.0f;
        }
        
    }
}
