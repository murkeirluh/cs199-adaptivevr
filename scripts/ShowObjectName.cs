using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * ShowObjectName.cs
 * ------------
 * This script is responsible for the showing of object names once the reticle hovers on one.
 * 
 */

public class ShowObjectName : MonoBehaviour
{

    public GvrReticlePointer reticle;
    public Text popUp;
    int seconds;
    private bool walker;
    private GameObject currentObject;

    // Start is called before the first frame update
    void Awake()
    {   
        if (popUp == null) {
            popUp = GameObject.Find("Player/Main Camera/UI/Popup/Button/Text").GetComponent<Text>(); 
            }
        // set popup button to inactive
        HideName();
        seconds = 0;

    }

    // Update is called once per frame
    void Update()
    {   
        //if (SceneManager.GetActiveScene().name == "evacuation")
        //{
        //    currentObject = reticle.CurrentRaycastResult.gameObject;
        //    if (currentObject != null && currentObject != reticle.CurrentRaycastResult.gameObject && seconds == 0) HideName();

        //}
        //if (currentObject.name != null && currentObject.transform.parent.name != "Supplies" || currentObject.transform.parent.name != "Extra stuff") Reset();
    }

    // starts 2-second timer before showing popup
    public IEnumerator StartTimer() {

        while (true) {
            if (seconds < 2) {
                //Debug.Log("counting, " + seconds);
                seconds++;
                yield return new WaitForSeconds(1);
            }

            else if (seconds >= 2)
            {
                ShowName();
                //Debug.Log("Seconds: " + seconds);
                yield break;

            }

            else if (currentObject != reticle.CurrentRaycastResult.gameObject)
            {
                HideName();
                yield break;
            }

        }
    }

    // calls to start the timer
    public void TimeForShow(bool walkerTile = false)
    {
        currentObject = reticle.CurrentRaycastResult.gameObject;
        
        if (walkerTile)
        {
            walker = true;

        }
        StartCoroutine(StartTimer());

    }

    // shows the popup
    public void ShowName()
    {
        //Debug.Log(string.Format("Show name: {0}", currentObject.name));
        if (currentObject != null && !walker) {
            //Debug.Log(string.Format("Looking at: {0}, Parent: {1}", currentObject.name, currentObject.transform.parent.name));
            if (currentObject.transform.parent.name == "Supplies" || currentObject.transform.parent.name == "Extra stuff") {
                popUp.transform.parent.gameObject.SetActive(true);
                popUp.text = currentObject.name;
            }

            else if (currentObject.transform.parent.name == "Backpack")
            {
                //Debug.Log(string.Format("Looking at: {0}, Parent: {1}", currentObject.name, currentObject.transform.parent.name));
                popUp.transform.parent.gameObject.SetActive(true);
                popUp.text = "Backpack / Supplies Kit";
            }
        }

        if (walker)
        {
            //popUp.transform.parent. = 
            popUp.transform.parent.gameObject.SetActive(true);
            popUp.text = "Walker tile. Click me\nto start walking!";
        }
    }

    public void HideName()
    {
        popUp.transform.parent.gameObject.SetActive(false);
        Reset();
    }

    // reset variables, hides popup, and stops timer
    public void Reset()
    {
        StopCoroutine(StartTimer()); 
        seconds = 0;
        walker = false;
        popUp.transform.parent.gameObject.SetActive(false);
        currentObject = null;
    }
}
