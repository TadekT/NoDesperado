using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerInputHadler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;
    

    [Header("Action Map Name Reference")]
    [SerializeField] private string actiomMapName = "Player"; 

    [Header("Action Name Refereces")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    //[SerializeField] private string point = "Point";
    //[SerializeField] private string click = "Click";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    //private InputAction pointAction;
    //private InputAction clickAction;


    public Vector2 MoveInput {get; private set;}
    public Vector2 LookInput {get; private set;}
    public bool JumpTriggered {get; private set;}
    public float SprintValue {get; private set;}

    public static PlayerInputHadler Instance {get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    //Finding acitons map and attache them to InputAction variables

    moveAction = playerControls.FindActionMap(actiomMapName).FindAction(move);
    lookAction = playerControls.FindActionMap(actiomMapName).FindAction(look);
    jumpAction = playerControls.FindActionMap(actiomMapName).FindAction(jump);
    sprintAction = playerControls.FindActionMap(actiomMapName).FindAction(sprint);
    //pointAction = playerControls.FindActionMap(actiomMapName).FindAction(point);
    //clickAction =playerControls.FindActionMap(actiomMapName).FindAction(click);
    }

    private void RegisterInputAction()
    {

        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => MoveInput = Vector2.zero;


    }


    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        //pointAction.Enable();
        //clickAction.Enable();

    }

    private void OnDisable()
    {
        lookAction.Disable();
        jumpAction.Disable();
        moveAction.Disable();
        sprintAction.Disable();
        //pointAction.Disable();
        //clickAction.Disable();
    }


}

