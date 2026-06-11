using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = cam.transform.rotation;
    }
}
