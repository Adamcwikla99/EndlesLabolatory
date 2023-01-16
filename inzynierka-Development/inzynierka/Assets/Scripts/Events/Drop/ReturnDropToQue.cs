using Structures.Enums;
using UnityEngine;


namespace Events.Drop
{
    /// <summary>
    ///  Class responsible for relaying drop returning projectile to que events
    /// </summary>
    [CreateAssetMenu(fileName = "ReturnDropToQue", menuName = "DropItem/ReturnDropToQue")]
    public class ReturnDropToQue : ScriptableObject
    {
        public System.Action<DropType, global::Room.RoomContent.Drop.Drop> ReturnToDropQue;
    }
}
