using UnityEngine;

public class OptimizedAnimationCulling : MonoBehaviour
{
    private Camera mainCamera;
    private Animator animator;
    private Transform objectTransform;

    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        objectTransform = transform;

        if (animator == null)
        {
            Debug.LogWarning($"No se encontró un Animator en el objeto {gameObject.name}");
        }
    }

    void Update()
    {
        if (animator != null)
        {
            // Verifica si el objeto está dentro del campo de visión de la cámara
            if (IsInView())
            {
                animator.enabled = true;
            }
            else
            {
                animator.enabled = false;
            }
        }
    }

    private bool IsInView()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(objectTransform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}

