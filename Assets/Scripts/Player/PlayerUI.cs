using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Sprite[] _healthBarImages = new Sprite[6];
    [SerializeField] private Sprite[] _extendedHealthBarImages = new Sprite[8];
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _dashIcon;
    [SerializeField] private Image _dashIconContainer;
    [SerializeField] private GameObject _pauseMenu;
    private bool _healthExtended;

    private float _nextTime;
    private float _dashCooldown;
    private bool _readyToDash = true;
    [SerializeField] private Color _darkColor;
    [SerializeField] private Color _lightColor;
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

    public void SetDashCooldown(float dashCooldown)
    {
        _dashCooldown = dashCooldown;
    }

    public void Dash()
    {
        _dashIconContainer.color = _darkColor;
        _dashIcon.fillAmount = 0;
        _nextTime = Time.time + _dashCooldown;
        _readyToDash = false;
    }

    void Update()
    {
        if(_readyToDash == false)
        {
            if(_nextTime <= Time.time)
            {
                // _nextTime = Time.time + _dashCooldown;
                _dashIconContainer.color = _lightColor;
                _readyToDash = true;
            }
            else
            {
                _dashIcon.fillAmount = Mathf.Abs(((_nextTime - Time.time) / _dashCooldown) - 1);
            }
        }

        PauseMenu();
    }

    private void PauseMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_pauseMenu.activeSelf == true)
            {
                Time.timeScale = 1;
                _pauseMenu.SetActive(false);
                GameManager.Instance.SetGameState(false);
            }
            else
            {
                Time.timeScale = 0;
                _pauseMenu.SetActive(true);
                GameManager.Instance.SetGameState(true);
            }
        }
    }

    public void Resume()
    {
        if(_pauseMenu.activeSelf == true)
        {
            Time.timeScale = 1;
            _pauseMenu.SetActive(false);
            GameManager.Instance.SetGameState(false);
        }
    }
}
