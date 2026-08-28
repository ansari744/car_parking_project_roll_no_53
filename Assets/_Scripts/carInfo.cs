using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carInfo : MonoBehaviour
{
    public Transform carView;
    public int carNumber;
    public int  unLocked;
    public int carPrice;
    public bool permanentUnlocked;
    private void Start()
    {

        if (permanentUnlocked)
            unLocked = 1;
        else
            unLocked = PlayerPrefs.GetInt("Unlocked" + carNumber.ToString());
        
    }
}
