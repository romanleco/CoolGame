using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorkbenchOneButtons : MonoBehaviour
{
    private DataContainer _data;
    [SerializeField] private GameObject[] _buttons = new GameObject[3];
    [SerializeField] private GameObject[] _unlockedTexts = new GameObject[3];
    [SerializeField] private TMP_Text[] _resourceTexts = new TMP_Text[4];
    [SerializeField] private Monitor _monitorScript;
    [Header("Sound Effects")]
    [SerializeField] private AudioClip _wrong;
    [SerializeField] private AudioClip _correct;

    void Start()
    {
        DataContainer data = SaveManager.Instance.Load();

        if(data.wBOneUpgOneUnlocked)
        {
            _buttons[0].SetActive(false);
            _unlockedTexts[0].SetActive(true);
        }

        if(data.wBOneUpgTwoUnlocked)
        {
            _buttons[1].SetActive(false);
            _unlockedTexts[1].SetActive(true);
        }

        if(data.wBOneUpgThreeUnlocked)
        {
            _buttons[2].SetActive(false);
            _unlockedTexts[2].SetActive(true);
        }

        UpdateUI();
    }

    private void Upgrade(int metalPlatesNeeded, int energyCoresNeeded, int gearsNeeded, int circuitBoardsNeeded, int buttonIndex)
    {
        _data = SaveManager.Instance.Load();

        if(_data.metalPlates >= metalPlatesNeeded && _data.energyCores >= energyCoresNeeded && _data.gears >= gearsNeeded && _data.circuitBoards >= circuitBoardsNeeded)
        {
            SaveManager.Instance.UnlockUpgrade(1, (buttonIndex + 1));
            _buttons[buttonIndex].SetActive(false);
            _unlockedTexts[buttonIndex].SetActive(true);
            SaveManager.Instance.Save(_data.metalPlates - metalPlatesNeeded, _data.energyCores - energyCoresNeeded, _data.gears - gearsNeeded, _data.circuitBoards - circuitBoardsNeeded);

            _monitorScript.UpdateMonitor();
            UpdateUI();

            MusicManager.Instance.fXPlayer.PlayOneShot(_correct, SaveManager.Instance.fXVolume);
        }
        else
        {
            MusicManager.Instance.fXPlayer.PlayOneShot(_wrong, SaveManager.Instance.fXVolume);
        }
    }

    private void UpdateUI()
    {
        DataContainer updatedData = SaveManager.Instance.Load();
        _resourceTexts[0].text = updatedData.metalPlates.ToString();
        _resourceTexts[1].text = updatedData.energyCores.ToString();
        _resourceTexts[2].text = updatedData.gears.ToString();
        _resourceTexts[3].text = updatedData.circuitBoards.ToString();
    }

    public void WBOneUpgOne()
    {
        //increase health
        Upgrade(20, 15, 15, 20, 0);
    }

    public void WBOneUpgTwo()
    {
        //decrease dash cooldown
        Upgrade(15, 10, 10, 15, 1);
    }

    public void WBOneUpgThree()
    {
        //increase movement speed
        Upgrade(25, 30, 30, 25, 2);
    }
}
