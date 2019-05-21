using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * PlayerData.cs
 * ------------
 * This script handles the serializing of the Player data variables
 * for storing and saving to file.
 * 
 */

[System.Serializable]
public class PlayerData {
    public int playerID;
    public string currentModule;
    public float[] worldPosition;
    public string[] backpackItems;
    public string[] backpackItemTags;
    public int incorrectTries, finishedIntroTV, finishedNPC, finishedBefore;
    public int HHNumber, HHNeedMeds, HHChildren, HHIncome;
    public int hasLPG, hasElectricity;
    public int foodDuplicate, waterDuplicate, toysDuplicate, medsDuplicate;
    public int foodCount, waterCount, toysCount, medsCount;

    public PlayerData (Player player) {
        playerID = (player.playerID);
        currentModule = player.currentModule;
        finishedIntroTV = (player.finishedIntroTV ? 1 : 0);
        finishedNPC = (player.finishedNPC ? 1 : 0);
        finishedBefore = (player.finishedBefore ? 1 : 0);
        worldPosition = new float[3];
        worldPosition[0] = player.worldPosition.x;
        worldPosition[1] = player.worldPosition.y;
        worldPosition[2] = player.worldPosition.z;
    
        /* important note: checklist is not saved anymore
         * the checklist shall be "loaded" depending on the items in the backpack */

        //checklistKeys = new string[player.checklist.Keys.Count];
        //player.checklist.Keys.CopyTo(checklistKeys, 0);
        //checklistValues = new int[player.checklist.Values.Count];
        //player.checklist.Values.CopyTo(checklistValues, 0);

        backpackItems = new string[player.backpackItems.Count];
        backpackItemTags = new string[player.backpackItems.Count];

        // copy keys to Items, values (tags) to ItemTags
        player.backpackItems.Values.CopyTo(backpackItems,0);
        player.backpackItems.Keys.CopyTo(backpackItemTags,0);

        incorrectTries = player.incorrectTries;
        HHNumber = player.HHNumber;
        HHNeedMeds = player.HHNeedMeds;
        HHChildren = player.HHChildren;
        HHIncome = player.HHIncome;
        hasLPG = (player.hasLPG ? 1 : 0); ;
        hasElectricity = (player.hasElectricity ? 1 : 0); ;

        foodDuplicate = player.foodDuplicate;
        waterDuplicate = player.waterDuplicate;
        toysDuplicate = player.toysDuplicate;
        medsDuplicate = player.medsDuplicate;
        foodCount = player.foodCount;
        waterCount = player.waterCount;
        toysCount = player.toysCount;
        medsCount = player.medsCount;
    }
    
}