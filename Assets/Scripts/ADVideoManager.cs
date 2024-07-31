using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class ADVideoManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void AddCoinsExtern(int value);

    [SerializeField] private ShopScript _shopScript;
    [SerializeField] private AudioItem _getAudio;

    [SerializeField] private TextMeshProUGUI _adVisitedText;
    private bool _moreThenOneVideo;

    [SerializeField] private int _adVisitedNeed;
    private int _adVisitedCount = 0;

    private void Start() 
    {
        _adVisitedCount = PlayerPrefs.HasKey("AdVisitedCount") ? PlayerPrefs.GetInt("AdVisitedCount") : 0;
        _adVisitedText.text = $"просмотрено: {_adVisitedCount} / {_adVisitedNeed} видеорекламы";
    }

    public void AddCoins(int value) 
    {
        if (_moreThenOneVideo)
        {
            if (_adVisitedCount + 1 == _adVisitedNeed)
            {
                _shopScript.AddMoney(value);
                _adVisitedCount = 0;

                _getAudio.PlayAudio();
            }
            else
                _adVisitedCount++;

            _adVisitedText.text = $"просмотрено: {_adVisitedCount} / {_adVisitedNeed} видеорекламы";
            PlayerPrefs.SetInt("AdVisitedCount", _adVisitedCount);
        }
        else
        {
            _shopScript.AddMoney(value);
            _getAudio.PlayAudio();
        }
    }

    public void ShowOneVideo(int value) 
    {
        _moreThenOneVideo = false;
        AddCoinsExtern(value);
    }

    public void ShowMoreVideoes(int value)
    {
        _moreThenOneVideo = true;
        AddCoinsExtern(value);
    }
}