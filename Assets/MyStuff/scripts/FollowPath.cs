using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public GameObject path;
    private Transform[] points;
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float rotationSpeed = 60f; // degrees per second
    public float raycastHeight = 2f;
    public float raycastDistance = 10f;
    public LayerMask groundMask;

    private int index = 0;
    private float currentSpeed = 0f;
    private Quaternion smoothRotation;

    void Start()
    {
        smoothRotation = transform.rotation;
    }

    void Update()
    {
        if (points == null || points.Length == 0)
        {
            if (path != null)
            {
                points = new Transform[path.transform.childCount];
                for (int i = 0; i < path.transform.childCount; i++)
                {
                    points[i] = path.transform.GetChild(i);
                }
            }
            else
            {
                points = new Transform[0];
            }
        }
        if (points.Length == 0) return;

        Transform target = points[index];

        // Get flat direction to the target
        Vector3 toTarget = (target.position - transform.position);
        Vector3 flatTargetDirection = new(toTarget.x, 0f, toTarget.z);

        // Smoothly ease rotation towards target
        if (flatTargetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatTargetDirection.normalized);

            // Easing: use Slerp for smooth interpolation
            float t = 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime * 0.1f); // ease-in
            smoothRotation = Quaternion.Slerp(smoothRotation, targetRotation, t);
            transform.rotation = smoothRotation;
        }

        // Calculate turning angle to reduce speed
        float angle = Vector3.Angle(transform.forward, flatTargetDirection.normalized);
        float turnFactor = Mathf.InverseLerp(0f, 90f, angle); // 0 when facing target, 1 when perpendicular
        float targetSpeed = Mathf.Lerp(maxSpeed, maxSpeed * 0.3f, turnFactor); // slow down when turning

        // Smooth speed changes
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        // Move forward (in the facing direction, not directly to the target)
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // Switch to next point when close (horizontal distance only)
        Vector3 flatPos = new(transform.position.x, 0f, transform.position.z);
        Vector3 flatTarget = new(target.position.x, 0f, target.position.z);
        if (Vector3.Distance(flatPos, flatTarget) < 0.5f)
        {
            index = (index + 1) % points.Length;
        }

        // Stay flush with ground
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeight;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastDistance, groundMask))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
        }

        // Debug: forward direction
        Debug.DrawLine(transform.position, transform.position + transform.forward * 3f, Color.green);
    }

    // Draw debug line to current target in the scene editor
    private void OnDrawGizmos()
    {
        if (points != null && points.Length > 0 && index < points.Length && points[index] != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, points[index].position);
        }
    }

}
