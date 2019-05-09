using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    Transform parentObject;

    public BoxCollider2D hurtBoxCollider;

    public enum ColliderState
    {
        NotHit,
        Hit
    }

    ColliderState state;

    [SerializeField]
    Color colliderStateNotHitColor;

    [SerializeField]
    Color colliderStateHitColor;

    public void StateHit()
    {
        state = ColliderState.Hit;
    }

    public void StateNotHit()
    {
        state = ColliderState.NotHit;
    }

    public void MaintainOrientation()
    {
        transform.localScale = -1 * parentObject.localScale;
    }

    void Start()
    {
        parentObject = transform.parent;
        //hurtBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
       //MaintainOrientation();
    }

    public void getHitBy(int damage, float stunTime)
    {
        PlayerController playerController = parentObject.GetComponent<PlayerController>();
        playerController.TakeDamage(damage);
        playerController.StunPlayer(stunTime);
        Debug.Log("I got hit");
        // Do something with the damage and the state
    }

    private void OnDrawGizmos()
    {
        switch (state)
        {
            case ColliderState.NotHit:
                Gizmos.color = colliderStateNotHitColor;
                break;
            case ColliderState.Hit:
                Gizmos.color = colliderStateHitColor;
                break;
        }
        //Gizmos.matrix = Matrix4x4.TRS(transform.parent.position, transform.parent.transform.rotation, transform.parent.transform.localScale);
        //Gizmos.DrawCube(hurtBoxCollider.bounds.center, new Vector3(hurtBoxCollider.bounds.extents.x * 2, hurtBoxCollider.bounds.extents.y * 2, hurtBoxCollider.bounds.extents.z * 2)); // Because size is halfExtents
        Gizmos.DrawWireCube(hurtBoxCollider.bounds.center, new Vector3(hurtBoxCollider.bounds.extents.x * 2, hurtBoxCollider.bounds.extents.y * 2, hurtBoxCollider.bounds.extents.z * 2)); // Because size is halfExtents
    }
}
