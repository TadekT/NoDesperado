using UnityEngine;

public class Exclamation_mark_signal : MonoBehaviour
{
    [SerializeField] private EnemyAI_Brain enemyAI_Brain;
    [SerializeField] private MeshRenderer meshRenderer;

    private AudioSource audioSource;

    //bools variable
    private bool hasPlayedChaseSound = false;
    
    private void Awake()
    {
        if(meshRenderer == null)
        {
            meshRenderer =  GetComponent<MeshRenderer>();
        }
        if(enemyAI_Brain == null)
        {
            enemyAI_Brain = GetComponent<EnemyAI_Brain>();
        }
        audioSource = GetComponent<AudioSource>();

        meshRenderer.enabled = false;
    }

    private void OnEnable()
    {
        if(enemyAI_Brain != null)
        {
            enemyAI_Brain.OnChangeState += HandleChaseState;
        }   

    }

    private void OnDisable()
    {
        if(enemyAI_Brain != null)
        {
            enemyAI_Brain.OnChangeState -= HandleChaseState;
        }  

    }

    private void ShowExclamationMark()
    {
        Debug.Log("Pokazuje Wykrzyknik");
        meshRenderer.enabled = true;
        if (!hasPlayedChaseSound)
        {
            PlayChaseSound();
            hasPlayedChaseSound = true;
        }

    }


    private void HideExclamationMark()
    {
        meshRenderer.enabled = false;
        hasPlayedChaseSound = false;
        Debug.Log("Chowam Wykrzyknik");
    }

    private void PlayChaseSound()
    {
        if(audioSource == null) return;
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void HandleChaseState(EnemyAI_Brain.EnemyState previusState, EnemyAI_Brain.EnemyState currentState)
    {
        if(currentState == EnemyAI_Brain.EnemyState.Chase)
        {
            ShowExclamationMark();
        }
        else if(previusState == EnemyAI_Brain.EnemyState.Chase)
        {
            HideExclamationMark();
        }
    }

}

