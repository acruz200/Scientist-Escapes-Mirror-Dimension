using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTagSetter : MonoBehaviour
{
    void Awake()
    {
        // Make sure this object is tagged as Player
        if (tag != "Player")
        {
            Debug.Log("Setting tag to 'Player' for " + gameObject.name);
            tag = "Player";
        }
    }
} 