using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* BackgroundTimer.cs
 * -----------------
 * This code is responsible for the 15-minute timer that runs in the background.
 * This also brings up the "take a break" screen once the 15 minutes is up.
 * 
 */

public class BackgroundTimer : MonoBehaviour {
       
    // remaining time in seconds (default: 900)
    public int timeRemaining = 900;
    private int defaultTime = 900;
    public bool shouldCountdown = true;

    // if the timer is ticking
    private bool isTicking;

    // array containing when (time remaining) should game automatically save 
    // game should save every 2 minutes/120 seconds
    int[] saveCheckpoint = {780,660,540,420,300,180,60};
    // counter/pointer for saveCheckpoint array
    int checkPoint;

	// Use this for initialization
	void Start () {
        isTicking = false;
        Time.timeScale = 1; //Just making sure that the timeScale is right
        checkPoint = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (!isTicking && SceneManager.GetActiveScene().name != "main_menu" && SceneManager.GetActiveScene().name != "introtv") {
            isTicking = true;
            StartCoroutine(CountDown());
        }

        // if 120 seconds have passed, save game
        if (timeRemaining == saveCheckpoint[checkPoint] && checkPoint < 6)
        {
            GameObject.Find("Game").GetComponent<Player>().SavePlayer();
            checkPoint++;
        }

        // show "take a break" screen if 15 minutes is up
        if (timeRemaining == 0) {
            GameObject.Find("Player/Main Camera/UI/Break Canvas").SetActive(true);
            GameObject.Find("Player/Main Camera/UI/Break Canvas").transform.GetChild(2).gameObject.GetComponent<Text>().text = "Player ID: #" + GameObject.Find("Game").GetComponent<Player>().playerID.ToString();
            PauseGame();
        }

    }

    // background timer main function
    public IEnumerator CountDown() {

        Debug.Log("Background Timer has started.");
        while (true) {

            // decrement time remaining
            if (timeRemaining > 0)
            {
                timeRemaining--;
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield break;
            }
        }
    }

    // call when timer is over
    // stops countdown, saves game and stops app
    public void PauseGame() {
        StopCoroutine(CountDown());
        Debug.Log("Countdown finished. Saving game.");
        GameObject.Find("Game").GetComponent<Player>().SavePlayer();
        Application.Quit();
    }
}