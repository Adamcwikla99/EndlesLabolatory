using System.Collections;
using System.Collections.Generic;
using Room;
using Room.RoomSpecificActions;
using Structures.Enums;
using UnityEngine;

/// <summary>
///  Class that implements drop pickup logic
/// </summary>
public class TriggerCaller : MonoBehaviour
{
    #region properties

    [field: SerializeField]
    private BulletStatType StatsBuffType { get; set; }

    [field: SerializeField]
    private ItemManager RoomItemManager { get; set; }

    [field: SerializeField]
    private float MinValue { get; set; }
    
    [field: SerializeField]
    private float MaxValue { get; set; }
    
    #endregion

    #region unityCallbacks

    /// <summary>
    /// methode responsible for calling player stat boost event with chosen values 
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player.Player>() == null)
        {
            return;
        }
        
        this.RoomItemManager.pickupItem?.Invoke(this.StatsBuffType, Tools.GetRandomNumberFromRange(this.MinValue, this.MaxValue));
    }

    #endregion

}
