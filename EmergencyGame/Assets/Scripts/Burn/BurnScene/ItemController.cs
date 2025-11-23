using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int itemIndex; // DropController에서 자동으로 세팅됨
    public delegate void ItemCollected(int index, BPlayerController player);
    public event ItemCollected OnCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        BPlayerController player = other.GetComponent<BPlayerController>();
        if (player != null)
            OnCollected?.Invoke(itemIndex, player);

        Destroy(gameObject);
    }
}
