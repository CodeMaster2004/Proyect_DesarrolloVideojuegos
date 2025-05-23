using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameRepository
{
    static GameRepository instance;
    GameData gameData;

    public static GameRepository GetInstance()
    {
        if (instance == null)
        {
            instance = new GameRepository();
        }
        return instance;
    }


    public GameData GetData()
    {
        if (gameData != null)
        {
            return gameData; // si ya existe el objeto GameData, devolverlo
        }

        string path = Application.persistentDataPath + "/data.save";

        if (!File.Exists(path))
        {
            return new GameData();
        }

        FileStream file = File.OpenRead(path); // leer el archivo

        BinaryFormatter bf = new BinaryFormatter();
        gameData = (GameData)bf.Deserialize(file); // deserializar el archivo

        file.Close();

        return gameData;

    }

    public void SaveData()
    {

        string path = Application.persistentDataPath + "/data.save";
        FileStream file = null;
        if (File.Exists(path))
        {
            file = File.OpenWrite(path); // abrir el archivo para escribir
        }
        else
        {
            file = File.Create(path); // crear el archivo si no existe
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, gameData); // serializar el objeto GameData
        file.Close(); // cerrar el archivo
    }
}