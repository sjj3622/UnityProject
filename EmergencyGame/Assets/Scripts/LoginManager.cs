using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public void SetUserId(int userId)
    {
        PlayerPrefs.SetInt("USER_ID", userId);
        PlayerPrefs.Save();
    }

    public int GetUserId()
    {
        return PlayerPrefs.GetInt("USER_ID", 0);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Title");
    }
}
