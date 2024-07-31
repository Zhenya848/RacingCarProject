using UnityEngine;
using TMPro;

public class QualityManager : MonoBehaviour
{
    private const string QUALITY_TAG = "QualityLevel";

    [SerializeField] private TextMeshProUGUI _qualityLevelText;
    private int _currentQualityIndex;

    private void Start()
    {
        if (PlayerPrefs.HasKey(QUALITY_TAG))
            _currentQualityIndex = PlayerPrefs.GetInt(QUALITY_TAG);
        else
            _currentQualityIndex = Application.isMobilePlatform ? 0 : QualitySettings.maxQueuedFrames - 1;

        QualitySettings.SetQualityLevel(_currentQualityIndex);
        SetQualityLevelText();
    }

    public void ChangeQuality(int qualityIndex)
    {
        try 
        { 
            QualitySettings.SetQualityLevel(qualityIndex);
            PlayerPrefs.SetInt(QUALITY_TAG, qualityIndex);

            _currentQualityIndex = qualityIndex;
            SetQualityLevelText();
        }
        catch { QualitySettings.SetQualityLevel(0); }
    }

    private void SetQualityLevelText()
    {
        switch (_currentQualityIndex)
        {
            case 0:
                _qualityLevelText.text = "Качество: мобильное";
                break;
            case 1:
                _qualityLevelText.text = "Качество: среднее";
                break;
            case 2:
                _qualityLevelText.text = "Качество: высокое";
                break;
        }
    }
}
