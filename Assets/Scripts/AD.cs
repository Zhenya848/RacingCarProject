using UnityEngine;
using System.Runtime.InteropServices;

public class AD : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void MultiplyCoinsExtern();

    [SerializeField] private InteractionManager _interactionManager;
    [SerializeField] private bool _showInStart;

    private void Start()
    {
        if (_showInStart)
            ShowAd();
    }

    public void ShowAd()
    {
        Application.ExternalCall("ShowAd");
    }

    public void ShowVideo()
    {
        MultiplyCoinsExtern();
    }

    public void MultiplyCoins()
    {
        _interactionManager.MultiplyMoney();
    }
}
