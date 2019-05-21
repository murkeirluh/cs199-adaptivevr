using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SelectStick.cs
 * ------------
 * This script takes care of the events associated with the stick in the During typhoon scene, such as
 * grabbing the stick before the still water scenario and then dropping it after the user crosses the water.
 *
 */

public class SelectStick : MonoBehaviour
{
    // the player's "hand"
    public GameObject grabHand;
    // the "hole" that player falls into
    public GameObject fallEvent;

    public bool isGrabbed;

    // allows user to "hold" the stick
    public void Grab() 
    {
        transform.parent = grabHand.transform; // parents kay grabHand
        transform.position = grabHand.transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
        // disables the stick's highlight/outline
        transform.GetChild(0).gameObject.GetComponent<cakeslice.Outline>().enabled = false;
        GetComponent<Collider>().enabled = false;
        // activate the hole's collider so player doesn't shoot through
        fallEvent.GetComponent<Collider>().enabled = true;
        
        isGrabbed = true;
    }

    // destroy the stick
    void Drop() {
        Destroy(gameObject);
    }
    
    void Start()
    {
        isGrabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
