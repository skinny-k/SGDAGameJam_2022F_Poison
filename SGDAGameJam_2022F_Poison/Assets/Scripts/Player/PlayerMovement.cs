using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementSpeed = 1f;

    GameObject _moveTarget_obj;
    Vector3 _moveTarget_pos = new Vector3(0, 0, -100);
    Rigidbody2D _rb;
    Vector3 _movementThisFrame = Vector3.zero;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        MoveManually();
    }

    void MoveManually()
    {
        _movementThisFrame.x = Input.GetAxis("Horizontal");
        _movementThisFrame.y = Input.GetAxis("Vertical");
        _movementThisFrame.z = 0;

        _movementThisFrame = _movementThisFrame.normalized;
        _movementThisFrame *= _movementSpeed * Time.deltaTime;

        _rb.MovePosition(transform.position + _movementThisFrame);
    }

    void SetNewMoveTarget(GameObject objClicked, Vector3 targetPosition)
    {
        if (objClicked != null)
        {
            _moveTarget_obj = objClicked;
        }
        _moveTarget_pos = new Vector3(targetPosition.x, targetPosition.y, 0);
    }
}
