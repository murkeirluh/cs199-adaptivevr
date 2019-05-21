using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * PutBackpack.cs
 * ------------
 * This script is responsible for putting stuff in the backpack and
 * placing items in the environment.
 * 
 * This also updates the checklist dictionary and handles the
 * respawning of the supplies in the room.
 * 
 */

public class PutBackpack : MonoBehaviour
{
    // to access checklist, subtract count when placed
    public GameObject checklistDictMule; 
    string nameToDestroy;
    // stores the object the reticle is pointing to
    GameObject objPointed;
    Transform initParent;
    IDictionary<string, int> checklist;
    bool isWalking, reinstantiated;

    const int INACTIVE = 0;

    // dictionary for hints
    public IDictionary<string, int> suppliesDict2 = new Dictionary<string, int>() {
        {"Water Bottle", WATER},
        {"Canned Good", FOOD},
        {"Radio", RADIO},
        {"First Aid Kit", FAK},
        {"Flashlight", FLASH},
        {"Batteries", BATT},
        {"Spare Batteries", EXT_BATT},
        {"Medicine", MEDS },
        {"Stuffed toy", TOYS }
    };

    // dictionary of objects' transforms
    IDictionary<string, Transform> transformDict;
    IDictionary<string, Vector3> posDict;
    IDictionary<string, Quaternion> rot8Dict;
    
    // constants; used for dictionary of hints
    // these variables are also in Adaptivity.cs
    private const int WATER = 20;
    private const int FOOD = 21;
    private const int RADIO = 22;
    private const int FAK = 23;
    private const int FLASH = 24;
    private const int BATT = 25;
    private const int MEDS = 26;
    private const int TOYS = 27;
    private const int EXT_BATT = 28;

    // Start is called before the first frame update
    void Start()
    {
        
        if (checklistDictMule != null && GameObject.Find("Game").GetComponent<Launcher>().moduleName == "Before")
        {
            // initialize checklist variable
            checklist = checklistDictMule.GetComponent<ChecklistUpdate>().checklist;
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // if the current scene is the supplies kit game scene
        if (SceneManager.GetActiveScene().name == "beforetyphoon")
        {
            // check if checklist variable is assigned properly
            if (checklistDictMule == null || checklist == null)
            {
                checklistDictMule = GameObject.Find("Checklist").transform.GetChild(1).gameObject;
                checklist = checklistDictMule.GetComponent<ChecklistUpdate>().checklist;

            }

            // checks if the dictionary for the respawning objects' Transform has been assigned properly
            if (posDict == null || rot8Dict == null && checklistDictMule != null)
            {
                posDict = checklistDictMule.GetComponent<ChecklistUpdate>().posDict;
                rot8Dict = checklistDictMule.GetComponent<ChecklistUpdate>().rot8Dict;
            }
            

            if (transformDict == null && checklistDictMule != null)
            {
                transformDict = checklistDictMule.GetComponent<ChecklistUpdate>().transformDict;
            }
        }
        
        // checks if the player is currently walking
        isWalking = gameObject.GetComponent<Walk>().isWalking;

    }

    // puts held object to backpack
    public void Place()
    {
        objPointed = gameObject.GetComponent<SelectObject>().objectPointed;
        initParent = gameObject.GetComponent<SelectObject>().initParent;

        // check if object being put to bag is in checklist
        if (gameObject.GetComponent<SelectObject>().isGrabbing == true) {
            
            // successful tap; stop timer
            gameObject.GetComponent<Adaptivity>().ResetTimer();

            nameToDestroy = objPointed.name;
            // DO NOT Destroy gameobject in room but set inactive instead
            (gameObject.GetComponent<SelectObject>().objectPointed).SetActive(false);

            // check if there is a need to reinstantiate
            // if object is water bottle
            if ((nameToDestroy == "Water Bottle") && GameObject.Find("Game").GetComponent<Player>().HHNumber > 1) {
                if (GameObject.Find("Game").GetComponent<Player>().waterDuplicate < 2 * GameObject.Find("Game").GetComponent<Player>().HHNumber - 2)
                {
                    Debug.Log(nameToDestroy + " init position: " + checklistDictMule.GetComponent<ChecklistUpdate>().transformDict[objPointed.tag].position.ToString());
                    Debug.Log(nameToDestroy + " init rotation: " + checklistDictMule.GetComponent<ChecklistUpdate>().transformDict[objPointed.tag].rotation.ToString());
                    reinstantiated = Reinstantiate(objPointed, initParent);

                    if (reinstantiated) GameObject.Find("Game").GetComponent<Player>().waterDuplicate++;
                    else Debug.LogError("Error with Reinstantiation of " + objPointed.name);
                }
            }

            // if object is canned good
            else if (nameToDestroy == "Canned Good" && GameObject.Find("Game").GetComponent<Player>().HHNumber > 1) {
                if (GameObject.Find("Game").GetComponent<Player>().foodDuplicate < 5 * GameObject.Find("Game").GetComponent<Player>().HHNumber - 5)
                {
                    Debug.Log(nameToDestroy + " init position: " + checklistDictMule.GetComponent<ChecklistUpdate>().transformDict[objPointed.tag].position.ToString());
                    Debug.Log(nameToDestroy + " init rotation: " + checklistDictMule.GetComponent<ChecklistUpdate>().transformDict[objPointed.tag].rotation.ToString());

                    reinstantiated = Reinstantiate(objPointed, initParent);
                    if (reinstantiated) GameObject.Find("Game").GetComponent<Player>().foodDuplicate++;
                    else Debug.LogError("Error with Reinstantiation of " + objPointed.name);
                }
            }
            // if object is medicine
            else if ((nameToDestroy == "Medicine") && GameObject.Find("Game").GetComponent<Player>().HHNeedMeds > 1)
            {
                if (GameObject.Find("Game").GetComponent<Player>().medsDuplicate < GameObject.Find("Game").GetComponent<Player>().HHNeedMeds-1)
                {
                    Debug.Log(nameToDestroy + " init position: " + checklistDictMule.GetComponent<ChecklistUpdate>().transformDict[objPointed.tag].position.ToString());
                    Debug.Log(nameToDestroy + " init rotation: " + checklistDictMule.GetComponent<ChecklistUpdate>().transformDict[objPointed.tag].rotation.ToString());
                    
                    reinstantiated = Reinstantiate(objPointed, initParent);

                    if (reinstantiated) GameObject.Find("Game").GetComponent<Player>().medsDuplicate++;
                    else Debug.LogError("Error with Reinstantiation of " + objPointed.name);
                }
            }

            // if object is stuffed toy
            else if ((nameToDestroy == "Stuffed toy") && GameObject.Find("Game").GetComponent<Player>().HHChildren > 0 && GameObject.Find("Game").GetComponent<Player>().HHIncome == 1)
            {
                if (GameObject.Find("Game").GetComponent<Player>().toysDuplicate < GameObject.Find("Game").GetComponent<Player>().HHChildren - 1)
                {
                    Debug.Log(nameToDestroy + " init position: " + checklistDictMule.GetComponent<ChecklistUpdate>().transformDict[objPointed.tag].position.ToString());
                    Debug.Log(nameToDestroy + " init rotation: " + checklistDictMule.GetComponent<ChecklistUpdate>().transformDict[objPointed.tag].rotation.ToString());

                    reinstantiated = Reinstantiate(objPointed, initParent);

                    if (reinstantiated) GameObject.Find("Game").GetComponent<Player>().toysDuplicate++;
                    else Debug.LogError("Error with Reinstantiation of " + objPointed.name);
                }
            }
            
            // add item to backpack items list
            GameObject.Find("Game").GetComponent<Player>().AddToPlayerBackpack(objPointed);

            // if object is in checklist, decrement count on checklist
            // CORRECT MOVE
            if (checklist.ContainsKey(objPointed.name) && initParent.name == "Supplies")
            {
                // subtract only if value > 0; just for catching mis-respawning objects
                if ((checklistDictMule.GetComponent<ChecklistUpdate>().checklist[nameToDestroy]) > 0) (checklistDictMule.GetComponent<ChecklistUpdate>().checklist[nameToDestroy])--;
                // increment count for duplicates
                switch (objPointed.name)
                {
                    case "Water Bottle":
                        GameObject.Find("Game").GetComponent<Player>().waterCount++;
                        Debug.Log("Water count: " + GameObject.Find("Game").GetComponent<Player>().waterCount);
                        break;

                    case "Canned Good":
                        GameObject.Find("Game").GetComponent<Player>().foodCount++;
                        Debug.Log("Food count: " + GameObject.Find("Game").GetComponent<Player>().foodCount);
                        break;

                    case "Stuffed toy":
                        GameObject.Find("Game").GetComponent<Player>().toysCount++;
                        Debug.Log("Toys count: " + GameObject.Find("Game").GetComponent<Player>().toysCount);
                        break;

                    case "Medicine":
                        GameObject.Find("Game").GetComponent<Player>().medsCount++;
                        Debug.Log("Meds count: " + GameObject.Find("Game").GetComponent<Player>().medsCount);
                        break;

                }
                // correct sound
                GameObject.Find("Supplies").GetComponent<SuppliesKitGame>().PlayCorrect();

                // if all of the instances of the item have been located, show explanation
                if (checklistDictMule.GetComponent<ChecklistUpdate>().checklist[nameToDestroy] == 0) {
                   
                     if (suppliesDict2.ContainsKey(nameToDestroy)) {
                        Debug.Log("contains key");
                        GameObject.Find("Player").GetComponent<Adaptivity>().ShowHint(suppliesDict2[nameToDestroy]); 
                    }
                }
            }

            // if not in checklist, mark as wrong
            else {
                gameObject.GetComponent<Adaptivity>().WrongMove();
            }

            gameObject.GetComponent<SelectObject>().isGrabbing = false;

            // run timer after putting in bag
            if (!isWalking && !gameObject.GetComponent<Adaptivity>().isTicking) gameObject.GetComponent<Adaptivity>().RunTimer();
        }

    }

    // "reinstantiates" by placing object back to its init position
    // returns true if respawned successfully
    public bool Reinstantiate(GameObject item, Transform initParent)
    {
        Debug.Log("Pumasok sa Reinstantiate()");
        Transform currentParent = item.transform.parent;
        Debug.Log("Current parent: " + currentParent);
        if (currentParent != initParent)
        {
            // return to init parent
            item.transform.parent = initParent;
            Debug.Log("Pinalitan si parent; current parent: " + item.transform.parent);
        
            item.transform.localPosition = posDict[item.tag];
            item.transform.localRotation = rot8Dict[item.tag];
            item.transform.position = transformDict[item.tag].position;
            item.transform.rotation = transformDict[item.tag].rotation;
            Debug.Log("Reinstantiated " + item.name);

            item.SetActive(true);
            // just double-checking
            if (!item.activeSelf) GameObject.FindGameObjectWithTag(item.tag).SetActive(true);
            
            return true;
        }

        Debug.Log("Returning false reinstantiation");
        return false;

    }
    
}
