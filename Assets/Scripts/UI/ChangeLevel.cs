using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("FirstScene");
    }
}
