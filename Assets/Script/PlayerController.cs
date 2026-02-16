using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    [SerializeField] Vector3 mouse_position;

    [SerializeField] Vector3 mouse_world_position;



    void Update()
    {
        mouse_position = Mouse.current.position.ReadValue();
        mouse_world_position = Camera.main.ScreenToWorldPoint(mouse_position);
        //Debug.Log("Mouse Position: " + mouse_position);
        //Debug.Log("Mouse World Position: " + mouse_world_position); 
    }
}
