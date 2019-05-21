using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * NPC.cs
 * ------------
 * 
 * This script handles the scenario where the NPC neighbor
 * talks to the user.
 * This also takes care of the UI elements in the environment. 
 * 
 */

public class NPC : MonoBehaviour
{
    // NPC's dialogue
    private List<string> npcDialogue = new List<string>() {
        "Hello, neighbor!\n\nI'm here to ask if you're ready for the coming typhoon season. I can help you prepare!",
        "How many people are there in your household?\n(including you)",
        "How many people in your household need medication?",
        "How many people in your household are children?",
        "How would you generally classify your household?", 
        "Do you own an LPG tank at home?",
        "Do you have electricity in your home?",
        "Those are very useful information!\n\nThank you and\ntake care, neighbor!"
    };

    // current index in dialogue
    public int dialogueIndex;
    private GameObject dialogueBox, continueButton, numberDisplay, optionOne, optionTwo, minusToggle, plusToggle, nextButton;
    private GameObject theGameObject, objectPointed;
    GvrReticlePointer reticle;

    int seconds;
    string answer;
    GameObject npc;

    // Start is called before the first frame update
    void Start()
    {
        dialogueIndex = 0;
        seconds = 0;
        dialogueBox = GameObject.Find("NPC/UI/Popup/Dialogue");
        continueButton = GameObject.Find("NPC/UI/Popup/Continue");
        numberDisplay = GameObject.Find("NPC/UI/Popup/Number display");
        optionOne = GameObject.Find("NPC/UI/Popup/Option 1");
        optionTwo = GameObject.Find("NPC/UI/Popup/Option 2");
        minusToggle = GameObject.Find("NPC/UI/Popup/Minus toggle");
        plusToggle = GameObject.Find("NPC/UI/Popup/Plus toggle");
        nextButton = GameObject.Find("NPC/UI/Popup/Next");
        theGameObject = GameObject.Find("Game");
        reticle = GameObject.Find("Player/Main Camera/GvrReticlePointer").GetComponent<GvrReticlePointer>();
        npc = GameObject.Find("NPC");
    
        // inactivate entire NPC object if game is not loaded
        if (GameObject.Find("Game").GetComponent<Launcher>().GetMode() != "Load") npc.SetActive(false);
        // else, inactivate its children instead
        // children will re-activate in LetNPCIn()
        else
        {
            npc.transform.GetChild(0).gameObject.SetActive(false);
            npc.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (theGameObject == null) theGameObject = GameObject.Find("Game");
    }

    // increments dialogueIndex then change buttons displayed in environment
    public void Continue()
    {
        dialogueIndex++;
        ChangeButtons();
    }

    // changes buttons for display
    public void ChangeButtons()
    {
        //Debug.Log("ChangeButtons() dialogueIndex: " + dialogueIndex);
        dialogueBox.transform.GetChild(0).GetComponent<Text>().text = npcDialogue[dialogueIndex];
        // click sound
        theGameObject.GetComponent<BGSfxManager>().PlaySFX(2);
        switch (dialogueIndex)
        {
            case 1:
                continueButton.SetActive(false);
                minusToggle.SetActive(true);
                plusToggle.SetActive(true);
                numberDisplay.SetActive(true);
                nextButton.SetActive(true);
                break;
                
            case 4:
                minusToggle.SetActive(false);
                plusToggle.SetActive(false);
                numberDisplay.SetActive(false);
                optionOne.transform.GetChild(0).GetComponent<Text>().text = "Poor";
                optionTwo.transform.GetChild(0).GetComponent<Text>().text = "Well-off";
                optionOne.SetActive(true);
                optionTwo.SetActive(true);
                nextButton.SetActive(false);
                break;

            case 5:
                optionOne.transform.GetChild(0).GetComponent<Text>().text = "Yes";
                optionTwo.transform.GetChild(0).GetComponent<Text>().text = "No";
                break;

            case 6:
                optionOne.transform.GetChild(0).GetComponent<Text>().text = "Yes";
                optionTwo.transform.GetChild(0).GetComponent<Text>().text = "No";
                break;
                
            // ready for transition to next scene
            case 7:
                optionOne.SetActive(false);
                optionTwo.SetActive(false);
                nextButton.SetActive(false);
                // set Continue button text to "OK"
                continueButton.transform.GetChild(0).GetComponent<Text>().text = "OK";
                continueButton.SetActive(true);
                break;


            default:
                break;
        }
    }

    // increase number
    public void Plus()
    {
        if (numberDisplay != null)
        {
            // click sound
            theGameObject.GetComponent<BGSfxManager>().PlaySFX(2);
            numberDisplay.transform.GetChild(0).GetComponent<Text>().text = (int.Parse(numberDisplay.transform.GetChild(0).GetComponent<Text>().text) + 1).ToString();
            
        }
    }

    // decrease number
    public void Minus()
    {
        if (numberDisplay != null)
        {
            // click sound
            theGameObject.GetComponent<BGSfxManager>().PlaySFX(2);
            string txt = numberDisplay.transform.GetChild(0).GetComponent<Text>().text;
            // only decrease if display number >= 0
            if (int.Parse(txt) > 0)
            {
                numberDisplay.transform.GetChild(0).GetComponent<Text>().text = (int.Parse(numberDisplay.transform.GetChild(0).GetComponent<Text>().text) - 1).ToString();

            }
            
        }


    }

    // takes user's answer in the text box
    public void Answer()
    {
        // get text of clicked button
        objectPointed = reticle.CurrentRaycastResult.gameObject;
        //Debug.Log("Object pointed " + objectPointed.name);
        if (objectPointed.name != "Text") answer = objectPointed.transform.GetChild(0).GetComponent<Text>().text;
        else answer = objectPointed.GetComponent<Text>().text;
        //Debug.LogFormat("Answer(): {0} at dialogueIndex {1} ", answer, dialogueIndex);

        switch (dialogueIndex)
        {
            case 1:
                // get number
                int hhnum = int.Parse(numberDisplay.transform.GetChild(0).GetComponent<Text>().text);
                theGameObject.GetComponent<Player>().HHNumber = hhnum;
                Debug.Log("Player HHNum: " + theGameObject.GetComponent<Player>().HHNumber);
                OntoTheNext();
                break;

            case 2:
                int hhmeds = int.Parse(numberDisplay.transform.GetChild(0).GetComponent<Text>().text);
                theGameObject.GetComponent<Player>().HHNeedMeds = hhmeds;
                Debug.Log("Player HHNeedMeds: " + theGameObject.GetComponent<Player>().HHNeedMeds);
                OntoTheNext();
                break;

            case 3:
                int hhchildren = int.Parse(numberDisplay.transform.GetChild(0).GetComponent<Text>().text);
                theGameObject.GetComponent<Player>().HHChildren = hhchildren;
                Debug.Log("Player HHChildren: " + theGameObject.GetComponent<Player>().HHChildren);
                OntoTheNext();
                break;

            case 4:
                if (answer == "Poor") theGameObject.GetComponent<Player>().HHIncome = 0;
                else theGameObject.GetComponent<Player>().HHIncome = 1;
                Debug.Log("Player HHIncome: " + theGameObject.GetComponent<Player>().HHIncome);
                OntoTheNext();
                break;

            case 5:
                if (answer == "Yes") theGameObject.GetComponent<Player>().hasLPG = true;
                else theGameObject.GetComponent<Player>().hasLPG = false;
                Debug.Log("Player hasLPG: " + theGameObject.GetComponent<Player>().hasLPG);
                OntoTheNext();
                break;

            case 6:
                if (answer == "Yes") theGameObject.GetComponent<Player>().hasElectricity = true;
                else theGameObject.GetComponent<Player>().hasElectricity = false;
                Debug.Log("Player hasElectricity: " + theGameObject.GetComponent<Player>().hasElectricity);
                OntoTheNext();
                break;

            case 7:
                // play click sound
                theGameObject.GetComponent<BGSfxManager>().PlaySFX(2);
                // start countdown towards beforetyphoon transition
                StartCoroutine(EndScene());
                break;

            default:
                OntoTheNext();
                break;
        }

    }

    public void OntoTheNext()
    {
        // increment index
        dialogueIndex += 1;
        // change buttons for choices + dialogue text
        ChangeButtons();
    }

    // transition to beforetyphoon
    public IEnumerator EndScene()
    {
        while (true)
        {
            Debug.Log("Ending Intro scene!");
            if (seconds < 3)
            {
                seconds++;
                yield return new WaitForSeconds(1);
            }

            else
            {
                // SAVE GAME HERE
                if (GameObject.Find("Game") != null && theGameObject.GetComponent<Launcher>().saveMode)
                {
                    GameObject.Find("Game").GetComponent<Player>().finishedNPC = true;
                    theGameObject.GetComponent<Player>().SavePlayer();
                    GameObject.Find("Game").GetComponent<Player>().playerSet = true;
                }

                Debug.Log("Changing scene!");
                theGameObject.GetComponent<SceneController>().ChangeScene("beforetyphoon");
                seconds = 0;
                yield break;
            }
        }
    }
}
