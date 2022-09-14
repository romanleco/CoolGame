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
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataContainer data = Load();
        data.metalPlates += GameManager.Instance.metalPlates;
        data.energyCores += GameManager.Instance.energyCores;
        data.gears += GameManager.Instance.gears;
        data.circuitBoards += GameManager.Instance.circuitBoards;

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
