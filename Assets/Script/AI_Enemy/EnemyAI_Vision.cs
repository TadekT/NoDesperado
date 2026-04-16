using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Vision : MonoBehaviour
{

    [SerializeField] private LayerMask _playerLayer;

    [SerializeField] private Transform _eyes;
    //[SerializeField] private float rayDistance = 20f;


    [Header("Sphere Cast arg")]
    [SerializeField] private float sphereMaxDistance = 20f;
    [SerializeField] private float sphereRadius = 10f;
    private List<Collider> detectedPlayerHits = new List<Collider>();


    private Coroutine CheckingTheSurroundingsReference;
    private void Start()
    {
        if(_eyes == null)
        {
            Debug.Log("THER IS NO EYES TRANSFORM");
        }   
        StartCoroutine(CheckingTheSurroundings());

    }



    private IEnumerator CheckingTheSurroundings()
    {
        SphereCheck();
        yield return new WaitForSecondsRealtime(1f);
    }


    private  bool SphereCheck()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.forward * sphereMaxDistance;
        
        detectedPlayerHits.Clear();

        RaycastHit hit;

        bool playerWasHit = Physics.SphereCast(startPos, sphereRadius, transform.forward, out hit ,sphereMaxDistance, _playerLayer);

        if (playerWasHit)
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



    //     private void OnDrawGizmos()
    // {
    //     Vector3 start = transform.position;
    //     Vector3 dir = transform.forward.normalized;
    //     Vector3 end = start + dir * sphereMaxDistance;

    //     Gizmos.color = Color.cyan;

    //     // kula startowa
    //     Gizmos.DrawWireSphere(start, sphereRadius);

    //     // kula końcowa
    //     Gizmos.DrawWireSphere(end, sphereRadius);

    //     // linia środka castu
    //     Gizmos.DrawLine(start, end);
    // }

}
