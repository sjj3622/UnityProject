using UnityEngine;

public class LockController : MonoBehaviour
{
    private Animator animator;
    public GameObject Enemy;  // 씬에 있는 Enemy 오브젝트

    [Header("Animation Names")]
    public string Lock = "Lock";
    public string Unlock = "unlock";



    private bool locked = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        CheckEnemy();
    }

    void CheckEnemy()
    {
        if (Enemy != null)
        {
            if (!locked)
            {
                animator.Play(Lock);
                locked = true;
            }
        }
        else
        {
            if (locked)
            {
                animator.Play(Unlock);
                locked = false;

                //// Enemy가 없으므로 게임 상태 변경
                //HMgpManager.gameState = "HMClear";
            }
        }
    }
}
