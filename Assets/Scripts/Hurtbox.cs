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

    public bool CheckIfGuarding()
    {
        if (parentObject.GetComponent<MichaelPlayerController>().IsGuarding)
        {
            return true;
        }
        return false;
    }

    public void getHitBy(int damage, float stunTime, string attack = "")
    {
        MichaelPlayerController michaelPlayerController = parentObject.GetComponent<MichaelPlayerController>();
        michaelPlayerController.TakeDamage(damage);
        michaelPlayerController.StunPlayer(stunTime);

        if (attack == "light")
        {
            michaelPlayerController.anim.SetTrigger("facehit");
        }
        else if (attack == "medium")
        {
            michaelPlayerController.anim.SetTrigger("bodyhit");
        }
        else if (attack == "heavy")
        {
            michaelPlayerController.anim.SetTrigger("knockdown");
            if(michaelPlayerController.CurrentHealth <= 0)
            {
                michaelPlayerController.anim.SetBool("dead", true);
            }
        }

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
