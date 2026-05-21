using UnityEngine;

public class Question_mark_Signal : MonoBehaviour
{
    [SerializeField] private EnemyAI_Brain enemyAI_Brain;
    [SerializeField] private MeshRenderer meshRenderer;

    private AudioSource audioSource;

    //bools variable
    private bool hasPlayedSuspiceSound = false;
    
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
            enemyAI_Brain.OnChangeState += HandleSuspiciousState;
        }   

    }

    private void OnDisable()
    {
        if(enemyAI_Brain != null)
        {
            enemyAI_Brain.OnChangeState -= HandleSuspiciousState;
        }  

    }

    private void ShowQuestionMark()
    {
        Debug.Log("Pokazuje pytajnik");
        meshRenderer.enabled = true;
        if (!hasPlayedSuspiceSound)
        {
            PlaySuspiceSound();
            hasPlayedSuspiceSound = true;
        }

    }


    private void HideQuestionMark()
    {
        meshRenderer.enabled = false;
        hasPlayedSuspiceSound = false;
        Debug.Log("Chowam Question Mark");
    }

    private void PlaySuspiceSound()
    {
        if(audioSource == null) return;
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void HandleSuspiciousState(EnemyAI_Brain.EnemyState previusState, EnemyAI_Brain.EnemyState currentState)
    {
        if(currentState == EnemyAI_Brain.EnemyState.Suspicious)
        {
            ShowQuestionMark();
        }
        else if(previusState == EnemyAI_Brain.EnemyState.Suspicious)
        {
            HideQuestionMark();
        }
    }

}
