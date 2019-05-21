using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * DontDestroy.cs
 * ------------
 * This script handles the GameObjects that shouldn't be destroyed
 * during the gameplay.
 * 
 */

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");
        Debug.Log("Do not destroy the ff:");

        for (int i = 0; i < objs.Length; i++)
        {
            DontDestroyOnLoad(objs[i]);
            Debug.Log(objs[i]);
        }



    }
}