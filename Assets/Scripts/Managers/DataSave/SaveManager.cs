using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("SaveManager the SaveManager is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    public void Save()
    {
        DataContainer loadedData = Load();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataContainer data = new DataContainer(GameManager.Instance.metalPlates, GameManager.Instance.energyCores, GameManager.Instance.gears, GameManager.Instance.circuitBoards);
        if(loadedData != null)
        {
            data.metalPlates += loadedData.metalPlates;
            data.energyCores += loadedData.energyCores;
            data.gears += loadedData.gears;
            data.circuitBoards += loadedData.circuitBoards;
        }

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public DataContainer Load()
    {
        string path = Application.persistentDataPath + "/save.datasave";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            if(stream.Length == 0)
            {
                stream.Close();
                return null;
            }

            DataContainer data = (DataContainer)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
