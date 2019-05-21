using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * SaveSystem.cs
 * ------------
 * In here, the file reading and writing is handled. The serialized variables from the
 * PlayerData object are either stored or loaded from file.
 * 
 */

public static class SaveSystem
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        string path = Application.persistentDataPath + "/save";
        string txtpath = Application.persistentDataPath + "/" + player.playerID.ToString() + ".txt";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Directory exists: " + Directory.Exists(path).ToString());
        }

        // 1. text file for easier debugging
        StreamWriter writer = new StreamWriter(txtpath, false);

        path += "/" + player.playerID.ToString() + ".save";

        FileStream stream = new FileStream(path, FileMode.Create);

        // create PlayerData object to serialize data
        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();

        // 2. start writing variables in text file
        writer.WriteLine("Player ID: " + player.playerID);
        writer.WriteLine("\nCurrent module: " + player.currentModule);
        writer.WriteLine("\nIncorrect tries: " + player.incorrectTries);
        writer.WriteLine("\nPosition: " + player.worldPosition.ToString());
        writer.WriteLine("\nHHNumber: " + player.HHNumber);
        writer.WriteLine("\nHHNeedMeds: " + player.HHNeedMeds);
        writer.WriteLine("\nHHChildren: " + player.HHChildren);
        writer.WriteLine("\nHHIncome: " + player.HHIncome);
        writer.WriteLine("\nhasLPG: " + player.hasLPG);
        writer.WriteLine("\nhasElectricity: " + player.hasElectricity);
        writer.WriteLine("\nFood duplicate: " + player.foodDuplicate);
        writer.WriteLine("\nWater duplicate: " + player.waterDuplicate);
        writer.WriteLine("\nMeds duplicate: " + player.medsDuplicate);
        writer.WriteLine("\nToys count: " + player.toysCount);
        writer.WriteLine("\nFood count: " + player.foodCount);
        writer.WriteLine("\nWater count: " + player.waterCount);
        writer.WriteLine("\nMeds count: " + player.medsCount);
        writer.WriteLine("\nToys count: " + player.toysCount);
        writer.WriteLine("\nBackpack items: ");
        if (player.backpackItems.Count > 0)
        {
            foreach (string item in player.backpackItems.Keys)
            {
                writer.WriteLine("\n" + item);
            }
        }
        
        else writer.WriteLine("None\n");

        // 3. close writer
        writer.Close();

        Debug.Log("Save successful.");

    }

    public static PlayerData LoadPlayer(string playerID)
    {
        string path = Application.persistentDataPath + "/save/" + playerID + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;


        }

        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
