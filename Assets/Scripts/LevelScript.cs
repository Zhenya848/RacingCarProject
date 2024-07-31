using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelScript : MonoBehaviour
{
    [SerializeField] private CarControllerOld _carController;

    [SerializeField] private Scrollbar _xpScrollbar;
    [SerializeField] private TextMeshProUGUI _xpText;
    [SerializeField] private TextMeshProUGUI _levelText;

    public int Level { get; private set; }
    private int _valueForLevelUp;
    [SerializeField] private float _xp;
    private const float _XP_UP_DELAY = 0.003f;

    private bool _carControllerActive;

    private int _lastLevel; 

    private void Start()
    {
        _carControllerActive = _carController ?? false;

        Level = GetSaveValue("Level", 1);
        _valueForLevelUp = GetSaveValue("ValueForLevelUp", 3);

        if (PlayerPrefs.HasKey("Xp") == false)
            _xp = 0.0f;
        else
            _xp = PlayerPrefs.GetFloat("Xp");

        _levelText.text = "Уровень: " + Level;
        _xpScrollbar.size = _xp;
        _xpText.text = $"Опыт: {Mathf.Round(_xp * _valueForLevelUp * 10) / 10} / {_valueForLevelUp}";

        if (_carControllerActive)
            StartCoroutine(XpSaveDelay(4.0f));

        _lastLevel = Level;
    }

    private void FixedUpdate()
    {
        if (_carControllerActive == false)
            return;

        _xp += (_XP_UP_DELAY * _carController.Speed * Time.fixedDeltaTime) / _valueForLevelUp;
        _xpText.text = $"Опыт: {Mathf.Round(_xp * _valueForLevelUp * 10) / 10} / {_valueForLevelUp}";
        _xpScrollbar.size = _xp;

        if (_xp >= 1)
            LevelUp();
    }

    private IEnumerator XpSaveDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            PlayerPrefs.SetFloat("Xp", _xp);
        }
    }

    public void XpUp(float value)
    {
        _xp += value / _valueForLevelUp;

        if (_xp >= 1)
            LevelUp();
        else
            PlayerPrefs.SetFloat("Xp", _xp);

        _xpText.text = $"Опыт: {Mathf.Round(_xp * _valueForLevelUp * 10) / 10} / {_valueForLevelUp}";
        _xpScrollbar.size = _xp;
    }

    private void LevelUp()
    {
        _levelText.text = "Уровень: " + (++Level);
        _xp--;
        _valueForLevelUp += 2;

        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetInt("ValueForLevelUp", _valueForLevelUp);
        PlayerPrefs.SetFloat("Xp", _xp);

        PlayerPrefs.SetInt("LastLevel", _lastLevel);
    }

    private int GetSaveValue(string tag, int defaultValue)
    {
        if (PlayerPrefs.HasKey(tag) == false)
            return defaultValue;

        return PlayerPrefs.GetInt(tag);
    }
}