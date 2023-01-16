using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  
/// </summary>
public class BuildDebuging : MonoBehaviour
{
    #region properties

    [field: SerializeField]
    private GameObject ToSpawn { get; set; }

    #endregion

    #region variables

    

    #endregion

    #region unityCallbacks

    private void Awake()
    {
        Instantiate(ToSpawn, this.transform.position, Quaternion.identity);
    }

    #endregion

    #region methods

    

    #endregion



}
