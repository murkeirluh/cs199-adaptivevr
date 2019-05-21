using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Moving_Water_Script.cs
 * ------------
 * This script is responsible for the scene where the user
 * can get carried away by the moving water hazard.
 * 
 */

public class Moving_Water_Script : MonoBehaviour
{
    bool enter = true; // rid public
    bool stay = true;

    bool insideTrigger;
    public float flowSpeed = 1.5f;
    GameObject parent;
    public GameObject resPoint;

    // Start is called before the first frame update
    void Start()
    {
        insideTrigger = false;
        resPoint = GameObject.Find("Checkpoints/Moving Water");
    }

    // respawn user to checkpoint before hazard 
    void Respawn() {
        Debug.Log("Respawn");
        GameObject.Find("Player").GetComponent<Walk>().isWalking = false;        
        GameObject.Find("Player").transform.position = resPoint.transform.position;

        GameObject.Find("Player").GetComponent<Adaptivity>().eventRunning = "Moving";
        GameObject.Find("Player").GetComponent<Adaptivity>().WrongMove();    
    }

    // Update is called once per frame
    void Update()
    {
        if (insideTrigger) {
            parent.transform.position = parent.transform.position - transform.right * flowSpeed * Time.deltaTime;
        }
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
            parent.GetComponent<Walk>().isWalking = false;
            insideTrigger = true;
            Debug.Log(insideTrigger);
        }
    }

    // sends user back to checkpoint after hazard interacts with user
    private float stayCount = 0.0f;
    private void OnTriggerStay(Collider other)
    {
        if (stay)
        {
            if (stayCount > 7.0f)
            {
                stayCount = 0.0f;
                Debug.Log("KEME");
                insideTrigger = false;
                Respawn();
            }
            //TEST

            stayCount = stayCount + Time.deltaTime;
        }
    }
}
