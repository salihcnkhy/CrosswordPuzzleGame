using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerProfile 
{

   public string playerName;
    public int[] easy;
    public int[] medium;
    public int[] hard;


    public PlayerProfile(Player player)
    {
        playerName = player.playerName;
        easy = player.easy;
        medium = player.medium;
        hard = player.hard;
    }
}
