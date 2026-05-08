using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question_mark_Signal : MonoBehaviour
{
    [SerializeField] private EnemyAI_Vision PlayerInFovActionSigal;

    [SerializeField] private MeshRenderer meshRenderer;

    private void Awake()
    {
        if(meshRenderer == null)
        {
            meshRenderer =  GetComponent<MeshRenderer>();
        }
        meshRenderer.enabled = false;
    }

    private void OnEnable()
    {
        PlayerInFovActionSigal.OnPlayerInFovAction += ShowQuestionMark;
    }

    private void OnDisable()
    {
        PlayerInFovActionSigal.OnPlayerInFovAction -= ShowQuestionMark;
    }

    private void ShowQuestionMark()
    {
        Debug.Log("Pokazuje pytajnik");
        meshRenderer.enabled = true;

    }

}
