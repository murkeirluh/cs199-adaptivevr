using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * SuppliesKitGame.cs
 * ------------
 * This script is attached to the Checklist object.
 * 
 * This is responsible for the spawning and deactivating 
 * of the supplies in the house, rearrangement of the checklist,
 * and for transitioning to the next scene after the
 * Emergency Supplies Kit game.
 * 
 */

public class SuppliesKitGame : MonoBehaviour
{
    private IDictionary<string, int> checklist;
    public GvrReticlePointer reticle;
    public bool debug;
    // upper bound for time remaining before rearranging checklist (default: 420)
    // 420 seconds == 7 minutes remaining (8 minutes passed)
    public int threshold;

    private int total;
    private float progress;
    
    private int seconds;
    private bool destroyed;

    // audio
    AudioSource[] sfx;
    AudioSource correct, completed;


    void Awake()
    {
        destroyed = false;
        sfx = GameObject.Find("Supplies").GetComponents<AudioSource>();
        correct = sfx[0];
        completed = sfx[1];

    }

    // Start is called before the first frame update
    void Start()
    {

        // initialize variables
        checklist = GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().checklist;

        total = 0;
        seconds = 0;
        // !?!??!?! 
        debug = false;

        total = GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().GetTotal();
        progress = 0.0f;

        if (debug) {
            threshold = 870; 
        }
        else {
            threshold = 420; 
        }

        // if meds are needed, activate
        if (GameObject.Find("Game").GetComponent<Player>().HHNeedMeds == 0)
        {
            GameObject.Find("Supplies/Medicine").SetActive(false);
        }

        else GameObject.Find("Supplies/Medicine").SetActive(true);

        // if there are no children + !well-off
        if (GameObject.Find("Game").GetComponent<Player>().HHChildren == 0 || GameObject.Find("Game").GetComponent<Player>().HHIncome == 0)
        {
            GameObject.FindGameObjectWithTag("Stuffed Toy 1").SetActive(false);
            GameObject.FindGameObjectWithTag("Stuffed Toy 2").SetActive(false);
        }

        else if (GameObject.Find("Game").GetComponent<Player>().HHChildren > 0 && GameObject.Find("Game").GetComponent<Player>().HHIncome == 1)
        {
            GameObject.FindGameObjectWithTag("Stuffed Toy 1").SetActive(true);
            // if more than 1 child in household, activate the other toy
            if (GameObject.Find("Game").GetComponent<Player>().HHChildren > 1) GameObject.FindGameObjectWithTag("Stuffed Toy 2").SetActive(true);
            else
            {
                if (GameObject.FindGameObjectWithTag("Stuffed Toy 2").activeSelf) GameObject.FindGameObjectWithTag("Stuffed Toy 2").SetActive(false);
            }
        }

        // destroy all those in player backpack if game is loaded
        if (!destroyed && GameObject.Find("Game").GetComponent<Launcher>().mode == "Load" && GameObject.Find("Game").GetComponent<Player>().backpackItems != null && GameObject.Find("Game").GetComponent<Player>().backpackItems.Count > 0)
        {

            GameObject toDestroy;
            foreach (KeyValuePair<string, string> item in GameObject.Find("Game").GetComponent<Player>().backpackItems)
            {
                // find gameobject using its tag
                toDestroy = GameObject.FindGameObjectWithTag(item.Key);

                // check if object is already completed
                if (toDestroy != null)
                {
                    Debug.Log("Item to destroy: " + toDestroy.name);
                    // check checklist if value == 0; if yes, "destroy"
                    if (checklist != null)
                    {
                        Debug.Log("Count in checklist: " + checklist[toDestroy.name]);
                        if (checklist[toDestroy.name] == 0)
                        {
                            toDestroy.SetActive(false);
                            Debug.Log("*\"Destroyed\" " + toDestroy.name);
                            destroyed = true;
                        }
                        
                    }

                    else
                    {
                        Debug.Log("Count in checklist: " + GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().checklist[toDestroy.name]);
                        if (GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().checklist[toDestroy.name] == 0)
                        {
                            toDestroy.SetActive(false);
                            Debug.Log("\"Destroyed\" " + toDestroy.name);
                            destroyed = true;
                        }
                    }

                }
            }

            
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (checklist != null) AdaptivityFunction();
        // destroy all those in player backpack if game is loaded
        if (!destroyed && GameObject.Find("Game").GetComponent<Launcher>().mode == "Load" && GameObject.Find("Game").GetComponent<Player>().backpackItems != null && GameObject.Find("Game").GetComponent<Player>().backpackItems.Count > 0)
        {

            GameObject toDestroy;
            foreach (KeyValuePair<string, string> item in GameObject.Find("Game").GetComponent<Player>().backpackItems)
            {
                // find gameobject using its tag
                toDestroy = GameObject.FindGameObjectWithTag(item.Key);

                // check if object is already completed
                if (toDestroy != null)
                {
                    Debug.Log("Item to destroy: " + toDestroy.name);
                    // check checklist if value == 0; if yes, "destroy"
                    if (checklist != null)
                    {
                        Debug.Log("Count in checklist: " + checklist[toDestroy.name]);
                        if (checklist[toDestroy.name] == 0)
                        {
                            toDestroy.SetActive(false);
                            Debug.Log("*\"Destroyed\" " + toDestroy.name);
                            destroyed = true;
                        }

                    }

                    else
                    {
                        Debug.Log("Count in checklist: " + GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().checklist[toDestroy.name]);
                        if (GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().checklist[toDestroy.name] == 0)
                        {
                            toDestroy.SetActive(false);
                            Debug.Log("\"Destroyed\" " + toDestroy.name);
                            destroyed = true;
                        }
                    }

                }
            }


        }

        if (total == 0)
        {
            total = GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().GetTotal();
        }

    }

    // Get percentage done
    float PercentageDone()
    {
        progress = 0;
        foreach (int item in checklist.Values)
        {
            progress += item;
        }

        return (100 - (progress / total * 100));
    }

    // rearranges checklist
    void AdaptivityFunction()
    {
        int timeRemaining = GameObject.Find("Game").GetComponent<BackgroundTimer>().timeRemaining;

        // if 8 minutes have passed 
        if (GameObject.Find("Game").GetComponent<BackgroundTimer>().shouldCountdown && timeRemaining <= threshold)
        {
            Debug.Log(string.Format("Time remaining less than 9 minutes! Current percentage: {0}", PercentageDone()));
            // check for percentage done
            Debug.Log(string.Format("Percentage done: {0}", PercentageDone()));

            // rearrange checklist if player isn't pass 60% and is "doing well", with negligible mistakes
            if (PercentageDone() < 60 && GameObject.Find("Player").GetComponent<Adaptivity>().incorrectTries <= 1)
            {
                if (debug) Debug.Log("Rearranging checklist! (DEBUG MODE)");
                else Debug.Log("Rearranging checklist! (REAL MODE)");
                GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().RearrangeChecklist();
            }
        }

    }

    // called when sofa is clicked; for debugging only!
    public void TemporaryDebugger()
    {
        StartCoroutine(EndGame());

    }

    public void EndSuppliesKitGame()
    {
        seconds = 0;
        StartCoroutine(EndGame());

    }

    public IEnumerator EndGame()
    {
        while (true)
        {
            Debug.Log("Ending Before Typhoon scene!");
            if (seconds < 6)
            {
                seconds++;
                yield return new WaitForSeconds(1);
            }

            else
            {
                // SAVE GAME HERE
                if (GameObject.Find("Game") != null && GameObject.Find("Game").GetComponent<Launcher>().saveMode)
                {
                    if (GameObject.Find("Subtext").GetComponent<ChecklistUpdate>().gameDone) GameObject.Find("Game").GetComponent<Player>().finishedBefore = true;
                    GameObject.Find("Game").GetComponent<Player>().SavePlayer();
                }
                
                Debug.Log("Changing scene!");
                GameObject.Find("Game").GetComponent<SceneController>().ChangeScene("bahay_interior");
                seconds = 0;
                yield break;
            }
        }
    }

    public void PlayCorrect()
    {
        correct.Play();
    }

    public void PlayCompleted()
    {
        completed.Play();
    }
}
