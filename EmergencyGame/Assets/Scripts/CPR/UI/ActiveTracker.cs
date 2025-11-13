using UnityEngine;

public class ActiveTracker : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log($"{name} 활성화! (Scene: {gameObject.scene.name})");
        Debug.Log($"StackTrace:\n{System.Environment.StackTrace}");
    }

    void OnDisable()
    {
        Debug.Log($"{name} 비활성화! (Scene: {gameObject.scene.name})");
        Debug.Log($"StackTrace:\n{System.Environment.StackTrace}");
    }
}
