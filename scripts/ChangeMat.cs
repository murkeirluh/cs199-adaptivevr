using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ChangeMat.cs
 * ---------------
 * This handles the changing of the walker tile's material once hovered 
 * in and out by the reticle.
 * 
 */

public class ChangeMat : MonoBehaviour {

    public Material originalMaterial, hoverMaterial;
    public GameObject popUp;

	// Use this for initialization
	void Start () {
        popUp = GameObject.Find("Supplies");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // changes the material of the walker tile once reticle hovers on
    public void OnHoverChange() {
        GetComponent<Renderer>().material = hoverMaterial;
        if (popUp != null)
        {
            popUp.GetComponent<ShowObjectName>().TimeForShow(true);
        }

    }

    // reverts the material of the walker tile once reticle hovers out
    public void OnExitChange()
    {
        GetComponent<Renderer>().material = originalMaterial;
        if (popUp != null) popUp.GetComponent<ShowObjectName>().HideName();
    }
}
