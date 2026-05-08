using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToWorldPosition : MonoBehaviour
{
    public static MouseToWorldPosition instance{ get; private set;}
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);

        }
        else
        {
            instance = this;
        }


    }


    public Vector3 MouseWorldPosition()
    {
        //Shooting ray from main camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //What ray hit?
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, instance.groundLayer))
        {
        Debug.Log(" Mouse World POsiton : " + hit.point);
        return hit.point;
            
        }
    
    return Vector3.zero;

    }


}
