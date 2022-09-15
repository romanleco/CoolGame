using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Monitor : MonoBehaviour
{
    [SerializeField] private TMP_Text _metalPlatesText;
    [SerializeField] private TMP_Text _energyCoresText;
    [SerializeField] private TMP_Text _gearsText;
    [SerializeField] private TMP_Text _circuitBoardsText;
    void Start()
    {
        UpdateMonitor();
    }

    public void UpdateMonitor()
    {
        DataContainer data = SaveManager.Instance.Load();
        if(data != null)
        {
            _metalPlatesText.text = data.metalPlates.ToString();
            _energyCoresText.text = data.energyCores.ToString();
            _gearsText.text = data.gears.ToString();
            _circuitBoardsText.text = data.circuitBoards.ToString();
        }
        else
        {
            _metalPlatesText.text = "0";
            _energyCoresText.text = "0";
            _gearsText.text = "0";
            _circuitBoardsText.text = "0";
        }
    }
}
