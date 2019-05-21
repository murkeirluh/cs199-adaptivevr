using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ChildDecision.cs
 * ===============
 * This code handles the situation where in a child asks to play in the water
 * in the During typhoon scene.
 * 
 */

public class ChildDecision : MonoBehaviour
{
    
    bool incorrect;
    private float count = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        incorrect = false;

        if (GameObject.Find("Game").GetComponent<Player>().HHChildren > 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // handles the event when user says yes to child
    public void YesButton()
    {
        GameObject.Find("child_wrong").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("child_wrong").transform.GetChild(1).gameObject.SetActive(true);

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);

        GameObject.Find("Player").GetComponent<Adaptivity>().eventRunning = "Child";
        GameObject.Find("Player").GetComponent<Adaptivity>().WrongMove();

        incorrect = true;
    }

    // handles the event when user says no to child
    public void NoButton()
    {
        GameObject.Find("Player/Main Camera/UI").transform.GetChild(8).gameObject.SetActive(true); // correct message

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (incorrect)
        {
            if (count > 7.0f)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameObject.Find("child_wrong").transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find("child_wrong").transform.GetChild(1).gameObject.SetActive(false);

                incorrect = false;
                count = 0.0f;
            }

            count += Time.deltaTime;
        }
    }
}