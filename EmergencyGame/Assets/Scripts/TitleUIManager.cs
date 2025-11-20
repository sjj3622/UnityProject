using UnityEngine;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    public Image[] starImages; // 각 씬별 별 이미지
    public Sprite[] starSprites; // 0,1,2,3,4 등 별 상태에 따른 스프라이트

    void Start()
    {
        GameDataManager.EnsureExists();

        // 초기화용 디버그
        //for (int i = 0; i < GameDataManager.Instance.gameData.starLevels.Length; i++)
        //{
        //    Debug.Log($"Initial star[{i}] = {GameDataManager.Instance.gameData.starLevels[i]}");
        //}

        UpdateStars();
        StartCoroutine(GameDataManager.Instance.UploadGameData());
    }


    public void UpdateStars()
    {
        if (GameDataManager.Instance == null || GameDataManager.Instance.gameData == null)
        {
            Debug.LogWarning("GameDataManager 또는 gameData가 존재하지 않습니다! 기본값 사용");
            foreach (var img in starImages)
                img.sprite = starSprites[0];
            return;
        }

        int sceneCount = GameDataManager.Instance.gameData.starLevels.Length;

        for (int i = 0; i < starImages.Length; i++)
        {
            int stars = 0;

            if (i < sceneCount)
                stars = GameDataManager.Instance.GetStar(i);

            stars = Mathf.Clamp(stars, 0, starSprites.Length - 1);

            //Debug.Log("stars :" + stars);
            starImages[i].sprite = starSprites[stars];

            //Debug.Log("Star Image " + i + " : " + starImages[i].name + " assigned sprite: " + starImages[i].sprite.name);
        }
    }
}