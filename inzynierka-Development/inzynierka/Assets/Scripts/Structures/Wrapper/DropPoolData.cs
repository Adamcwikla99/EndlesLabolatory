using Room.RoomContent.Drop;
using Structures.Enums;

namespace Structures.Wrapper
{
    /// <summary>
    ///  data wraper for droop pool data objects information
    /// </summary>
    [System.Serializable]
    public class DropPoolData 
    {
        #region variables

        public DropType Type;
        public Drop Prefab;
        public int initialCount;

        #endregion

    }
}
