using UnityEngine;

public class ActiveTracker : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log($"{name} »∞º∫»≠µ ! (Scene: {gameObject.scene.name})");
        Debug.Log($"StackTrace:\n{System.Environment.StackTrace}");
    }

    void OnDisable()
    {
        Debug.Log($"{name} ∫Ò»∞º∫»≠µ ! (Scene: {gameObject.scene.name})");
        Debug.Log($"StackTrace:\n{System.Environment.StackTrace}");
    }
}
