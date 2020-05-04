using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public PlayerProfile profile;

    private void Awake()
    {
       
      
        try
        {
           profile = SaveSystem.LoadPlayerProfile();
            transform.Find("MainMenu").gameObject.SetActive(true);

        }
        catch
        {
            transform.Find("AddProfilePanel").gameObject.SetActive(true);
        }
    }
    public void addProfile()
    {

        var profileName = transform.Find("AddProfilePanel").Find("ProfileInput").GetComponent<TMPro.TMP_InputField>().text;
        var emptyPlayer = new Player();
        emptyPlayer.playerName = profileName.Equals("") ? "NoName" : profileName;
        profile = new PlayerProfile(emptyPlayer);
        SaveSystem.SavePlayer(emptyPlayer);
        transform.Find("AddProfilePanel").gameObject.SetActive(false);
        transform.Find("MainMenu").gameObject.SetActive(true);

    }
}
