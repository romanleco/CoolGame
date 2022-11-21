using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkbenchTwoButtons : MonoBehaviour
{
    private DataContainer _data;
    [SerializeField] private TMP_Text[] _resourceTexts = new TMP_Text[4];
    [SerializeField] private Monitor _monitorScript;
    [SerializeField] private List<WeaponData> _weapons = new List<WeaponData>();
    [SerializeField] private GameObject[] _playerWeapons = new GameObject[4];
    private int _currentlyDisplayedWeapon = 0;
    [Header("UI Components")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _weaponChangeText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _damageText, _fireRateText, _rangeText;
    [SerializeField] private Image _weaponDisplayImage;
    [SerializeField] private TMP_Text _metalPlatesText, _energyCoresText, _gearsText, _circuitBoardsText;
    [SerializeField] private GameObject _purchaseButton, _equipButton, _equippedButton, _ownedButton;

    void Start()
    {
        _data = SaveManager.Instance.Load();
        LoadWeaponData(0);
        this.gameObject.SetActive(false);
    }

    private void LoadWeaponData(int index)
    {
        _currentlyDisplayedWeapon = index;
        WeaponData weapon = _weapons[index];

        _nameText.text = weapon.weaponName;
        _weaponChangeText.text = (index + 1).ToString() + "/" + _weapons.Count.ToString();
        _descriptionText.text = weapon.description;

        _damageText.text = "Damage: " + weapon.damage.ToString();
        _fireRateText.text = "Fire Rate: " + weapon.fireRate.ToString() + "/s";
        _rangeText.text = "Range: " + weapon.range.ToString();

        _weaponDisplayImage.sprite = weapon.sprite;
        _weaponDisplayImage.SetNativeSize();
        _weaponDisplayImage.rectTransform.sizeDelta *= 2;

        if(_data.weaponsUnlocked[index] == false)
        {
            _metalPlatesText.text = weapon.metalPlatesNeeded.ToString();
            _energyCoresText.text = weapon.energyCoresNeeded.ToString();
            _gearsText.text = weapon.gearsNeeded.ToString();
            _circuitBoardsText.text = weapon.circuitBoardsNeeded.ToString();
        }
        else
        {
            _metalPlatesText.text = "-";
            _energyCoresText.text = "-";
            _gearsText.text = "-";
            _circuitBoardsText.text = "-";
        }

        ManageButtons(index);
    }

    private void ManageButtons(int index)
    {
        if(_data.weaponsUnlocked[index] == true)
        {
            _ownedButton.SetActive(true);
            _purchaseButton.SetActive(false);
            if(index == _data.currentWeaponIndex)
            {
                _equipButton.SetActive(false);
                _equippedButton.SetActive(true);
            }
            else
            {
                _equipButton.SetActive(true);
                _equippedButton.SetActive(false);
            }
        }
        else
        {
            _ownedButton.SetActive(false);
            _purchaseButton.SetActive(true);
            _equipButton.SetActive(false);
            _equippedButton.SetActive(false);
        }
    }

    public void ButtonRight()
    {
        if((_currentlyDisplayedWeapon + 1) >= _data.weaponsUnlocked.Length)
        {
            LoadWeaponData(0);
        }
        else
        {
            LoadWeaponData(_currentlyDisplayedWeapon + 1);
        }
    }

    public void ButtonLeft()
    {
        if(_currentlyDisplayedWeapon > 0)
        {
            LoadWeaponData(_currentlyDisplayedWeapon - 1);
        }
        else
        {
            LoadWeaponData(_data.weaponsUnlocked.Length - 1);
        }
    }

    public void PurchaseButton()
    {
        WeaponData weapon = _weapons[_currentlyDisplayedWeapon];
        if(_data.metalPlates >= weapon.metalPlatesNeeded && _data.energyCores >= weapon.energyCoresNeeded && _data.gears >= weapon.gearsNeeded && _data.circuitBoards >= weapon.circuitBoardsNeeded)
        {
            SaveManager.Instance.UnlockWeapon(_currentlyDisplayedWeapon);
            _purchaseButton.SetActive(false);
            _equipButton.SetActive(true);
            SaveManager.Instance.Save(_data.metalPlates - weapon.metalPlatesNeeded, _data.energyCores - weapon.energyCoresNeeded, _data.gears - weapon.gearsNeeded, _data.circuitBoards - weapon.circuitBoardsNeeded);

            _monitorScript.UpdateMonitor();

            _data = SaveManager.Instance.Load();
        }
    }

    public void EquipButton()
    {
        _playerWeapons[_data.currentWeaponIndex].SetActive(false);
        _playerWeapons[_currentlyDisplayedWeapon].SetActive(true);
        GameManager.Instance.SetCurrentWeaponIndex(_currentlyDisplayedWeapon);
        _equipButton.SetActive(false);
        _equippedButton.SetActive(true);

        SaveManager.Instance.SetCurrentlyEquippedWeapon(_currentlyDisplayedWeapon);

        _data = SaveManager.Instance.Load();
    }
}
