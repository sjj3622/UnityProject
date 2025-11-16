using UnityEngine;

public class ColliderDebug2D : MonoBehaviour
{
    void Start()
    {
        Debug.Log("ColliderDebug2D active on " + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name + " triggered by " + other.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.name + " collided with " + collision.gameObject.name);
    }
}
