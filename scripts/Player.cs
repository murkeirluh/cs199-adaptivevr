using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 
 * Player.cs
 * ------------
 * This script is attached to the Game object.
 * 
 * Stores important player info and personalization variables.
 * Also responsible for saving and loading player.
 * 
 */

public class Player : MonoBehaviour
{

    /* ALL RELEVANT VARIABLES THAT WILL BE SAVED */
    public int playerID;
    public Vector3 worldPosition;
    public IDictionary<string, int> checklist;

    // Dictionary<GameObject.Find("Player").name, GameObject.Find("Player").tag>
    // Dictionary key = object.tag, value = object.name
    public Dictionary<string, string> backpackItems;
    public int incorrectTries;
    public string currentModule;
    public bool finishedIntroTV, finishedNPC, finishedBefore;

    // PERSONALIZATION VARIABLES
    // HHIncome (poor: 0, well-off: 1) * for simplification
    public int HHNumber, HHNeedMeds, HHChildren, HHIncome;
    public bool hasLPG, hasElectricity;
    public int foodDuplicate, waterDuplicate, toysDuplicate, medsDuplicate;
    public int foodCount, waterCount, toysCount, medsCount;
    /* END RELEVANT VARIABLES */

    // for checking if player info has been assigned
    public bool playerSet;

    Transform checklistObject;
   
    // Start is called before the first frame update
    void Start()
    {

        backpackItems = new Dictionary<string, string>();
        playerSet = false;
        checklist = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if playerID is not set
        if (playerID == 0) SetPlayerID(GameObject.Find("Game").GetComponent<Launcher>().idNumber);
    }

    // sets playerID to `id`
    public void SetPlayerID(int id)
    {
        playerID = id;

    }

    // called from PutBackpack; adds to list before destroying object
    public void AddToPlayerBackpack(GameObject item)
    {
        backpackItems[item.tag] = item.name;
        Debug.Log("Added to bag: " + item.tag);
    }

    // initializes checklist for user
    public void FixChecklist()
    {
        Debug.Log("Entered FixChecklist");

        if (SceneManager.GetActiveScene().name == "beforetyphoon")
        {
            Debug.Log("beforetyphoon!");

            // Checklist
            Debug.Log("Checklist is null; putting stuff in it:");
            checklistObject = GameObject.Find("Subtext").transform;
            checklist = checklistObject.GetComponent<ChecklistUpdate>().ReturnDefaultChecklist();

            if (backpackItems != null && backpackItems.Count > 0)
            {
                // search backpack and subtract from checklist item count
                foreach (KeyValuePair<string, string> item in backpackItems)
                {
                    Debug.Log("Backpack key: " + item.Key);
                    Debug.Log("Backpack value: " + item.Value);

                    // check if item is in checklist
                    if (checklist.ContainsKey(item.Value))
                    {
                        switch (item.Value)
                        {
                            case "Water Bottle":
                                checklist[item.Value] -= waterCount;
                                break;

                            case "Canned Good":
                                checklist[item.Value] -= foodCount;
                                break;

                            case "Stuffed toy":
                                checklist[item.Value] -= toysCount;
                                break;

                            case "Medicine":
                                checklist[item.Value] -= medsCount;
                                break;

                            default:
                                checklist[item.Value]--;
                                break;

                        }

                    }

                }
            }
        }
    }

    // saves the current player
    public void SavePlayer()
    {
        // assign variables
        worldPosition = GameObject.Find("Player").transform.position;
        currentModule = GameObject.Find("Game").GetComponent<Launcher>().moduleName;
        incorrectTries = (GameObject.Find("Player").GetComponent<Adaptivity>() != null ? GameObject.Find("Player").GetComponent<Adaptivity>().incorrectTries : 0);

        // then save the player
        SaveSystem.SavePlayer(this);

    }

    // loads player with playerID `id`
    public void LoadPlayer(string id)
    {
        PlayerData data = SaveSystem.LoadPlayer(id);
        Debug.Log("====PLAYER LOADED:====");
        Debug.Log("Player ID: " + data.playerID);

        // load variables
        // PlayerID
        playerID = int.Parse(data.playerID.ToString());

        currentModule = data.currentModule;
        incorrectTries = data.incorrectTries;
        finishedIntroTV = (data.finishedIntroTV == 1);
        finishedNPC = (data.finishedNPC == 1);
        finishedBefore = (data.finishedBefore == 1);

        // Player worldPosition
        Vector3 position;
        position.x = data.worldPosition[0];
        position.y = data.worldPosition[1];
        position.z = data.worldPosition[2];
        if (position != Vector3.zero)
        {
            GameObject.Find("Player").transform.position = position;
        }

        // assigns loaded variables to in-game tracked variables
        HHNumber = data.HHNumber;
        HHNeedMeds = data.HHNeedMeds;
        HHChildren = data.HHChildren;
        HHIncome = data.HHIncome;
        hasLPG = (data.hasLPG == 1);
        hasElectricity = (data.hasElectricity == 1);
        foodDuplicate = data.foodDuplicate;
        waterDuplicate = data.waterDuplicate;
        toysDuplicate = data.toysDuplicate;
        medsDuplicate = data.medsDuplicate;
        foodCount = data.foodCount;
        waterCount = data.waterCount;
        toysCount = data.toysCount;
        medsCount = data.medsCount;

        for (int i = 0; i < data.backpackItems.Length; i++)
        {
            backpackItems[data.backpackItemTags[i]] = data.backpackItems[i];
            Debug.Log(backpackItems[data.backpackItemTags[i]]);
        }

        // based on items in backpack, fix the checklist
        FixChecklist();
        playerSet = true;
    }

    // checks if backpack or checklist is null
    public bool NullChecker()
    {
        return (backpackItems != null && checklist != null);
    }
}