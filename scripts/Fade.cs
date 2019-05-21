using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Fade.cs
 * ------------
 * This script handles the fade out effect during scene transitions.
 * 
 */

public class Fade : MonoBehaviour
{
    IEnumerator FadeOut() 
    {
        for (float f = 1f; f >= 0; f -= 0.1f) 
        {
            Color c = GetComponent<Renderer>().material.color;
            c.a = f;
            GetComponent<Renderer>().material.color = c;
            yield return null;
        }
    }

    IEnumerator FadeIn() 
    {
        for (float f = 1f; f >= 0; f += 0.1f) 
        {
            Color c = GetComponent<Renderer>().material.color;
            c.a = f;
            GetComponent<Renderer>().material.color = c;
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("f")) 
        {
            StartCoroutine("FadeOut");
            StartCoroutine("FadeIn");
        }
    }
}
