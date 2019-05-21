using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TurnOff.cs
 * ------------
 * This script is responsible for the scene for turning off the 
 * LPG tank and main switch.
 * 
 */

public class TurnOff : MonoBehaviour
{
    AudioSource audio = new AudioSource();
    public bool lpgOff, fuseOff;
    bool hasElectricity, hasLPG;

    void Start() {
        audio = GetComponent<AudioSource>();
        lpgOff = false;
        fuseOff = false;
        hasElectricity = GameObject.Find("Game").GetComponent<Player>().hasElectricity;
        hasLPG = GameObject.Find("Game").GetComponent<Player>().hasLPG;
        fuseOff |= !hasElectricity;
        lpgOff |= !hasLPG;
    }

    public void turnOff()
    {
        // play a switch sound
        audio.Play(); 
        // disable outline/highlight
        GetComponent<cakeslice.Outline>().enabled = false;
        GetComponent<Collider>().enabled = false;
        // return true if the object has been turned off
        if (this.gameObject.name == "LPG") lpgOff = true;
        else if (this.gameObject.name == "Fuse") fuseOff = true;
    }

    private void Update()
    {

    }
}
