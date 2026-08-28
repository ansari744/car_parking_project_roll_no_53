using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parkingSpot : MonoBehaviour
{
      GameplayManager gm;
    //PlayerScript player;
    
    public SpriteRenderer parking;
    public GameObject parkingArrow;
    //public Transform target;

    private void Start()
    {
        gm = FindObjectOfType<GameplayManager>();
        
    }

    private void Update()
    {
        parkingArrow.SetActive(true);

        if (gm.currentVehicle != null)
        {
            parkingArrow.transform.LookAt(gm.currentVehicle.transform);
        }
        if (gm.playerScript.wrongParking)
        {
            parkingArrow.SetActive(false);
            gm.playerScript.parked = false;
            parking.color = Color.white;
        }
        else if (gm.playerScript.parked)
        {
                parkingArrow.SetActive(false);
                parking.color = Color.yellow;

                if (gm.playerScript.carFront)
                {
                    parking.color = Color.green;

                }
           
        }


        

    }


}
