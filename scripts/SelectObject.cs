using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SelectObject.cs
 * ------------
 * This script handles the user's "grabbing" mechanism. This takes care of the player holding objects.
 * 
 */

public class SelectObject : MonoBehaviour
{
    public GvrReticlePointer reticle;     
    public GameObject grabHand;
    public GameObject objectPointed;
    public Transform initParent;
    
    public Transform previousTransform; // previous transform ng gameobject selected
    public bool isGrabbing;

    // Start is called before the first frame update
    public void Grab() 
    {
        if (isGrabbing == false) {
            // check what the reticle is currently looking at
            objectPointed = reticle.CurrentRaycastResult.gameObject;
            // get the object's previous transform
            previousTransform = objectPointed.transform;
            objectPointed.GetComponent<Collider>().attachedRigidbody.isKinematic = true;
            // store the object's initial parent
            initParent = objectPointed.transform.parent;
            // parent the object to the Player's "hand"
            objectPointed.transform.parent = grabHand.transform; // parents kay grabHand
            objectPointed.transform.position = grabHand.transform.position;
            // return true now that user is "holding" something
            isGrabbing = true;

            // if checklist item, successful click: reset timer
            if (initParent.name == "Supplies") gameObject.GetComponent<Adaptivity>().ResetTimer();
        }
    }

    void Start()
    {
        isGrabbing = false;
        initParent = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
