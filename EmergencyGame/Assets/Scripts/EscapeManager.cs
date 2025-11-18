using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeManager : MonoBehaviour
{
    public static EscapeManager Instance;

    [Header("UI Elements")]
    public GameObject warningPanel;
    public Button yesButton;
    public Button noButton;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            //// 다른 오브젝트에 의해 부모가 되어버린 경우 제거
            //if (transform.parent != null)
            //    transform.SetParent(null, true); // true로 하면 위치/회전 유지

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        warningPanel.SetActive(false);

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        isPaused = true;
        warningPanel.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지
    }

    void ResumeGame()
    {
        isPaused = false;
        warningPanel.SetActive(false);
        Time.timeScale = 1f; // 게임 재개
    }

    void OnYesClicked()
    {
        Time.timeScale = 1f; // 씬 전환 전에 반드시 재개

        string currentScene = SceneManager.GetActiveScene().name;

        // "CPR" 또는 "GamePlaying" 씬이면
        if (currentScene == "CPR" || currentScene == "GamePlaying")
        {
            GameManager.gameState = null;
            SceneStateManager.instance.ClearSaved();
        }
        // "Bleeding" 또는 "BleedingGamepalying" 씬이면
        else if (currentScene == "Bleeding" || currentScene == "BleedingGamepalying")
        {
            BDgpManager.gameState = "";
            BDSceneStateManager.instance.ClearSaved();
        }

        warningPanel.SetActive(false);
        SceneManager.LoadScene("Title");
    }


   
    void OnNoClicked()
    {
        ResumeGame();
    }
}
