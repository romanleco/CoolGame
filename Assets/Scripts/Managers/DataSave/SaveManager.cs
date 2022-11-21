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

    public float fXVolume {get; private set;}

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        DataContainer loadedData = Load();
        if(loadedData != null)
        {
            fXVolume = loadedData.fXVolume / 100;
            Debug.Log("Volume: " + loadedData.fXVolume);
            Debug.Log("fXVolume: " + fXVolume);
        }
        else
        {
            fXVolume = 1;
            Debug.Log("NoSaveVolume: " + loadedData.fXVolume);
        }
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

            data.wBOneUpgOneUnlocked = loadedData.wBOneUpgOneUnlocked;
            data.wBOneUpgTwoUnlocked = loadedData.wBOneUpgTwoUnlocked;
            data.wBOneUpgThreeUnlocked = loadedData.wBOneUpgThreeUnlocked;

            for(int i = 0; i < data.weaponsUnlocked.Length; i++)
            {
                data.weaponsUnlocked[i] = loadedData.weaponsUnlocked[i];
            }

            data.currentWeaponIndex = loadedData.currentWeaponIndex;
        }
        data.weaponsUnlocked[0] = true;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void Save(int newMetalPlates, int newEnergyCores, int newGears, int newCircuitBoards)
    {
        DataContainer loadedData = Load();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataContainer data = new DataContainer(newMetalPlates, newEnergyCores, newGears, newCircuitBoards);

        if(loadedData != null)
        {
            data.wBOneUpgOneUnlocked = loadedData.wBOneUpgOneUnlocked;
            data.wBOneUpgTwoUnlocked = loadedData.wBOneUpgTwoUnlocked;
            data.wBOneUpgThreeUnlocked = loadedData.wBOneUpgThreeUnlocked;

            for(int i = 0; i < data.weaponsUnlocked.Length; i++)
            {
                data.weaponsUnlocked[i] = loadedData.weaponsUnlocked[i];
            }

            data.currentWeaponIndex = loadedData.currentWeaponIndex;
        }
        data.weaponsUnlocked[0] = true;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void UnlockUpgrade(int workbenchNumber, int upgradeNumber, bool unlock = true)
    {
        DataContainer loadedData = Load();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        if(loadedData == null)
        {
            loadedData = new DataContainer(0, 0, 0, 0);
            loadedData.weaponsUnlocked[0] = true;
            loadedData.currentWeaponIndex = 0;
        }

        if(workbenchNumber == 1)
        {
            switch(upgradeNumber)
            {
                case 1:
                    loadedData.wBOneUpgOneUnlocked = unlock;
                break;

                case 2:
                    loadedData.wBOneUpgTwoUnlocked = unlock;
                break;

                case 3:
                    loadedData.wBOneUpgThreeUnlocked = unlock;
                break;

                default:
                    Debug.LogError("Upgrade number defaulted");
                break;
            }
        }



        formatter.Serialize(stream, loadedData);
        stream.Close();
    }

    public void UnlockWeapon(int index, bool unlock = true)
    {
        DataContainer loadedData = Load();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        if(loadedData == null)
        {
            loadedData = new DataContainer(0, 0, 0, 0);
            loadedData.weaponsUnlocked[0] = true;
        }

        loadedData.weaponsUnlocked[index] = unlock;

        formatter.Serialize(stream, loadedData);
        stream.Close();
    }

    public void SetCurrentlyEquippedWeapon(int index)
    {
        DataContainer loadedData = Load();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        if(loadedData == null)
        {
            loadedData = new DataContainer(0, 0, 0, 0);
            loadedData.weaponsUnlocked[0] = true;
        }

        loadedData.currentWeaponIndex = index;

        formatter.Serialize(stream, loadedData);
        stream.Close();
    }

    public void AdjustVolume(int vol, bool musicVolume)
    {
        DataContainer loadedData = Load();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        if(loadedData == null)
        {
            loadedData = new DataContainer(0, 0, 0, 0);
        }

        if(musicVolume)
        {
            loadedData.volume = vol;
        }
        else
        {
            loadedData.fXVolume = vol;
            fXVolume = loadedData.fXVolume;
        }

        formatter.Serialize(stream, loadedData);
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
