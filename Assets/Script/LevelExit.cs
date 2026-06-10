using UnityEngine;

public class LevelExit : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.HasKey)
        {
            GameManager.Instance.LevelComplete();
        }
        else
        {
            Debug.Log("Potrzebujesz klucza!");
        }
    }

}
