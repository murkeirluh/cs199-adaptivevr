using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * LetNPCIn.cs
 * ------------
 * This script is responsible for the scene where the neighbor NPC
 * knocks on the door. 
 * 
 */

public class LetNPCIn : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource[] sounds;
    public AudioSource noise1;
    public AudioSource noise2;
    public AudioSource noise3;

    GameObject npc;
    bool npcInside;
    
    void Awake() {
        npc = GameObject.Find("NPC");
        // do these inactivation in NPC.cs
        npcInside = false;
    }

    void Start()
    {
        // play knock knock audio here in a loop
        sounds = GetComponents<AudioSource>();
        noise1 = sounds[0];
        noise2 = sounds[1];
        noise3 = sounds[2];

        /* check if intro is already done
         * *******       
         * NOTE that this doesn't handle instances wherein user saved in the middle of convo with NPC
         * this only assumes FINISHED or UNFINISHED NPC dialogue. */
        
         // if the game is a loaded game
        if (GameObject.Find("Game").GetComponent<Launcher>().GetMode() == "Load")
        {
            // check if there are already values for stuff
            Player player = GameObject.Find("Game").GetComponent<Player>();
            if (player.finishedNPC)
            {
                gameObject.GetComponent<cakeslice.Outline>().enabled = false;
                gameObject.transform.GetChild(0).gameObject.GetComponent<cakeslice.Outline>().enabled = false;
                GameObject.Find("Turn off/Arrow").SetActive(false);
                StartCoroutine(GameObject.Find("NPC").GetComponent<NPC>().EndScene());
            }

            else
            {
                noise1.Play();
            }
        } 
        
        else
        {
            noise1.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("backspace") && !npcInside)
        {
            LetIn();
        }
    }

    public void LetIn() {
        //stop knock
        //play door open
        //play hello
        noise1.Stop();
        noise2.Play();
        noise3.Play();

        // activate NPC character
        npc.SetActive(true);
        npc.transform.GetChild(0).gameObject.SetActive(true);
        npc.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.GetComponent<cakeslice.Outline>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.GetComponent<cakeslice.Outline>().enabled = false;
        GameObject.Find("Turn off/Arrow").SetActive(false);
        npcInside = true;
    }
}