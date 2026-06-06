using UnityEngine;

public class BushHide : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        PlayerStatus status = other.GetComponent<PlayerStatus>();
        if(status != null)
        {
            status.SetHidden(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        PlayerStatus status = other.GetComponent<PlayerStatus>();
        if(status != null)
        {
            status.SetHidden(false);
        }

    }




}
