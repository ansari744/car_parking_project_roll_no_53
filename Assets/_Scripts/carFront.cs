using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carFront : MonoBehaviour
{
     public PlayerScript player;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("carFront"))
        {
            player.carFront = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("carFront"))
        {
            player.carFront = false;
        }
    }
}
