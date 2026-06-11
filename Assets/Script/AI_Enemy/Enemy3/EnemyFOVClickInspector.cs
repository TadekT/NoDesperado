using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyFOVClickInspector : MonoBehaviour
{
    [SerializeField] private InputActionReference inspectFOVAction;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    private void OnEnable()
    {
        inspectFOVAction.action.performed += OnInspectFOV;
        inspectFOVAction.action.Enable();
    }

    private void OnDisable()
    {
        inspectFOVAction.action.performed -= OnInspectFOV;
        inspectFOVAction.action.Disable();
    }

    private void OnInspectFOV(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayerMask)) return;

        FieldOfViewMesh fov = hit.collider.GetComponentInParent<FieldOfViewMesh>();
        if (fov == null)
            fov = hit.collider.transform.root.GetComponentInChildren<FieldOfViewMesh>();

        if (fov != null)
            fov.TogglePreview();
    }
}
