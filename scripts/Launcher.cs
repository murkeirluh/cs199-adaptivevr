using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


/* 
 * Launcher.cs
 * ------------
 * This script is attached to Game.
 * 
 * Provides functions for loading and starting a new game.
 * Player ID is generated here.
 * Module name for tracking is updated here.
 * 
 */

public class Launcher : MonoBehaviour {

    // tells whether saving is working fine.
    public bool saveMode = true;
       
    // current module
    public string moduleName;
    public string mode;

    public int idNumber;
    public string dateTime;

    GameObject playerObject, breakCanvas;

    // Start is called before the first frame update
    void Awake()
    {
        // updating module name at Awake to prevent Inactivity timer from running at main_menu
        UpdateModuleName();
        playerObject = GameObject.Find("Player");
        breakCanvas = GameObject.Find("Break Canvas");

        saveMode = true;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateModuleName();
        if (breakCanvas != null) breakCanvas.SetActive(false);
        playerObject = GameObject.Find("Player");

        if (Input.GetKeyUp("space") && saveMode)
        {
            gameObject.GetComponent<Player>().SavePlayer();
        }
    }

    public void UpdateModuleName() {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "main_menu":
                moduleName = "Main Menu";
                break;

            case "bahay_interior":
                moduleName = "Bahay interior";
                break;

            case "beforetyphoon":
                moduleName = "Before";
                break;

            case "evacuation":
                moduleName = "During";
                break;

            case "introduction":
                moduleName = "Introduction";
                break;

        }
        
   }

    // changes the game mode to `m`
    public void ChangeMode(string m)
    {
        mode = m;
    }

    // returns current game mode
    public string GetMode()
    {
        return mode;
    }

    // generates a unique player ID for new player
    public void NewGame()
    {
        string path = Application.persistentDataPath + "/save";
        List <string> files = new List<string>();

        foreach (string file in files)
        {
            files.Add(Path.GetFileNameWithoutExtension(file));
        }

        idNumber = Random.Range(1, 999);
        // avoid duplicate file names
        while (files.Contains(idNumber.ToString()))
        {
            idNumber = Random.Range(1, 999);

        }

        dateTime = System.DateTime.Now.ToString();
        mode = "New Game";

        Debug.LogFormat("ID: {0}, Date: {1}", idNumber, dateTime);
    }

    // change config of player and game; called from SceneController.cs
    public void LoadGame(string idNum)
    {
        ChangeMode("Load");
        idNumber = System.Int32.Parse(idNum);
        gameObject.GetComponent<Player>().SetPlayerID(idNumber);
    }

    // updates local Player object variable (since it differs every scene)
    public void UpdatePlayerVariable()
    {
        playerObject = GameObject.Find("Player");
    }
}
