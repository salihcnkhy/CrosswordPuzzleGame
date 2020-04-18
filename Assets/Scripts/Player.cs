using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
     public string playerName = "";
    public int[] easy = new int[6];
    public int[] medium = new int[6];
    public int[] hard = new int[6];

    public void load()
    {

        try
        {
            var profile = SaveSystem.LoadPlayerProfile();
            playerName = profile.playerName;
            easy = profile.easy;
            medium = profile.medium;
            hard = profile.hard;
        }
        catch 
        {

            Debug.LogError("FileNot Found");

        }

    }

}
