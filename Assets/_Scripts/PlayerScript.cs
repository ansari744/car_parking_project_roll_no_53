using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Transform target, hoodCamTarget;
    public GameplayManager gm;
    public Rigidbody rb;
    [HideInInspector]
    public bool wrongParking;
    [HideInInspector]
    public bool parked;
    [HideInInspector]
    public bool carFront;

    public Camera rearViewCam;
    //public RawImage mirrorImage;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("wrongParking"))
        {
            wrongParking = true;
        }
        if (other.CompareTag("parking") && !wrongParking )
        {
            if(carFront)
                gm.check();
            parked = true;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("wrongParking"))
        {
            wrongParking = false;
        }
    }
  

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("ignore"))
        {
            gm.checkHealth();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (gm)
        {
            gm.rearView.texture = rearViewCam.targetTexture;

            if (gm.hoodCam)
                gm.rearView.gameObject.SetActive(true);

            else
                gm.rearView.gameObject.SetActive(false);
        }
       
    }
}
