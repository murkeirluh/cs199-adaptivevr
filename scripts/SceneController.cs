using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/* 
 * SceneController.cs
 * ------------
 * This script is attached to Game.
 * 
 * Responsible for the transitions between scenes and handles the UI element changes
 * in different scenes.
 * 
 */

public class SceneController : MonoBehaviour
{
    private int seconds;
    private GameObject gameObj;
    public GameObject breakCanvas;
    string currentModule;

    void Start()
    {
        gameObj = GameObject.Find("Game");
        if (breakCanvas == null) breakCanvas = GameObject.Find("Player/Main Camera/UI/ID Canvas");
        if (breakCanvas != null) breakCanvas.SetActive(false);
        else GameObject.Find("Player/Main Camera/UI/ID Canvas").SetActive(false);
        seconds = 4;
        currentModule = GameObject.Find("Game").GetComponent<Launcher>().moduleName;
    }

    void Update()
    {
        if (currentModule != SceneManager.GetActiveScene().name && gameObj.GetComponent<Player>().playerID != 0)
        {
            GameObject.Find("Game").GetComponent<Launcher>().UpdatePlayerVariable();
            GameObject.Find("Game").GetComponent<Player>().SavePlayer();
            currentModule = SceneManager.GetActiveScene().name;
        }

    }

    // transition from main_menu > NEW GAME; called from StartNewGameTimer()
    public void StartGame()
    {
        //gameObj.GetComponent<Launcher>().NewGame();
        //gameObj.GetComponent<Launcher>().ChangeMode("New");
        // stop rain sound
        GameObject.Find("Rain Sound").GetComponent<AudioSource>().Stop();
        ChangeScene("introtv");

    }

    // triggered by Start Button on main menu
    public void StartButton()
    {
        gameObj.GetComponent<Launcher>().NewGame();
        gameObj.GetComponent<Launcher>().ChangeMode("New");
        // hide menu
        GameObject.Find("Menu Canvas").transform.GetChild(0).GetComponent<Text>().enabled = false;
        GameObject.Find("Menu Canvas").transform.GetChild(1).GetComponent<Text>().enabled = false;
        GameObject.Find("Menu Canvas").transform.GetChild(2).GetComponent<Text>().enabled = false;
        // show player ID
        breakCanvas.transform.GetChild(1).GetComponent<Text>().text = "Player ID:\n#" + GameObject.Find("Game").GetComponent<Launcher>().idNumber.ToString();
        Debug.Log(GameObject.Find("Player/Main Camera/UI/ID Canvas/Player ID").GetComponent<Text>().text);
        breakCanvas.SetActive(true);
        StartCoroutine(StartNewGameTimer());
    }

    public void ChangeScene(string sceneName)
    {
        // SceneManager.LoadScene(sceneName); //Replace with intro scene!
        Initiate.Fade(sceneName, Color.black, 2.0f);
        gameObj.GetComponent<Launcher>().UpdateModuleName();
        gameObj.GetComponent<Launcher>().UpdatePlayerVariable();
        // Play click sound
        gameObj.GetComponent<BGSfxManager>().PlaySFX(0);
        Debug.Log("Changing to " + sceneName + " scene");
    }

    // project list of saved files on screen
    public void LoadButton()
    {
        // get rid of title screen
        GameObject.Find("Menu Canvas").transform.GetChild(0).GetComponent<Text>().enabled = false;
        GameObject.Find("Menu Canvas").transform.GetChild(1).GetComponent<Text>().enabled = false;
        GameObject.Find("Menu Canvas").transform.GetChild(2).GetComponent<Text>().enabled = false;

        // make saved game canvas load
        //GameObject loadCanvas = GameObject.Find("Load Game Canvas");

        GameObject.Find("Load Game Canvas").SetActive(true);

        string path = Application.persistentDataPath + "/save";

        // get list of saved files
        int i = 0;

        // create a new directory if it doesn't exist yet
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Directory now exists: " + Directory.Exists(path).ToString());
        }

        // list all saved files in environment
        foreach (string file in Directory.GetFiles(path))
        {
            Debug.Log("Saved " + (i + 1).ToString());
            GameObject savedFileObj;

            string fileName = Path.GetFileNameWithoutExtension(file);
            Debug.Log("File: " + fileName);
            if (i + 1 < 5)
            {
                savedFileObj = GameObject.Find("First col").transform.GetChild(i).gameObject;
                savedFileObj.GetComponent<Text>().text = '#' + fileName;
                savedFileObj.SetActive(true);

            }

            else if (i + 1 >= 5 && i + 1 < 9)
            {

                savedFileObj = GameObject.Find("Second col").transform.GetChild(i - 4).gameObject;
                savedFileObj.GetComponent<Text>().text = '#' + fileName;
                savedFileObj.SetActive(true);

            }

            else if (i + 1 >= 9 && i + 1 < 13)
            {
                savedFileObj = GameObject.Find("Third col").transform.GetChild(i - 8).gameObject;
                savedFileObj.GetComponent<Text>().text = '#' + fileName;
                savedFileObj.SetActive(true);

            }

            else if (i + 1 >= 13 && i + 1 < 17)
            {
                savedFileObj = GameObject.Find("Fourth col").transform.GetChild(i - 12).gameObject;
                savedFileObj.GetComponent<Text>().text = '#' + fileName;
                savedFileObj.SetActive(true);

            }

            else if (i + 1 >= 17 && i + 1 < 21)
            {
                savedFileObj = GameObject.Find("Fifth col").transform.GetChild(i - 16).gameObject;
                savedFileObj.GetComponent<Text>().text = '#' + fileName;
                savedFileObj.SetActive(true);

            }

            i += 1;
        }

        // if no files were found, show "no saved files" text
        if (i == 0)
        {
            seconds = 4;
            GameObject.Find("Load Game Canvas").transform.GetChild(1).gameObject.SetActive(true);
            GameObject.Find("Load Game Canvas").transform.GetChild(2).gameObject.SetActive(true);
            GameObject.Find("Load Game Canvas").transform.GetChild(2).gameObject.GetComponent<Text>().text = "Starting new game in " + seconds.ToString() + "...";
            StartCoroutine(StartNewGameTimer());

        }

        else
        {
            GameObject.Find("Load Game Canvas").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Load Game Canvas").transform.GetChild(3).gameObject.SetActive(true);
            gameObj.GetComponent<Launcher>().ChangeMode("Load");

        }

    }

    public void GoBack()
    {
        seconds = 2;
        StartCoroutine(GoBackToMenu());
    }

    public IEnumerator GoBackToMenu()
    {
        while (true)
        {
            if (seconds > 0)
            {
                seconds--;
                yield return new WaitForSeconds(1);
            }

            else
            {
                seconds = 4;
                GameObject.Find("Load Game Canvas").transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find("Load Game Canvas").transform.GetChild(3).gameObject.SetActive(false);

                for (int i = 0; i < 4; i++)
                {
                    GameObject.Find("First col").transform.GetChild(i).gameObject.SetActive(false);
                    GameObject.Find("Second col").transform.GetChild(i).gameObject.SetActive(false);
                    GameObject.Find("Third col").transform.GetChild(i).gameObject.SetActive(false);
                    GameObject.Find("Fourth col").transform.GetChild(i).gameObject.SetActive(false);
                    GameObject.Find("Fifth col").transform.GetChild(i).gameObject.SetActive(false);

                }

                GameObject.Find("Menu Canvas").transform.GetChild(0).GetComponent<Text>().enabled = true;
                GameObject.Find("Menu Canvas").transform.GetChild(1).GetComponent<Text>().enabled = true;
                GameObject.Find("Menu Canvas").transform.GetChild(2).GetComponent<Text>().enabled = true;
                yield break;
            }
        }
    }

    public void LoadGame()
    {
        seconds = 3;
        StartCoroutine(LoadTimer());
    }

    public IEnumerator LoadTimer()
    {
        while (true)
        {
            if (seconds > 0)
            {
                seconds--;
                yield return new WaitForSeconds(1);
            }

            else
            {
                seconds = 4;
                // get playerID and set to non-destroyable/static "Game" object
                gameObj.GetComponent<Launcher>().LoadGame(this.GetComponent<Text>().text.Substring(1));

                // load player from file
                gameObj.GetComponent<Player>().LoadPlayer(this.GetComponent<Text>().text.Substring(1));

                Debug.Log("Module to load:" + gameObj.GetComponent<Player>().currentModule);
                // change to scene where player saved
                switch (gameObj.GetComponent<Player>().currentModule)
                {
                    case "Before":
                        ChangeScene("beforetyphoon");
                        // play before typhoon bgm
                        gameObj.GetComponent<BGSfxManager>().PlaySFX(1);
                        break;

                    case "Bahay interior":
                        ChangeScene("bahay_interior");
                        // play before typhoon bgm
                        gameObj.GetComponent<BGSfxManager>().PlaySFX(1);
                        break;

                    case "During":
                        ChangeScene("evacuation");
                        break;

                    case "Introduction":
                        ChangeScene("introduction");
                        break;
                }
                yield break;
            }
        }
    }


    public void ExitApp()
    {
        Application.Quit();
    }

    public IEnumerator StartNewGameTimer()
    {
        while (true)
        {
            if (seconds > 0)
            {
                seconds--;
                yield return new WaitForSeconds(1);
            }

            else
            {
                seconds = 4;
                StartGame();
                yield break;
            }
        }
    }
}
