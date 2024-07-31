using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pausePanel;

    private void Start()
    {
        Time.timeScale = 0;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus == false && _pausePanel.activeInHierarchy == false && Time.timeScale != 0)
            Pause(true);
    }

    public void Pause(bool isActive)
    {
        _pauseMenu.SetActive(isActive);
        Time.timeScale = isActive ? 0 : 1;
    }
}
