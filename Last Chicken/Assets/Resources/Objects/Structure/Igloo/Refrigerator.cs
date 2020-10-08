using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    public static List<Refrigerator> refrigeratorList = new List<Refrigerator>();

    #region[OnEnable]
    private void OnEnable()
    {
        refrigeratorList.Add(this);
    }
    #endregion

    #region[OnDisable]
    private void OnDisable()
    {
        refrigeratorList.Remove(this);
    }
    #endregion

    
}
