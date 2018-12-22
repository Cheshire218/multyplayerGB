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
        float mod = 1;
        //if (moveVertical < 0) mod = 0.70f;
        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);





        Vector3 forw = _transform.TransformDirection(Vector3.forward);
        Vector3 transformRight = _transform.TransformDirection(Vector3.right);
        //float forwAndUp = Vector3.Dot(forw, Vector3.forward);
        //float forwAndRight = Vector3.Dot(forw, Vector3.right);

        float animMoveHorizontal = moveHorizontal;
        float animMoveVertical = moveVertical;

        float angle = Vector3.Angle(forw, Vector3.forward);


        //право-лево смотрим
        if (angle > 45 && angle < 135)
        {
            animMoveHorizontal = moveVertical;
            animMoveVertical = moveHorizontal;

            //смотрим лево
            if (Vector3.Dot(Vector3.right, forw) < 0)
            {
                animMoveVertical = -animMoveVertical;
                if(animMoveVertical != 0)
                {
                    animMoveHorizontal = animMoveHorizontal - forw.z;
                }
                else if(animMoveHorizontal != 0)
                {
                    animMoveVertical = animMoveVertical + forw.z;
                }
            }
            else //смотрим право
            {
                animMoveHorizontal = -animMoveHorizontal;
                Debug.Log(forw.z);
                if (animMoveVertical != 0)
                {
                    animMoveHorizontal = animMoveHorizontal + forw.z;
                }
                else if (animMoveHorizontal != 0)
                {
                    animMoveVertical = animMoveVertical - forw.z;
                }
                
            }

        }
        else //смотрим верх низ
        {
            //смотрим низ
            if (Vector3.Dot(Vector3.forward, forw) < 0)
            {
                animMoveVertical = -animMoveVertical;
                animMoveHorizontal = -animMoveHorizontal;
                if (animMoveVertical != 0)
                {
                    animMoveHorizontal = animMoveHorizontal + forw.x;
                }
            }
            else//смотрим право
            {
                if (animMoveVertical != 0)
                {
                    animMoveHorizontal = animMoveHorizontal - forw.x;
                }
            }
            

        }
        
        this.AnimateMove(animMoveHorizontal, animMoveVertical);


        movement = movement.normalized * speed * mod * Time.fixedDeltaTime;
        _rb.MovePosition(_transform.position + movement);
    }

    void AnimateMove(float hor, float vert)
    {
        _animator.SetFloat("right", hor);
        _animator.SetFloat("forward", vert);
    }
}
