using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* ActivateDoor.cs
 * ---------------
 * This script checks for the conditions during the turning off of LPG tank
 * and electricity main switch scene. (if applicable)
 * If correct conditions are met, the door will be highlighted and activated,
 * and the user can move on to the next module (During typhoon).
 * 
 */

public class ActivateDoor : MonoBehaviour
{
    public GameObject fuseBox;
    public GameObject lpgTank;
    public GameObject arrow;
    bool fuseOff, lpgOff;
    public GameObject floodAlertText;
    bool hasElectricity, hasLPG;
   
    // Start is called before the first frame update
    void Start()
    {
        // set all variables
        fuseBox = GameObject.Find("Turn off/Fuse");
        lpgTank = GameObject.Find("Turn off/LPG");
        arrow = GameObject.Find("Turn off").transform.GetChild(7).gameObject;
        hasElectricity = GameObject.Find("Game").GetComponent<Player>().hasElectricity;
        hasLPG = GameObject.Find("Game").GetComponent<Player>().hasLPG;

        GetComponent<cakeslice.Outline>().eraseRenderer = true;
        GetComponent<cakeslice.Outline>().enabled = false;
        GetComponent<Collider>().enabled = false;
        transform.GetChild(0).gameObject.GetComponent<cakeslice.Outline>().enabled = false;


    }

    // called when activated door is clicked
    public void ChangeScene() {
        Initiate.Fade("evacuation", Color.black, 2.0f);
    }

    // Update is called once per frame
    void Update() {

        if (SceneManager.GetActiveScene().name == "bahay_interior")
        {
            fuseBox = GameObject.Find("Turn off/Fuse");
            lpgTank = GameObject.Find("Turn off/LPG");
            // check if door should activate
            if (ShouldActivate())
            {
                // enable event trigger and door highlight
                GetComponent<cakeslice.Outline>().enabled = true;
                GetComponent<Collider>().enabled = true;
                GetComponent<cakeslice.Outline>().eraseRenderer = false;
                transform.GetChild(0).gameObject.GetComponent<cakeslice.Outline>().enabled = true;
                // activate arrow pointing out the door
                arrow.SetActive(true);

            }
        }

    }

    // checks for conditions if the door highlight should be activated
    bool ShouldActivate()
    {
        hasElectricity = GameObject.Find("Game").GetComponent<Player>().hasElectricity;
        hasLPG = GameObject.Find("Game").GetComponent<Player>().hasLPG;
        if (fuseBox != null && lpgTank != null)
        {
            fuseOff = GameObject.Find("Turn off/Fuse").GetComponent<TurnOff>().fuseOff;
            lpgOff = GameObject.Find("Turn off/LPG").GetComponent<TurnOff>().lpgOff;
            Debug.Log(fuseOff + ", " + lpgOff);
        }
        
        // if both exist,
        if (hasElectricity && hasLPG)
        {
            // both gameobjects should be active and outlines gone
            if (fuseBox != null && lpgTank != null && fuseOff && lpgOff)
                return true;
        }

        // else, check individual active state and outlines
        else if (hasElectricity)
        {
            fuseOff = GameObject.Find("Turn off/Fuse").GetComponent<TurnOff>().fuseOff;
            if (fuseBox.activeSelf && fuseOff)
                return true;
        }

        else if (hasLPG)
        {
            lpgOff = GameObject.Find("Turn off/LPG").GetComponent<TurnOff>().lpgOff;
            if (lpgTank.activeSelf && lpgOff)
                return true;
        }

        else if (!hasElectricity && !hasLPG) return true;

        return false;
    }
}
