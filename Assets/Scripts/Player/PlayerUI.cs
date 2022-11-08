using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Sprite[] _healthBarImages = new Sprite[6];
    [SerializeField] private Sprite[] _extendedHealthBarImages = new Sprite[8];
    [SerializeField] private Image _healthBar;
    private bool _healthExtended;
    public void UpdateHealth(int health)
    {
        if(_healthExtended)
        {
            _healthBar.sprite = _extendedHealthBarImages[Mathf.Abs(health-7)];
        }
        else
        {
            _healthBar.sprite = _healthBarImages[Mathf.Abs(health-5)];
        }
    }

    public void HealthExtended(bool healthExtended)
    {
        _healthExtended = healthExtended;
    }
}
