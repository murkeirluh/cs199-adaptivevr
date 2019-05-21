using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* BGSfxManager.cs
 * ---------------
 * Manages background sound effects for Before Typhoon.
 * Audio sources are attached to Game object.
 * 
 */

public class BGSfxManager : MonoBehaviour
{
    public AudioSource[] sfx;

    // click - change scene sfx
    // buttonClick - for button presses in intro/npc scene
    public AudioSource click, beforetyphoonBGM, buttonClick;
    public bool beforeBGMplaying;
    string sceneName;


    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponents<AudioSource>();
        click = sfx[0];
        beforetyphoonBGM = sfx[1];
        buttonClick = sfx[2];
        beforeBGMplaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
        // check if before typhoon BGM not playing at respective scenes
        if (sceneName == "beforetyphoon" || sceneName == "bahay_interior")
        {
            if (!beforeBGMplaying) PlaySFX(1);
        }
    }

    // plays the appropriate sfx
    public void PlaySFX(int index)
    {
        switch (index)
        {
            case 0:
                click.Play();
                break;

            case 1:
                beforetyphoonBGM.Play();
                beforeBGMplaying = true;
                break;

            case 2:
                buttonClick.Play();
                break;

            default:
                break;
        }
    }
}