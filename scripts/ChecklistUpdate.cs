using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ChecklistUpdate.cs
 * ------------
 * This script updates the checklist display text.
 * This script is also responsible for the rearrangement of the
 * checklist items.
 * 
 */


public class ChecklistUpdate : MonoBehaviour
{
    // the main checklist variable
    public IDictionary<string, int> checklist;

    // lists all original transforms of gameobjects; for reinstantation
    public IDictionary<string, Transform> transformDict;
    public IDictionary<string, Vector3> posDict;
    public IDictionary<string, Quaternion> rot8Dict;

    // the checklist text
    private Text thisText;
    public Text headerText;
    private string concat, mode;

    // total number of items needed to be collected
    public int total;
    // total number of items user has collected divided by total number of objects in room
    float progress;

    // defaultChecklist: if user is using the default (i.e. not rearranged)
    // gameDone: if the game is done or not
    public bool defaultChecklist, gameDone;
    private SortedList<float, string> rearrangedChecklist;

    // the Player gameobject
    public GameObject player;
    // the hint/popup gameobject
    private GameObject hintObject, hintText;

    void Awake()
    {
        // initialize default checklist
        defaultChecklist = true;
        // check game mode
        mode = GameObject.Find("Game").GetComponent<Launcher>().GetMode();

        // if the game is a new game, 
        if (mode == "New")
        {
            // generate the default checklist
            checklist = new Dictionary<string, int>()
            {
                {"Radio", 1},
                {"Water Bottle", 2},
                {"Flashlight", 1},
                {"Canned Good", 5},
                {"Batteries", 2},
                {"Spare Batteries", 2},
                {"First Aid Kit", 1}

            };

            // personalization; add other supplies if needed
            if (GameObject.Find("Game").GetComponent<Player>().HHNumber > 1)
            {
                checklist["Water Bottle"] *= GameObject.Find("Game").GetComponent<Player>().HHNumber;
                checklist["Canned Good"] *= GameObject.Find("Game").GetComponent<Player>().HHNumber;
            }

            // if there are children and family is well-off, add stuffed toys
            if (GameObject.Find("Game").GetComponent<Player>().HHChildren > 0 && GameObject.Find("Game").GetComponent<Player>().HHIncome == 1)
            {
                checklist["Stuffed toy"] = GameObject.Find("Game").GetComponent<Player>().HHChildren;
            }

            // if hhmeds
            if (GameObject.Find("Game").GetComponent<Player>().HHNeedMeds > 0)
            {
                checklist["Medicine"] = GameObject.Find("Game").GetComponent<Player>().HHNeedMeds;
            }

            //GameObject.Find("Game").GetComponent<Player>().foodCount = checklist["Canned Good"];
            //GameObject.Find("Game").GetComponent<Player>().waterCount = checklist["Water Bottle"];
            //GameObject.Find("Game").GetComponent<Player>().medsCount = checklist["Medicine"];
            //GameObject.Find("Game").GetComponent<Player>().toysCount = checklist["Stuffed toy"];

            // assign the text of the checklist to thisText
            thisText = GetComponent<Text>();
            defaultChecklist = true;
            total = GetTotal();
        }

        // if the game is a loaded game, retrieve checklist from saved file
        else if (mode == "Load")
        {
            GameObject.Find("Game").GetComponent<Player>().FixChecklist();
            checklist = GameObject.Find("Game").GetComponent<Player>().checklist;
            thisText = GetComponent<Text>();
            total = GetTotal();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        // get init transforms of objects
        transformDict = new Dictionary<string, Transform>();
        posDict = new Dictionary<string, Vector3>();
        rot8Dict = new Dictionary<string, Quaternion>();
        GameObject tempObject;

        // get the Transforms of the supplies for respawning
        for (int i = 0; i < GameObject.Find("Supplies").transform.childCount; i++) {
            tempObject = GameObject.Find("Supplies").transform.GetChild(i).gameObject;
            transformDict[tempObject.tag] = tempObject.transform;
            posDict[tempObject.tag] = tempObject.transform.localPosition;
            rot8Dict[tempObject.tag] = tempObject.transform.localRotation;
            //Debug.Log(tempObject.tag + " position: " + tempObject.transform.position);
            //Debug.Log(tempObject.tag + " rotation: " + tempObject.transform.rotation);
        }

        if (checklist != null)
        {
            // update checklist UI
            foreach (KeyValuePair<string, int> item in checklist)
            {
                if (item.Value == 0)
                {
                    concat = concat + item.Key + " ✓\n";
                }
                else
                {
                    concat = concat + item.Key + " (" + item.Value + ")\n";
                }
            }
            thisText.text = concat;
            concat = "";
        }

        hintObject = GameObject.Find("Player/Main Camera/UI/Hint");
        hintText = GameObject.Find("Player/Main Camera/UI/Hint/Button/Text");

        concat = "";

        rearrangedChecklist = new SortedList<float, string>();
        gameDone = false;
        progress = 0;


    }

    // Update is called once per frame
    void Update()
    {

        if (checklist == null && mode == "Load")
        {
            GameObject.Find("Game").GetComponent<Player>().FixChecklist();
            checklist = GameObject.Find("Game").GetComponent<Player>().checklist;

        }

        // check for user progress
        GetProgress();

        // project checklist according to idictionary
        if (defaultChecklist && !gameDone)
        {
            foreach (KeyValuePair<string, int> item in checklist)
            {
                if (item.Value == 0)
                {
                    concat = concat + item.Key + " ✓\n";

                }
                else
                {
                    concat = concat + item.Key + " (" + item.Value + ")\n";
                }
            }
            thisText.text = concat;
            concat = "";
        }

        // project rearranged checklist
        else if (!defaultChecklist && !gameDone)
        {
            CheckRearrangedChecklist();

            headerText.text = "Checklist*";
            foreach (KeyValuePair<float, string> item in rearrangedChecklist)
            {
                if (checklist[item.Value] == 0)
                {
                    concat = concat + item.Value + " ✓\n";
                }
                else
                {
                    concat = concat + item.Value + " (" + checklist[item.Value] + ")\n";
                }
            }
            thisText.text = concat;
            concat = "";

        }

        // check if game has been completed (changed from GetProgress())
        if (gameDone)
        {
            // play completed sound
            GameObject.Find("Supplies").GetComponent<SuppliesKitGame>().PlayCompleted();
            headerText.text = "Congratulations!";
            thisText.text = "You finished assembling your supplies kit!";
            GameObject.Find("Supplies").GetComponent<SuppliesKitGame>().EndSuppliesKitGame();
        }


    }

    // checks for user's progress with the checklist
    void GetProgress()
    {
        float percentage = PercentageDone();
        Debug.Log("Percentage: " + percentage);
        if (percentage >= 100.0f)
        {
            gameDone = true;
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

    // responsible for rearranging checklist
    public void RearrangeChecklist()
    {
        if (defaultChecklist)
        {
            float dist = float.Epsilon;
            float d = 0.0f;
            int totalNumber;

            Vector3 playerLocation;
            GameObject currentObject;

            playerLocation = player.transform.position;
            //Debug.Log(string.Format("Player location: {0}", playerLocation));

            foreach (KeyValuePair<string, int> item in checklist)
            {

                d = 0.0f;

                Debug.Log(string.Format("CURRENT ITEM: {0}", item.Key));

                // if there is only one instance of that object,
                if (item.Value == 1 || item.Key == "Medicine")
                {
                    // get instance of gameobject
                    currentObject = GameObject.Find(item.Key);
                    // get the distance between player and object
                    dist = Vector3.Distance(playerLocation, currentObject.transform.position);
                    rearrangedChecklist[dist] = currentObject.name;
                    totalNumber = item.Value;
                }

                else if (item.Value > 1)
                {
                    // get "real" total number of object
                    switch (item.Key)
                    {
                        case "Water Bottle":
                            totalNumber = 2;
                            break;
                        case "Stuffed toy":
                            totalNumber = GameObject.Find("Game").GetComponent<Player>().HHChildren > 1 ? 2 : 1;
                            break;
                        case "Canned Good":
                            totalNumber = 5;
                            break;
                        case "Medicine":
                            totalNumber = 1;
                            break;
                        default: 
                            totalNumber = item.Value;
                        break;

                    }
                    
                    for (int i = 1; i < totalNumber + 1; i++)
                    {
                        Debug.Log(string.Format("{0} {1}", item.Key, i));
                        currentObject = GameObject.FindWithTag(string.Format("{0} {1}", item.Key, i));
                        // sum up all distances and get average
                        d += Vector3.Distance(playerLocation, currentObject.transform.position);

                        // get the nearest instance
                        //if (dist <= float.Epsilon || (dist > float.Epsilon && d < dist)) dist = d;
                    }

                    // get average of distances
                    d = System.Math.Abs(d / totalNumber);

                    // assign to rearranged checklist
                    rearrangedChecklist[d] = item.Key;
                }


            }

            // show hint that checklist has been rearranged
            Debug.Log("REARRANGED CHECKLIST:");
            hintText.GetComponent<Text>().text = "Your checklist has been rearranged.\n Try prioritizing the first few items on the list.";
            hintObject.SetActive(true);

            //rearrangedChecklist debug
            foreach (KeyValuePair<float, string> it in rearrangedChecklist)
            {
                Debug.Log(it);
            }

            // using rearranged checklist now; mark as false
            defaultChecklist = false;
        }
    }

    // check if game can be completed if first few items  
    // in rearranged checklist has been found 
    public void CheckRearrangedChecklist()
    {

        int count = 0;
        string key;
        // if the checklist is rearranged
        if (!defaultChecklist)
        {

            IList<string> vals = rearrangedChecklist.Values;
            // check if first three items are done
            for (int i = 0; i < 4; i++)
            {
                key = vals[i];

                // check if current item is already in bag
                if (checklist[key] == 0)
                {
                    count++;
                }
            }

            if (count == 3)
            {
                gameDone = true;

            }

        }

    }

    // return the rearrangedchecklist
    public IDictionary<string, int> GetRearrangedChecklist()
    {
        if (!defaultChecklist)
        {

            IList<string> keys = new List<string>();
            IList<int> values = new List<int>();

            keys = rearrangedChecklist.Values;
            for (int i = 0; i < keys.Count; i++)
            {
                values.Add(checklist[keys[i]]);

            }

            IDictionary<string, int> rearrangedC = new Dictionary<string, int>();
            for (int i = 0; i < keys.Count; i++)
            {
                rearrangedC[keys[i]] = values[i];

            }

            return rearrangedC;
        }

        return checklist;
    }


    // generates the default checklist
    public IDictionary<string,int> ReturnDefaultChecklist()
    {
        Player playerObject = GameObject.Find("Game").GetComponent<Player>();
        IDictionary<string, int> newChecklist = new Dictionary<string, int>()
            {
                {"Radio", 1},
                {"Water Bottle", 2 * playerObject.HHNumber},
                {"Flashlight", 1},
                {"Canned Good", 5 * playerObject.HHNumber},
                {"Batteries", 2},
                {"Spare Batteries", 2},
                {"First Aid Kit", 1}

            };

        if (playerObject.HHChildren * playerObject.HHIncome > 0)
        {
            newChecklist["Stuffed toy"] = playerObject.HHChildren * playerObject.HHIncome;

        }

        if (playerObject.HHNeedMeds > 0)
        {
            newChecklist["Medicine"] = playerObject.HHNeedMeds;

        }

        return newChecklist;

    }

    // computes for the total of the items (wrt personalization variables)
    public int GetTotal(string item = null)
    {
        Player playerObject = GameObject.Find("Game").GetComponent<Player>();
        int totalNumber = 0;

        switch (item)
        {
            case "Water Bottle":
                return 2 * playerObject.HHNumber;

            case "Canned Good":
                return 5 * playerObject.HHNumber;

            case "Toys":
                return playerObject.HHChildren * playerObject.HHIncome;

            case "Stuffed toy":
                return playerObject.HHChildren * playerObject.HHIncome;

            case "Medicine":
                return playerObject.HHNeedMeds;
            
            // if no specific item was passed as argument, return total number of checklist items
            default:
                totalNumber = 0;
                // food
                totalNumber += 5 * playerObject.HHNumber;
                // water
                totalNumber += 2 * playerObject.HHNumber;
                // flashlight, radio, batteries x4,first aid kit
                totalNumber += 7;
                // toys
                totalNumber += playerObject.HHChildren * playerObject.HHIncome;
                // meds
                totalNumber += playerObject.HHNeedMeds;

                return totalNumber;

        }

    }
}



