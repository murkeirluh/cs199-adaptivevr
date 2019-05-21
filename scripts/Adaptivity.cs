using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Adaptivity.cs
 * ------------
 * This script handles the showing of hints and the adaptive 
 * timer that ticks after user's last successful tap.
 * Also stored here is the user's counter for incorrect items
 * and decisions made.
 * 
 * Attached to the Player object.
 * 
 */

public class Adaptivity : MonoBehaviour
{
    // to execute some code blocks under Update() every second instead of every frame
    float interval = 1.0f;
    float nextTime = 0;

    // constants; used for dictionaries
    public const int INACTIVE = 0;
    public const int INCORRECT = -1;
    private const int RANDOM = 7;
    private const int DESC = 9;

    // supplies constants; for explanation/popups
    private const int WATER = 20;
    private const int FOOD = 21;
    private const int RADIO = 22;
    private const int FAK = 23;
    private const int FLASH = 24;
    private const int BATT = 25;
    private const int MEDS = 26;
    private const int TOYS = 27;
    private const int EXT_BATT = 28;

    // supplies dictionary: mapping of constants to the object name
    public IDictionary<int, string> suppliesDict = new Dictionary<int, string>() {
        {WATER, "Water Bottle"},
        {FOOD, "Canned Good"},
        {RADIO, "Radio"},
        {FAK, "First Aid Kit"},
        {FLASH, "Flashlight"},
        {BATT, "Batteries"},
        {MEDS, "Medicine"},
        {TOYS, "Stuffed toy"},
        {EXT_BATT, "Spare Batteries"}
    };

    // time elapsed since last successful click
    //public int timeElapsedSUC = 0;
    // time elapsed while inactive
    public int timeElapsedINA = 0;

    // in seconds; upper bound time before hints are shown (default: 30) 
    public int threshold = 33;
    // upper bound number of incorrect decisions made
    public int incorrectThreshold = 3;

    // indicates if inactivity timer is running
    public bool isTicking;

    // counter for incorrect items/decisions made by user
    public int incorrectTries;

    // if player is walking
    bool isWalking;

    // seconds ticked 
    int seconds;

    // hints
    private GameObject hintObject, hintText;
    // (During) current event user is in -- affects the hint shown pagtapos ng several tries
    public string eventRunning;

    // list of hints for inactivity
    // texts 0-1: hints for walking
    // texts 2-4: hints for Before Before Module
    private List<string> inactivityHintTexts = new List<string>() {
        "Hey. You haven't been moving for a while now. \n\nTo walk around, look an angle down towards the floor,\nand click on the gray tile that pops out.",
        "Try looking down until you see a gray tile pop out,\nand then click on it.",
        "There is a checklist on the wall. Try finding the listed items\naround the house and put them on the bag next to the checklist.",
        "You are building an Emergency Preparedness Kit.\nUse the checklist on the wall as your guide.",
        "Find the items on the checklist around the house.",
    };

    // list of hints of incorrect moves
    private List<string> incorrectHintTexts = new List<string>() {
        "You have been putting unnecessary items in your backpack.\nPlease refer to the checklist as your guide.",
        "These things are unnecessary in preparation for a typhoon.\nTry getting the items from the checklist.",
        // Incorrect During
        "The power line electrified the water puddles\non the ground. Consider another way.",
        "You fell because you had no way of telling the depth and stability\nof the ground below, or if there are any manholes.",
        "As little as 12 inches of moving water can make one fall,\nand greater depths can carry people and cars away.",
        "Leaving children to play in floodwater leaves them at\nrisk of illness, drowning or injury."
    };


    // random hints
    private List<string> randomHints = new List<string>() {
        "You can place objects on another surface by clicking\non any surface while holding an object.",
        "You can hover on any object to find out what it is.",
        "Try interacting with the highlighted\nobjects in the room."

    };

    // dictionary of explanations for supplies
    public IDictionary<string, string> supplyDesc = new Dictionary<string, string>() {
        {"Water Bottle", "Water:\nOne gallon (~ 4 liters) per person, per day\n(for at least 3 days)."},
        {"Canned Good", "Food:\nNon-perishable, easy-to-prepare food items like canned goods\n (to last at least 3 days)."},
        {"Radio", "Radio:\nMust be battery-powered, to serve as a source of\ninformation even when there is no access to electricity."},
        {"First Aid Kit", "First Aid Kit:\nTo be able to treat most common minor injuries and\nillnesses that may occur during evacuation."},
        {"Flashlight", "Flashlight:\nTo be able to see in the dark,\nin case there is no access to electricity."},
        {"Batteries", "Batteries:\nTo power your devices."},
        {"Stuffed toy", "Toys:\nAdditional items to keep children of the household\nentertained, or to provide comfort." },
        {"Medicine", "Medicines:\nPrescription and/or maintenance medication." },
        {"Spare Batteries", "Spare batteries:\nExtra batteries for backup."}
    };


    // indices corresponding to hint categories in list
    const int INA_MIN = 0;
    const int INA_MAX = 1;

    const int BEFMOD_MIN = 2;
    const int BEFMOD_MAX = 4;

    // incorrect hints
    const int INC_MIN = 0;
    const int INC_MAX = 1;

    // during incorrect hints

    // random hints
    const int RAND_MIN = 0;
    const int RAND_MAX = 1;


    // Use this for initialization
    void Start()
    {
        isTicking = false;
        seconds = 0;

        if (GameObject.Find("Game") != null && GameObject.Find("Game").GetComponent<Launcher>().moduleName != "Main Menu")
        {
            hintObject = GameObject.Find("Player/Main Camera/UI/Hint");
            hintText = GameObject.Find("Player/Main Camera/UI/Hint/Button/Text");
            hintObject.SetActive(false);
            //Debug.Log("Hint object is here");
        }
    }

    // checks for inactivity
    void Update()
    {
        if (GameObject.Find("Game") != null && GameObject.Find("Game").GetComponent<Launcher>().moduleName != "Main Menu")
        {
            // sets hint object if it is null
            if (hintObject == null)
            {
                hintObject = GameObject.Find("Player/Main Camera/UI/Hint");
                hintText = GameObject.Find("Player/Main Camera/UI/Hint/Button/Text");
                hintObject.SetActive(false);
                //Debug.Log("Hint object is here");
            }
        }

        // checks if user is currently walking
        isWalking = gameObject.GetComponent<Walk>().isWalking;

        if (Time.time >= nextTime) 
        {   
            // checks if current module isn't main menu
            if (GameObject.Find("Game") != null && GameObject.Find("Game").GetComponent<Launcher>().moduleName != "Main Menu")
            {
                if (!isWalking && !isTicking) RunTimer();
                else if (isWalking && isTicking) ResetTimer();

                // generate random hints
                int ran = Random.Range(1, 700);
                if ((ran % RANDOM == 0 && ran % 5 == 0) && seconds == 0) ShowHint(RANDOM);
                
            }

            else
            {
                if (isTicking) ResetTimer();
            }

            nextTime += interval;
        }

        
    }


// Inactivity timer
public IEnumerator CountDown()
{
    while (true)
    {
        Debug.Log("Inactivity timer running");

        // increment if elapsed time is below upper bound
        if (timeElapsedINA < threshold && isTicking)
        {
            timeElapsedINA++;
            yield return new WaitForSeconds(1);
        }
        else if (timeElapsedINA >= threshold)
        {
            ShowHint(INACTIVE);
            yield break;
        }

        else if (!isTicking)
        {
            yield break;
        }

    } 
}

// invokes CountDown timer
public void RunTimer()
{
    if (!isTicking && GameObject.Find("Game") != null && GameObject.Find("Game").GetComponent<Launcher>().moduleName != "Main Menu")
    {
        isTicking = true;
        StartCoroutine("CountDown");
    }

}

// resets inactivity timer and deactivates hint object
    public void ResetTimer()
{
    StopCoroutine("CountDown");
    seconds = 0;
    isTicking = false;
    timeElapsedINA = 0;
    hintObject.SetActive(false);
}

// resets timer that shows hint and deactivates hint object
    public void ResetHint()
{
    Debug.Log("Reset hint!");
    StopCoroutine("ShowHintUI");
    seconds = 0;
    hintObject.SetActive(false);

}

// increments counter for incorrect tries
// SHOULD be called from scripts where decisions can be made
public void WrongMove()
{
    incorrectTries++;
        // play wrong sound
        GameObject.Find("Game").GetComponent<BGSfxManager>().PlaySFX(3);
    Debug.Log(string.Format("WRONG OBJECT; incorrect: {0}", incorrectTries));
    if (incorrectTries >= incorrectThreshold) ShowHint(INCORRECT);
}

       
public IEnumerator ShowHintUI(int purpose)
{
    while (true)
    {
        if (purpose == INCORRECT)
        {
            Debug.Log("Too many incorrect tries! Here's a hint.");
            if (hintObject != null && !hintObject.activeSelf) hintObject.SetActive(true);
            incorrectTries = 0;

            // wait for 4 seconds before removing hint
            if (seconds < 5)
            {
                Debug.Log(string.Format("countdown {0} INCORRECT HINT", seconds));
                seconds++;
                yield return new WaitForSeconds(1);
            }
            else
            {
                ResetHint();
                yield break;
            }

        }

        if (purpose == INACTIVE)
        {
            if (!isTicking) yield break;
            if (hintObject != null && !hintObject.activeSelf) hintObject.SetActive(true);
            Debug.Log("Inactive hint is up");
            // wait for 4 seconds before removing hint
            if (seconds < 5)
            {
                Debug.Log(string.Format("countdown {0} INACTIVE HINT", seconds));
                seconds++;
                yield return new WaitForSeconds(1);
            }
            else
            {
                ResetTimer();
                yield break;
            }
        }

        if (purpose == RANDOM)
        {
            Debug.Log("Random hint is up");
            if (hintObject != null && !hintObject.activeSelf) hintObject.SetActive(true);
            // wait for 4 seconds before removing hint
            if (seconds < 5)
            {
                Debug.Log(string.Format("countdown {0} RANDOM HINT", seconds));
                seconds++;
                yield return new WaitForSeconds(1);
            }
            else
            {
                ResetHint();
                yield break;
            }
        }

         if (purpose == DESC)
        {
            //Debug.Log("desc!!!!");
            if (hintObject != null && !hintObject.activeSelf) hintObject.SetActive(true);
            // wait for 4 seconds before removing hint
            if (seconds < 10)
            {
                //Debug.Log(string.Format("countdown {0} OBJECT DESC", seconds));
                seconds++;
                    Debug.Log("DESC seconds: " + seconds);
                yield return new WaitForSeconds(1);
            }
            else
            {
                ResetHint();
                yield break;
            }
        }
        }

}


// Sets appropriate text for hint popup, then calls function to show popup
public void ShowHint(int purpose)
{
    // if there are no currently active hints
    if (!hintObject.activeSelf) {
        // get active scene name
        string m_Scene = SceneManager.GetActiveScene().name;

        switch (purpose)
        { 
            case INCORRECT:
                //Debug.Log("Too many incorrect tries! Here's a hint.");
                if (m_Scene == "evacuation")
                {
                    // Add recognition of hazard
                    if (eventRunning == "Electric")
                    {
                        hintText.GetComponent<Text>().text = incorrectHintTexts[2];
                    }
                    else if (eventRunning == "Fall")
                    {
                        hintText.GetComponent<Text>().text = incorrectHintTexts[3];
                        Debug.Log(incorrectHintTexts[3]);
                    }
                    else if (eventRunning == "Moving")
                    {
                        hintText.GetComponent<Text>().text = incorrectHintTexts[4];
                        Debug.Log(incorrectHintTexts[4]);

                    }
                    else if (eventRunning == "Child")
                    {
                        hintText.GetComponent<Text>().text = incorrectHintTexts[5];
                        Debug.Log(incorrectHintTexts[4]);

                    }
                }
                else if (m_Scene == "beforetyphoon") {
                    hintText.GetComponent<Text>().text = incorrectHintTexts[Random.Range(INC_MIN, INC_MAX + 1)];
                }

                StartCoroutine(ShowHintUI(INCORRECT));
                break;

            case INACTIVE:
                Debug.Log("You too inactive gurl");
                if (m_Scene != "bahay_interior") {
                    // get Before module hints
                    if (m_Scene == "evacuation")
                    {
                        hintText.GetComponent<Text>().text = inactivityHintTexts[Random.Range(INA_MIN, INA_MAX + 1)];
                    }
                    else if (m_Scene == "beforetyphoon")
                    {
                        hintText.GetComponent<Text>().text = inactivityHintTexts[Random.Range(BEFMOD_MIN, BEFMOD_MAX + 1)];
                    }
                    StartCoroutine(ShowHintUI(INACTIVE));
                }
                // add something dito, how to find our scene number?
                break;

            // random hints
            default:
                if (m_Scene != "evacuation" && m_Scene != "beforetyphoon" && m_Scene != "introduction")
                {
                    if (m_Scene == "beforetyphoon")
                    {
                        hintText.GetComponent<Text>().text = randomHints[Random.Range(0, 1)];
                    }

                    else if (m_Scene == "bahay_interior") {
                        hintText.GetComponent<Text>().text = randomHints[2];
                    }

                    StartCoroutine(ShowHintUI(RANDOM));
                }

                else if (m_Scene == "beforetyphoon" && suppliesDict.ContainsKey(purpose))
                {
                    if (supplyDesc.ContainsKey(suppliesDict[purpose]))
                    {
                        hintText.GetComponent<Text>().text = supplyDesc[suppliesDict[purpose]];
                        Debug.Log(hintText.GetComponent<Text>().text);
                        StartCoroutine(ShowHintUI(DESC));

                    }

                }
                break;
        }


        }
    }


}