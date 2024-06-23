using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D _myRigidBody;
    private Vector3 _change;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _change = Vector3.zero;
        // GetAxisRaw is used as it returns a value of 1, 0, or -1. GetAxis returns a float.
        // So, GetAxisRaw is more responsive to player input due to the binary movement.
        // GetAxis is gradual in the context of movement while GetAxisRaw creates immediate movement.
        _change.x = Input.GetAxisRaw("Horizontal");
        _change.y = Input.GetAxisRaw("Vertical");
        UpdateAnimationAndMove();
    }

    void UpdateAnimationAndMove()
    {
        if (_change != Vector3.zero)
        {
            MoveCharacter();
            _animator.SetFloat("moveX", _change.x);
            _animator.SetFloat("moveY", _change.y);
            _animator.SetBool("moving", true);
        }
        else
        {
            _animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        _myRigidBody.MovePosition(
           transform.position + _change * speed * Time.deltaTime);
    }
}
