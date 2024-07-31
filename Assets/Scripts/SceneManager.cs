using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneManager : MonoBehaviour
{
	private string[] _sceneNames = new string[] { "SampleScene", "Shop" };

    private void Awake()
    {
		Time.timeScale = 1.0f;
	}

	public void LoadSceneOfName(string sceneName)
    {
		foreach (string name in _sceneNames)
        {
			if (sceneName == name)
            {
				UnityEngine.SceneManagement.SceneManager.LoadScene(name);
				return;
			}
		}

		Debug.LogWarning("Не удалось загрузить сцену");
	}

	public void ReloadScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}
}