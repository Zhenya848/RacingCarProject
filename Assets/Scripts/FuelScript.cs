using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelScript : MonoBehaviour
{
    [SerializeField] private ShopScript _shopScript;
    [SerializeField] private AudioItem _buyItem;

    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private Image _fuelButton;

    [SerializeField] private TextMeshProUGUI _fuelCostText;
    [SerializeField] private Scrollbar _fuelScrollbar;

    [SerializeField] private float _currentFuel;
    private int _cost;

    private const string _FUEl_TAG = "FuelValue";
    private const string _FUEl_COST_TAG = "FuelCostValue";

    private const float MAX_TANK = 0.99f;

    private void Start()
    {
        _currentFuel = PlayerPrefs.HasKey(_FUEl_TAG) ? PlayerPrefs.GetFloat(_FUEl_TAG) : 0.7f;
        _cost = PlayerPrefs.HasKey(_FUEl_COST_TAG) ? PlayerPrefs.GetInt(_FUEl_COST_TAG) : 45;

        _fuelScrollbar.size = _currentFuel;
        _fuelCostText.text = _currentFuel < MAX_TANK ? $"öåíà: {_cost}$" : "Ìàêñèìóì";
    }

    public void FuelButtonActive(bool isActive)
    {
        _fuelButton.color = isActive ? new Color32(255, 193, 126, 206) : new Color32(136, 185, 255, 206);
        _upgradeButton.SetActive(isActive);
    }

    public void UpgradeCistern()
    {
        if (_shopScript.Money >= _cost && _currentFuel < MAX_TANK)
        {
            _currentFuel += 0.075f;
            _fuelScrollbar.size = _currentFuel;

            PlayerPrefs.SetFloat(_FUEl_TAG, _currentFuel);

            _shopScript.WriteOffMoney(_cost);
            _buyItem.PlayAudio();

            if (_currentFuel < MAX_TANK)
            {
                _cost += 12 + _cost / 5;
                _fuelCostText.text = $"öåíà: {_cost}$";

                PlayerPrefs.SetInt(_FUEl_COST_TAG, _cost);
            }
            else
                _fuelCostText.text = "ÌÀÊÑÈÌÓÌ";
        }
    }
}
