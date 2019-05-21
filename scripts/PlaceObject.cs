using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PlaceObject.cs
 * ------------
 * This script takes care of placing/dropping an object held by the user on a surface 
 * that the player looks at.
 * 
 */

public class PlaceObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GvrReticlePointer reticle;

    void Place() {
        // make sure user is not looking at backpack or walker tile
        GameObject currentObject = reticle.CurrentRaycastResult.gameObject;
        if (gameObject.GetComponent<SelectObject>().isGrabbing == true && currentObject != null) {
            if (currentObject.name != "Backpack" && currentObject.name != "Johnny Walker" && currentObject.transform.parent.name != "Walls" && currentObject.transform.parent.name != "Checklist") {

                gameObject.GetComponent<SelectObject>().objectPointed.GetComponent<Collider>().attachedRigidbody.isKinematic = false;

                if (gameObject.GetComponent<SelectObject>().initParent != null) {

                    gameObject.GetComponent<SelectObject>().objectPointed.transform.parent = gameObject.GetComponent<SelectObject>().initParent; // parents kay grabHand, replace with Correct or Wrong na parents? or write as attribute ng bawat object?

                }

                else gameObject.GetComponent<SelectObject>().objectPointed.transform.parent = null;
                gameObject.GetComponent<SelectObject>().objectPointed.transform.position = reticle.CurrentRaycastResult.worldPosition;

                gameObject.GetComponent<SelectObject>().isGrabbing = false;
                Debug.Log("Dropped\n");
            }
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            Place();
        }

    }
}
