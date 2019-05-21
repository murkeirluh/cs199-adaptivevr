using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SetLPGElecActiveState.cs
 * ------------
 * Checks for hasElectricity/hasLPG variables
 * and disables the respective GameObjects (whichever is false).
 *
 */

public class SetLPGElecActiveState : MonoBehaviour
{
    public GameObject fuseBox;
    public GameObject lpgTank;
    bool hasElectricity, hasLPG; 

    // Start is called before the first frame update
    void Start()
    {
        if (fuseBox == null || lpgTank == null)
        {
            fuseBox = GameObject.Find("Turn off/Fuse");
            lpgTank = GameObject.Find("Turn off/LPG");
        }
        
        hasElectricity = GameObject.Find("Game").GetComponent<Player>().hasElectricity;
        hasLPG = GameObject.Find("Game").GetComponent<Player>().hasLPG;

        // if the user answered that they don't have electricity, 
        // deactivate main switch in environment
        if (!hasElectricity)
        {
            GameObject.Find("Turn off/Fuse").GetComponent<TurnOff>().fuseOff = true;
            fuseBox.SetActive(false);
            GameObject.Find("Turn off/Wire_1").SetActive(false);
        }

        // if the user answered that they don't own an LPG tank, 
        // deactivate lpg tank in environment
        if (!hasLPG)
        {
            GameObject.Find("Turn off/LPG").GetComponent<TurnOff>().lpgOff = true;
            lpgTank.SetActive(false);
            GameObject.Find("Turn off/Wire_3 (2)").SetActive(false);
            GameObject.Find("Turn off/Wire_2").SetActive(false);
            GameObject.Find("Turn off/Wire_3").SetActive(false);
            GameObject.Find("Turn off/Wire_3 (1)").SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
