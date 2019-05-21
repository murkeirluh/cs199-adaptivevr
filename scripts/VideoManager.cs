using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/*
 * VideoManager.cs
 * ------------
 * This script handles and controls the videos played during the Introduction scene. 
 * 
 */

public class VideoManager : MonoBehaviour
{
    public VideoClip video1;
    public VideoClip video2;

    VideoPlayer vp;

    public bool playedVid1;
    public bool playedVid2;
    bool finishedIntroTV;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObj = GameObject.Find("Game");
        if (gameObj != null) finishedIntroTV = GameObject.Find("Game").GetComponent<Player>().finishedIntroTV;
        if (gameObj == null || !finishedIntroTV)
        {
            playedVid1 = false;
            playedVid2 = false;

            vp = GetComponent<VideoPlayer>();
            vp.clip = video1;

            vp.loopPointReached += EndReached;

            vp.Play();

        }
        else
        {
            Continue();

        }

    }

    // Update is called once per frame
    void EndReached(VideoPlayer vp)
    {
        if (playedVid1 == false) {
            playedVid1 = true;
            vp.clip = video2;
            vp.Play();
        } else if (playedVid2 == false) {
            playedVid2 = true;
            Debug.Log("Videos finished");
            // mark intro as done
            GameObject.Find("Game").GetComponent<Player>().finishedIntroTV = true;
            // save game
            GameObject.Find("Game").GetComponent<Player>().SavePlayer();
            // change scene
            GameObject.Find("Game").GetComponent<SceneController>().ChangeScene("introduction");
        } 
    }

    // play the first video
    public void playOne() {
        vp = GetComponent<VideoPlayer>();
        vp.Stop();
        playedVid1 = false;
        playedVid2 = false;
        vp.clip = video1;
        vp.loopPointReached += EndReached;
        vp.Play();        
    }

    // play the second video
    public void playTwo() {
        vp = GetComponent<VideoPlayer>();
        vp.Stop();
        playedVid2 = false;
        vp.clip = video2;
        vp.loopPointReached += EndReached;
        vp.Play();        
    }

    public void Continue() {
        GameObject.Find("Game").GetComponent<Player>().finishedIntroTV = true;
        GameObject.Find("Game").GetComponent<Player>().SavePlayer();
        GameObject.Find("Game").GetComponent<SceneController>().ChangeScene("introduction");        
    }
}
