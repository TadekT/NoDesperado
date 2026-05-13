using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_mark_Signal : MonoBehaviour
{
    [SerializeField] private EnemyAI_Vision EnemyAI_Vision;
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
        if(EnemyAI_Vision == null)
        {
            EnemyAI_Vision = GetComponent<EnemyAI_Vision>();
        }
        audioSource = GetComponent<AudioSource>();

        meshRenderer.enabled = false;
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
    }

    private void PlaySuspiceSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

}
