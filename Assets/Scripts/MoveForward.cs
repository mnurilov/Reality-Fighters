using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;

    public bool IsGoingRight;

    void Start()
    {
        if (!IsGoingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = moveSpeed * Time.deltaTime;

        if (IsGoingRight)
        {
            transform.position = new Vector2(transform.position.x + dirX, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - dirX, transform.position.y);
        }
    }
}
