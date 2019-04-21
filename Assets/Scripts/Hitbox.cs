using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;

    public Vector3 halfExtent;

    public Vector3 offset;

    public enum ColliderState
    {
        Closed,
        Open,
        Colliding
    }

    [SerializeField]
    Color inactiveColor;

    [SerializeField]
    Color collisionOpenColor;

    [SerializeField]
    Color collidingColor;

    ColliderState state;

    IHitboxResponder responder = null;

    //public bool Open;

    Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        halfExtent = Vector3.zero;
        offset = Vector3.zero;
    }

    public void setResponder(IHitboxResponder responder)
    {
        this.responder = responder;
    }

    public void MaintainOrientation()
    {
        transform.localScale = parent.localScale;
    }

    public void startCheckingCollision()
    {
        //Debug.Log(state.ToString());
        state = ColliderState.Open;
        //Debug.Log(state.ToString());
    }

    public void stopCheckingCollision()
    {
        state = ColliderState.Closed;
    }

    public void hitboxUpdate()
    {
        if (state == ColliderState.Closed) { return; }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + offset, halfExtent * 2, transform.rotation.y, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D aCollider = colliders[i];

            // Make sure you are not hitting yourself
            if (aCollider.transform.parent != transform.parent)
            {
                responder?.collisionedWith(aCollider);
            }
        }

        state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
    }

    /*void CheckState()
    {
        if (Open)
        {
            startCheckingCollision();
        }
        else
        {
            stopCheckingCollision();
        }
    }*/

    void orientX()
    {
        if(transform.parent.localScale.x < 0)
        {
            offset.x = Mathf.Abs(offset.x) * -1;
        }
    }

    void Update()
    {
        // MaintainOrientation();
        //Debug.Log(Open.ToString());
        //Debug.Log("start hixbox update");
        //CheckState();
        //Debug.Log(state.ToString());
        orientX();
        hitboxUpdate();
       /*if (state == ColliderState.Closed)
        {
            return; 
        }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, halfExtent * 2, transform.rotation.y, layerMask);

        //if (layerMask == LayerMask.GetMask())
        //Debug.Log("Woooo");
        //Debug.Log(colliders.Length);
        if (colliders.Length > 0)
        {
            state = ColliderState.Colliding;
            Debug.Log("We hit something!");
        }
        else
        {
            state = ColliderState.Open;
        }*/
    }

    void OnDrawGizmos()
    {
        orientX();
        switch (state)
        {
            case ColliderState.Closed:
                Gizmos.color = inactiveColor;
                break;
            case ColliderState.Open:
                Gizmos.color = collisionOpenColor;
                break;
            case ColliderState.Colliding:
                Gizmos.color = collidingColor;
                break;
        }
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero + offset, new Vector3(halfExtent.x * 2, halfExtent.y * 2, halfExtent.z * 2)); // Because size is halfExtents
       // Gizmos.DrawWireCube(Vector3.zero + offset, new Vector3(halfExtent.x * 2, halfExtent.y * 2, halfExtent.z * 2)); // Because size is halfExtents
    }
}
