using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Transform _road;
    private float _score;

    [SerializeField] private TextMeshProUGUI _currentDistanceText;
    [SerializeField] private TextMeshProUGUI _lastScoreText;

    private static string SCORE_TAG = "ScoreValue";

    private void Start()
    {   
        _score = PlayerPrefs.HasKey(SCORE_TAG) ? PlayerPrefs.GetFloat(SCORE_TAG) : 0;
    }

    private void FixedUpdate()
    {
        _currentDistanceText.text = (Mathf.RoundToInt(-_road.position.z) / 1000.0f).ToString() + " ����������";
    }

    private string GetCorrectWord(in int value)
    {
        if (value % 10 == 0 || value % 10 > 4 || (value > 4 && value < 21))
            return "����������";
        else if (value % 10 == 1)
            return "��������";

        return "���������";
    }

    public void SaveScore()
    {
        float current = Mathf.RoundToInt(-_road.position.z) / 1000.0f;

        if (current > _score)
        {
            _lastScoreText.text = $"����� ������! {current} {GetCorrectWord(Mathf.RoundToInt(current))}";
            PlayerPrefs.SetFloat(SCORE_TAG, current);
        }
        else
            _lastScoreText.text = $"���� ������: {_score} {GetCorrectWord(Mathf.RoundToInt(_score))}";
    }

    private void OnApplicationQuit()
    {
        SaveScore();
    }
}
