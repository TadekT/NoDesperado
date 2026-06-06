using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference rotateAction;
    [SerializeField] private InputActionReference zoomAction;


    private void OnEnable()
    {
        moveAction?.action.Enable();
        rotateAction?.action.Enable();
        zoomAction?.action.Enable();
    }

    private void OnDisable()
    {
        moveAction?.action.Disable();
        rotateAction?.action.Disable();
        zoomAction?.action.Disable();

    }



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
        if (moveAction == null) return;
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    
    private void HandleRotation()
    {
        if (rotateAction == null) return;
        float rotate = rotateAction.action.ReadValue<float>();
        transform.eulerAngles += new Vector3(0, rotate * rotateSpeed * Time.deltaTime , 0);
    }

    private void HandleZoom()
    {
        if (zoomAction == null) return;
        float scroll = zoomAction.action.ReadValue<Vector2>().y;

        if (scroll > 0f) targetFollowOffset.y -= zoomAmount;
        else if (scroll < 0f) targetFollowOffset.y += zoomAmount;
        
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, minZoom, maxZoom);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);

    }
}