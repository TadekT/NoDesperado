using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Cinemachine namespace dla kontroli kamery

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;


    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float zoomAmount = 1f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;


    void Start()
    {

        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;

    }


    void Update()
    {
        // Handle Inputs
        HandleMovement();
        HandleRotation();
        HandleZoom();

    }
        
    private void HandleMovement()
    {
        Vector3 inputMoveDirection = new Vector3(0, 0,0);
        if(Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z += 1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z -= 1f;
        }
        if(Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x -= 1f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x += 1f;
        }

        Vector3 moveDirection = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0,0,0);
        if(Input.GetKey(KeyCode.Q))
        {
            rotationVector.y -= 1f;
        }
        if(Input.GetKey(KeyCode.E))
        {
            rotationVector.y += 1f;
        }
        transform.eulerAngles += rotationVector * rotateSpeed * Time.deltaTime;
    }
    private void HandleZoom()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }
        
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, minZoom, maxZoom);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);

    }
}