using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Vision : MonoBehaviour
{

    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private Transform _eyes;
    //[SerializeField] private float rayDistance = 20f;


    [Header("Sphere Cast arg")]
    [SerializeField] private float sphereRadius = 10f;
    [SerializeField] private int maxColliderSize = 10;
    [SerializeField] private float scanIntervl = 1f;
    private Collider[] _hitColliders;
    
    private Coroutine CheckingTheSurroundingsReference;


    private void Awake()
    {
        _hitColliders = new Collider[maxColliderSize];
    }

    private void Start()
    {
        if(_eyes == null)
        {
            Debug.Log("THER IS NO EYES TRANSFORM");
            _eyes = transform;
        }   
        CheckingTheSurroundingsReference = StartCoroutine(CheckingTheSurroundings());

    }



    private IEnumerator CheckingTheSurroundings()
    {
        while (true)
        {
            SphereScan();
            yield return new WaitForSecondsRealtime(1f);
        }
    }


    private  bool SphereScan()
    {

        
        int scanColl = Physics.OverlapSphereNonAlloc(transform.position, sphereRadius ,_hitColliders,_playerLayer);
        
        if (scanColl > 0)
        {
            Debug.Log("Player IN SPHERE RANGE ");
            return true;
        }
        else
        {
            Debug.Log("Sphere find shiit ");
            return false;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,sphereRadius);

    }

}
