using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField]
    GameObject hitPicture;

    [SerializeField]
    string enemyName;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CapsuleCollider2D enemyCapsuleCollider = collision.gameObject.GetComponent<CapsuleCollider2D>();

        Animator enemyAnim = collision.gameObject.GetComponent<Animator>();

        // Throwing
        if (collision.gameObject.name.Equals(enemyName) && !enemyCapsuleCollider.enabled &&
            anim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            Debug.Log("throw");

            // Deal damage to enemy
            GameObject.Find(enemyName).GetComponent<PlayerController>().TakeDamage(30f);

            if (transform.localScale.x < 0)
            {
                collision.gameObject.transform.position = new Vector2(collision.gameObject.transform.position.x - 2, transform.position.y);
            }
            else
            {
                collision.gameObject.transform.position = new Vector2(collision.gameObject.transform.position.x + 2, transform.position.y);
            }
            enemyAnim.SetTrigger("knocked");
            GameObject.Find("Main Camera").GetComponent<ShakeObject>().TriggerShake();
        }
        // Guarding
        else if (GameObject.Find(enemyName).GetComponent<PlayerController>().IsGuarding == true)
        {
            Debug.Log("guard");

            if (transform.localScale.x < 0)
            {
                transform.position = new Vector2(collision.gameObject.transform.position.x + 3, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(collision.gameObject.transform.position.x - 3, transform.position.y);
            }
        }
        // Normal Hit
        else if (collision.gameObject.name.Equals(enemyName) && !enemyCapsuleCollider.enabled)
        {
            Debug.Log("kick");
            // Deal damage to enemy
            GameObject.Find(enemyName).GetComponent<PlayerController>().TakeDamage(10f);

            if (transform.localScale.x < 0)
            {
                Instantiate(hitPicture, new Vector2(transform.position.x - 1f, transform.position.y), Quaternion.identity);
            }
            else
            {
                Instantiate(hitPicture, new Vector2(transform.position.x + 1f, transform.position.y), Quaternion.identity);
            }

            GameObject.Find(enemyName).GetComponent<PlayerController>().Stunned = true;
        }
    }
}
