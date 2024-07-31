using UnityEngine;

public class DeleteAll : MonoBehaviour
{
    public void Delete()
    {
        PlayerPrefs.DeleteAll();
    }
}
