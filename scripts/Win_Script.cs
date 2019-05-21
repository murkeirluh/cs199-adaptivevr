using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Win_Script.cs
 * ------------
 * This handles the event where the user reaches
 * the evacuation center. This shows a congratulatory pop-up message
 * then returns the user back to the menu.
 *
 */

public class Win_Script : MonoBehaviour
{
    bool enter = true; // rid public

    public GameObject parent;
    
    private void OnTriggerEnter(Collider other)
    {
        parent = other.gameObject;

        if (enter && other.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<Walk>().isWalking = false;
            parent.transform.GetChild(0).GetChild(3).GetChild(6).gameObject.SetActive(true); //mod421
            parent.transform.GetChild(0).GetChild(3).GetChild(7).gameObject.SetActive(true); //mod421        
            GetComponent<AudioSource>().Play();
        }
    }

    public void ChangeScene() {
        Initiate.Fade("main_menu", Color.black, 2.0f);
    }
}
