using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ElectricEvent_Script.cs
 * ------------
 * This script handles the electrical posts hazard event.
 * 
 */
public class ElectricEvent_Script : MonoBehaviour
{
    bool enter = true; // rid public
    bool stay = true;
    public GameObject parent;
    AudioSource audio;
    public GameObject resPoint;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        resPoint = GameObject.Find("Start");
    }

    // respawn user to checkpoint before hazard 
    void Respawn() {
        Debug.Log("Respawn");
        GameObject.Find("Player").GetComponent<Walk>().isWalking = false;        
        GameObject.Find("Player").transform.position = resPoint.transform.position;
        
        GameObject.Find("Player").GetComponent<Adaptivity>().eventRunning = "Electric";
        GameObject.Find("Player").GetComponent<Adaptivity>().WrongMove();        
    }

    // handle event when user enters the hazard collider
    private void OnTriggerEnter(Collider other)
    {
        if (enter)
        {
            Debug.Log("entered");
            // Get parent of collider
            Debug.Log(other.name);
            parent = other.gameObject;
            // GetChild of player (VFX) then SetActive
            parent.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
            audio.Play();
        }
    }

    // sends user back to checkpoint after hazard interacts with user
    private float stayCount = 0.0f;
    private void OnTriggerStay(Collider other)
    {
        if (stay)
        {
            if (stayCount > 0.5f)
            {
                // GetComponent isWalking false
                parent.GetComponent<Walk>().isWalking = false;
            }

            if (stayCount > 3.0f)
            {
                stayCount = 0.0f;
                parent.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                audio.Stop();                
                Respawn();
            }

            stayCount = stayCount + Time.deltaTime;
            
        }
    }
}
