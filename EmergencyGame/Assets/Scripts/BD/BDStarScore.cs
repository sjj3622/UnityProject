using UnityEngine;

public class BDStarScore : MonoBehaviour
{
    Animator animator;

    [Header("Animation Names")]
    public string Star0 = "StarImg0";
    public string Star1 = "StarImg1";
    public string Star2 = "StarImg2";
    public string Star3 = "StarImg3";
    public string Star4 = "StarImg4";

    //string nowAni = "", oldAni = "";

    public int sceneIndex = 2; // 씬 인덱스 지정
    private BDTimerController bdtimerController;

    void Start()
    {
        bdtimerController = FindObjectOfType<BDTimerController>();
        animator = GetComponent<Animator>();

        if (bdtimerController == null)
            Debug.LogWarning("씬에서 TimerController를 찾을 수 없습니다!");
    }

    void Update()
    {
        UpdateStarScore();
    }

    void UpdateStarScore()
    {
        if (bdtimerController == null) return;

        int starCount = 0;

        float timer = bdtimerController.totalTimer;

        if (timer <= 180 && timer > 144)
        {
            animator.Play(Star4);
            starCount = 4;
            Debug.Log("별4");
        }
        else if (timer <= 144 && timer > 108)
        {
            animator.Play(Star3);
            starCount = 3;
            Debug.Log("별3");
        }
        else if (timer <= 108 && timer > 72)
        {
            animator.Play(Star2);
            starCount = 2;
            Debug.Log("별2");
        }
        else if (timer <= 72 && timer > 36)
        {
            animator.Play(Star1);
            starCount = 1;
            Debug.Log("별1");
        }
        else
        {
            animator.Play(Star0);
            starCount = 0;
            Debug.Log("별0");
        }

        //ChangeAnimation();

        // ⭐ GameDataManager에 저장
        if (GameDataManager.Instance != null)
            GameDataManager.Instance.SetStar(sceneIndex, starCount);
        Debug.Log("starCount :" + starCount);

        GameDataManager.Instance.UploadGameData();
    }

    //void ChangeAnimation()
    //{
    //    if (nowAni != oldAni)
    //    {

    //        oldAni = nowAni;
    //        animator.Play(nowAni);
    //        Debug.Log("별이미지 변경");
    //    }
    //}
}