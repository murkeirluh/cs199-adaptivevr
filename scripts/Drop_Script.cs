    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Drop_Script.cs
 * ------------
 * Pops up a congratulatory + explanation message after the still water
 * hazard scene in the During typhoon scenario.
 *
 */

public class Drop_Script : MonoBehaviour
{
    public GameObject parent;
    
    bool enter = true;
    bool wasSetOff;

    // Start is called before the first frame update
    void Start()
    {
        wasSetOff = false;        
    }

    private void OnTriggerEnter(Collider other)
    {
        parent = other.gameObject;
        // if the collider was triggered 
        if (enter && parent.name == "Player" && wasSetOff == false) {
            // destroy stick object
            Destroy(parent.transform.GetChild(0).GetChild(1).GetChild(1).gameObject); //mod421
            // player stops walking
            parent.GetComponent<Walk>().isWalking = false;
            // show explanation + "good job" popup
            parent.transform.GetChild(0).GetChild(3).GetChild(5).gameObject.SetActive(true); //mod421
            wasSetOff = true;
        }
    }
}
