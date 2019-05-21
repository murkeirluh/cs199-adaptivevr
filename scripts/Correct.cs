using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Correct.cs
 * ------------
 * Pops up a congratulatory + explanation message after the fallen electrical posts
 * hazard scene in the During typhoon scenario.
 *
 */

public class Correct : MonoBehaviour
{
    public GameObject parent;
    
    bool enter = true;
    bool wasSetOff;

    void Start() {
        wasSetOff = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        parent = other.gameObject;
        // if the collider was triggered 
        if (enter && parent.name == "Player" && wasSetOff == false) {
            // player stops walking
            parent.GetComponent<Walk>().isWalking = false;
            // show explanation + "good job" popup
            parent.transform.GetChild(0).GetChild(3).GetChild(4).gameObject.SetActive(true); //mod421
            wasSetOff = true;
        }
    }

}
