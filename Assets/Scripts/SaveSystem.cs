using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{

    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.profile";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerProfile profile = new PlayerProfile(player);
        formatter.Serialize(stream, profile);
        stream.Close();
    }
    public static PlayerProfile LoadPlayerProfile() 
    {
        string path = Application.persistentDataPath + "/player.profile";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
             FileStream stream = new FileStream(path,FileMode.Open);
            PlayerProfile profile =  formatter.Deserialize(stream) as PlayerProfile;
            stream.Close();

            return profile;
        }
        else
        {
            Debug.LogError("Save File not found in "+path);
            throw new FileNotFoundException();
        }

    }


}
