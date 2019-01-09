using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;

    private Rigidbody _rb;
    private Transform _transform;
    private Camera _cam;
    private Animator _animator;
    private int _floorMask;
    private float _camRayLength = 100f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = Camera.main;
        _transform = transform;
        _animator = GetComponent<Animator>();
        _floorMask = LayerMask.GetMask("Ground");
        //_cam.GetComponent<CameraController>().target = transform;
    }

    void FixedUpdate()
    {
        this.MovePlayer();
    }

    void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, _camRayLength, _floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = hit.point - _transform.position;
            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;
            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);
            // Set the player's rotation to this new rotation.
            _rb.MoveRotation(newRotatation);
        }
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        Vector3 tempMovement = _transform.InverseTransformDirection(movement);
        this.AnimateMove(tempMovement.x, tempMovement.z);

        movement = movement.normalized * speed * Time.fixedDeltaTime;
        _rb.MovePosition(_transform.position + movement);
    }

    void AnimateMove(float hor, float vert)
    {
        _animator.SetFloat("right", hor);
        _animator.SetFloat("forward", vert);
    }
}
