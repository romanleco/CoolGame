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

    void Start()
    {
        DataContainer data = SaveManager.Instance.Load();
    }
}
