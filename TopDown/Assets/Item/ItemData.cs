using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Arrow,
    Key,
    Life,
}




public class ItemData : MonoBehaviour
{
    public ItemType Type;
    public int count = 1;
    public int arrangle = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // if (Type == ItemType.Arrow) ItemKeeper.hasArrows += count;
            switch (Type)
            {
                case ItemType.Arrow:
                    ItemKeeper.hasArrows += count; break;
                case ItemType.Key:
                    ItemKeeper.hasKeys += count; break;
                case ItemType.Life:
                    PlayerController.hp++;
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                    break;
            }

            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 2.5f;
            rb.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);

            SaveDataManager.SetArrangeId(arrangle, gameObject.tag);
        
        }
    }


} 