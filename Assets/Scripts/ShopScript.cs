using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ShopScript : MonoBehaviour
{
    [SerializeField] private AudioItem _buySound;

    Component[] _components;
    Component _currentComponent;

    public int Money;
    [SerializeField] private int _indexOfChild;
    [SerializeField] private int _indexOfComponent;
    private int _level;

    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _blockItem;

    [SerializeField] private GameObject _lastLevelChildIcons;
    [SerializeField] private GameObject _nextLevelChildIcons;

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _buyText;
    [SerializeField] private TextMeshProUGUI _blockItemText;
    [SerializeField] private TextMeshProUGUI _selectText;

    [SerializeField] private TextMeshProUGUI _lastLevelChildText;
    [SerializeField] private TextMeshProUGUI _currentLevelChildText;
    [SerializeField] private TextMeshProUGUI _nextLevelChildText;

    [SerializeField] private Transform _kuzov;
    [SerializeField] private Transform _motors;
    [SerializeField] private Transform _ruls;
    [SerializeField] private Transform _spoilers;
    [SerializeField] private Transform _glushitels;
    [SerializeField] private Transform _wheels;
    [SerializeField] private Transform _flySwatter;
    [SerializeField] private Transform _ran;
    [SerializeField] private Transform _garages;

    public int[] rulCharacteristics = new int[] { 3, 3, 4, 5, 8, 8, 9 };
    public int[] wheelCharacteristics = new int[] { 1, 1, 1, 2, 5, 7, 7, 7 };
    public int[] kuzovCharacteristics = new int[] { 3, 3, 4, 4, 4, 5, 6, 6, 6, 7, 7, 7, 8 };
    public int[] motorCharacteristics = new int[] { 13, 18, 21 };
    public int[] spoilerCharacteristics = new int[] { 0, 5, 5, 5, 8, 12 };

    [SerializeField] private Scrollbar _speedScrollbar;
    [SerializeField] private Scrollbar _healthScrollbar;
    [SerializeField] private Scrollbar _turnableScrollbar;

    [SerializeField] private LevelScript _levelScript;
    [SerializeField] private GarageUpgrade _garageUpdate;

    [SerializeField] private GameObject[] _notifiesOfButtons;
    [SerializeField] private GameObject[] _componentIcons;
    [SerializeField] private Image[] _componentButtons;

    private int _lastLevel;

    private void Start()
    {
        _level = _levelScript.Level;
        _lastLevel = PlayerPrefs.GetInt("LastLevel");

        Money = GetSaveValue("Money", 10);
        StartCoroutine(MoneyAnim(0, Money));

        _components = new Component[]
        {
            new Component(_kuzov, "Kuzov", GetSaveValue("KuzovShopHistory", "1000000000000"), new int[] { 0, 5, 6, 10, 11, 13, 25, 26, 27, 37, 38, 40, 48 }, new int[] { 0, 2, 2, 3, 5, 5, 6, 9, 10, 10, 12, 13, 15 }),
            new Component(_motors, "Motor", GetSaveValue("MotorShopHistory", "100"), new int[] { 0, 20, 45 }, new int[] { 0, 5, 12 }),
            new Component(_ruls, "Rul", GetSaveValue("RulShopHistory", "1000000"), new int[] { 0, 5, 8, 21, 30, 31, 33 }, new int[] { 0, 2, 2, 3, 5, 9, 10, 10}),
            new Component(_spoilers, "Spoiler", GetSaveValue("SpoilerShopHistory", "100000"), new int[] { 0, 1, 3, 5, 24, 47 }, new int[] { 0, 2, 2, 3, 7, 12 }),
            new Component(_glushitels, "Glushitel", GetSaveValue("GlushitelShopHistory", "1000"), new int[] { 0, 11, 27, 50 }, new int[] { 0, 3, 5, 10 }),
            new Component(_wheels, "Wheels", GetSaveValue("WheelsShopHistory", "10000000"), new int[] { 0, 5, 8, 19, 28, 36, 39, 40 }, new int[] { 0, 0, 2, 4, 7, 10, 10, 11 }),
            new Component(_flySwatter, "FlySwatter", GetSaveValue("FlySwatterShopHistory", "1000"), new int[] { 0, 3, 5, 8 }, new int[] { 0, 3, 3, 4 }),
            new Component(_ran, "Ran", GetSaveValue("RanShopHistory", "10"), new int[] { 0, 100 }, new int[] { 0, 5 }),
            new Component(_garages, "Garage", GetSaveValue("GarageShopHistory", "10"), new int[] { 0, 100 }, new int[] { 0, 8 })
        };

        for (int i = 0; i < _components.Length; i++)
            InitializeChilds(i);

        ChangeComponent(0);
        ShowFeatures();
        NotifyComponents();
    }

    private void ShowFeatures()
    {
        int motorFeature = motorCharacteristics[GetSaveValue(_components[1].ComponentTag, 0)];
        int spoilerFeature = spoilerCharacteristics[GetSaveValue(_components[3].ComponentTag, 0)];
        int kuzovFeature = kuzovCharacteristics[GetSaveValue(_components[0].ComponentTag, 0)];
        int wheelFeature = wheelCharacteristics[GetSaveValue(_components[5].ComponentTag, 0)];
        int rulFeature = rulCharacteristics[GetSaveValue(_components[2].ComponentTag, 0)];

        _speedScrollbar.size = (motorFeature + spoilerFeature + kuzovFeature + wheelFeature) / 48.0f;
        _healthScrollbar.size = kuzovFeature / 8.0f;
        _turnableScrollbar.size = rulFeature / 8.0f;
    }

    private void InitializeChilds(int indexOfParent)
    {
        Component parent = _components[indexOfParent];

        for (int i = 0; i < parent.Childs.Length; i++)
            parent.Childs[i].SetActive(false);

        parent.Childs[GetSaveValue(parent.ComponentTag, 0)].SetActive(true);
    }

    private void NotifyComponents()
    {
        if (_lastLevel == 0)
            return;

        Debug.Log(_lastLevel);
        Debug.Log(_level);

        for (int i = 0; i < _components.Length; i++)
        {
            Component parent = _components[i];

            for (int j = 0; j < parent.Childs.Length; j++)
            {
                if (parent.ComponentLevels[j] >= (_lastLevel + 1) && parent.ComponentLevels[j] <= _level && parent.ComponentShopHistory[j] == '0')
                    _notifiesOfButtons[i].SetActive(true);
            }
        }

        _lastLevel = _level;
        PlayerPrefs.SetInt("LastLevel", 0);
    }

    private void SetLastGarage()
    {
        if (_currentComponent.ComponentTag == "Garage")
        {
            _currentComponent.Childs[_indexOfChild].SetActive(false);
            _currentComponent.Childs[GetSaveValue(_currentComponent.ComponentTag, 0)].SetActive(true);
        }
    }

    public void ChangeComponent(int indexOfComponent)
    {
        SetLastGarage();

        _componentButtons[_indexOfComponent].color = new Color32(136, 185, 255, 206);
        _componentIcons[_indexOfComponent].SetActive(false);

        _indexOfComponent = indexOfComponent;
        _currentComponent = _components[_indexOfComponent];

        for (int i = 0; i < _currentComponent.Childs.Length; i++)
        {
            if (_currentComponent.Childs[i].activeInHierarchy)
                _indexOfChild = i;
        }

        _buyButton.SetActive(IsItemPurchased(_currentComponent.ComponentShopHistory[_indexOfChild]));
        _blockItem.SetActive(IsItemBlocked(_indexOfChild));
        _selectText.text = IsItemSelected() ? "выбрано" : "выбрать";

        _notifiesOfButtons[_indexOfComponent].SetActive(false);
        UpdateChildIcons();

        _componentButtons[_indexOfComponent].color = new Color32(255, 193, 126, 206);
        _componentIcons[_indexOfComponent].SetActive(true);
    }

    private int GetSaveValue(string tag, int defaultValue)
    {
        if (PlayerPrefs.HasKey(tag) == false)
            return defaultValue;

        return PlayerPrefs.GetInt(tag);
    }

    private string GetSaveValue(string tag, string defaultValue)
    {
        if (PlayerPrefs.HasKey(tag) == false)
            return defaultValue;

        return PlayerPrefs.GetString(tag);
    }

    public void SaveItem()
    {
        PlayerPrefs.SetInt(_currentComponent.ComponentTag, _indexOfChild);
        _selectText.text = "выбрано";

        ShowFeatures();
    }

    private bool IsItemPurchased(char item)
    {
        if (item == '0')
        {
            _buyText.text = $"цена: {_currentComponent.ComponentPrices[_indexOfChild]}$";
            return true;
        }

        return false;
    }

    private bool IsItemBlocked(int indexOfChild)
    {
        if (_level < _currentComponent.ComponentLevels[indexOfChild])
        {
            _blockItemText.text = $"Требуется {_currentComponent.ComponentLevels[indexOfChild]} уровень!";
            return true;
        }

        return false;
    }

    private bool IsItemSelected()
    {
        return _indexOfChild == GetSaveValue(_currentComponent.ComponentTag, 0);
    }

    public void BuyItem()
    {
        if (Money >= _currentComponent.ComponentPrices[_indexOfChild])
        {
            int currentLevel = _level;
            char[] currentShopHistory = _currentComponent.ComponentShopHistory.ToCharArray();

            currentShopHistory[_indexOfChild] = '1';
            _currentComponent.ComponentShopHistory = new string(currentShopHistory);
            _components[_indexOfComponent] = _currentComponent;
            PlayerPrefs.SetString(_currentComponent.ComponentTag + "ShopHistory", _currentComponent.ComponentShopHistory);

            Money -= _currentComponent.ComponentPrices[_indexOfChild];
            StartCoroutine(MoneyAnim(Money + _currentComponent.ComponentPrices[_indexOfChild], Money));
            PlayerPrefs.SetInt("Money", Money);

            _levelScript.XpUp(_indexOfChild);
            _level = _levelScript.Level;

            _buyButton.SetActive(false);
            SaveItem();

            _buySound.PlayAudio();
            _garageUpdate.AddBox();

            if (currentLevel != _level)
            {
                _lastLevel = currentLevel;
                NotifyComponents();
            }
        }
    }

    public void WriteOffMoney(in int value)
    {
        Money -= value;
        StartCoroutine(MoneyAnim(Money + value, Money));

        PlayerPrefs.SetInt("Money", Money);
    }

    private void SetChildInfo()
    {
        _buyButton.SetActive(IsItemPurchased(_currentComponent.ComponentShopHistory[_indexOfChild]));
        _blockItem.SetActive(IsItemBlocked(_indexOfChild));
        _selectText.text = IsItemSelected() ? "выбрано" : "выбрать";
    }

    public void SelectLeft()
    {
        _currentComponent.Childs[_indexOfChild].SetActive(false);
       
        if (_indexOfChild == 0)
            _indexOfChild = _currentComponent.Childs.Length - 1;
        else
            _indexOfChild--;

        SetChildInfo();
        _currentComponent.Childs[_indexOfChild].SetActive(true);
    }

    public void SelectRight()
    {
        _currentComponent.Childs[_indexOfChild].SetActive(false);

        if (_indexOfChild == _currentComponent.Childs.Length - 1)
            _indexOfChild = 0;
        else
            _indexOfChild++;

        SetChildInfo();
        _currentComponent.Childs[_indexOfChild].SetActive(true);
    }

    public void UpdateChildIcons()
    {
        int maxChildIndex = _currentComponent.ComponentTransform.childCount - 1;

        _lastLevelChildIcons.SetActive(_indexOfChild != 0);
        _nextLevelChildIcons.SetActive(_indexOfChild < maxChildIndex);

        if (_componentIcons[_indexOfComponent].transform.childCount > 2)
        {
            _componentIcons[_indexOfComponent].transform.GetChild(0).gameObject.SetActive(_indexOfChild != 0);
            _componentIcons[_indexOfComponent].transform.GetChild(2).gameObject.SetActive(_indexOfChild < maxChildIndex);
        }

        _lastLevelChildText.text = "lvl " + _indexOfChild.ToString();
        _currentLevelChildText.text = "lvl " + (_indexOfChild + 1).ToString();
        _nextLevelChildText.text = "lvl " + (_indexOfChild + 2).ToString();
    }

    public void AddMoney(int value)
    {
        Money += value;
        StartCoroutine(MoneyAnim(Money - value, Money));

        PlayerPrefs.SetInt("Money", Money);
    }

    private IEnumerator MoneyAnim(int oldValue, int newValue)
    {
        int k = newValue - oldValue > 0 ? 1 : -1;
        float timeDelay = 0.4f / Mathf.Abs(newValue - oldValue);

        while (oldValue != newValue)
        {
            oldValue += k;
            _moneyText.text = $"Денег: {oldValue}$";

            yield return new WaitForSeconds(timeDelay);
        }
    }
}

struct Component
{
    public Transform ComponentTransform { get; private set; }
    public GameObject[] Childs { get; private set; }
    public string ComponentTag { get; private set; }
    public string ComponentShopHistory;
    public int[] ComponentPrices { get; private set; }
    public int[] ComponentLevels { get; private set; }

    public Component(Transform componentTransform, string componentTag, string componentShopHistory, int[] componentPrices, int[] componentLevels)
    {
        Childs = new GameObject[0];
        ComponentTransform = componentTransform;
        ComponentTag = componentTag;
        ComponentShopHistory = componentShopHistory;
        ComponentPrices = componentPrices;
        ComponentLevels = componentLevels;

        Childs = GetChilds();
    }

    private GameObject[] GetChilds()
    {
        GameObject[] result = new GameObject[ComponentTransform.childCount];

        for (int i = 0; i < result.Length; i++)
            result[i] = ComponentTransform.GetChild(i).gameObject;

        return result;
    }
}