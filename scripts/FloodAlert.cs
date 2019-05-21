using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* FloodAlert.cs
 * ------------
 * Activates flood alerts in the LPG/Main switch scenario. 
 *
 */

public class FloodAlert : MonoBehaviour
{
    GameObject floodAlert;
    GameObject floodAlert1;

    // Update is called once per frame
    void Start() {
        floodAlert = GameObject.Find("Player/Main Camera/UI/Flood Alert");
        floodAlert1 = GameObject.Find("Player/Main Camera/UI/Flood Alert (1)");
    }
    void Update()
    {
        Debug.Log(floodAlert.activeSelf);
       /* if (Input.GetMouseButtonDown(0) && floodAlert.activeSelf == true) {
            Debug.Log("PRESSED");
            floodAlert.SetActive(false);
        } else if (Input.GetMouseButtonDown(0) && floodAlert1.activeSelf == true) {
            Debug.Log("PRESSED");
            floodAlert1.SetActive(false);
        }*/
    }
}
