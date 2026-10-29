using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Commands a NavMeshAgent toward a target and draws debug gizmos.
/// Attach to the NPC capsule. Assign the target in the inspector.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class NPCNavigationTester : MonoBehaviour
{
    [Header("Navigation")]
    public Transform target;
    public bool navigateOnStart = true;

    [Header("Debug Display")]
    public bool showPath = true;
    public bool showVelocity = true;
    public Color pathColor = Color.yellow;
    public Color velocityColor = Color.red;

    NavMeshAgent _agent;

    void Awake() => _agent = GetComponent<NavMeshAgent>();

    void Start()
    {
        if (navigateOnStart && target != null)
            GoToTarget();
    }

    public void GoToTarget()
    {
        if (target == null) { Debug.LogWarning("NPCNavigationTester: no target assigned."); return; }
        _agent.SetDestination(target.position);
    }

    public void ResetAndGo(Vector3 startPos)
    {
        _agent.ResetPath();
        _agent.Warp(startPos);
        GoToTarget();
    }

    // ponytail: OnGUI kept intentionally simple — no canvas needed
    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 180, 40), "Navigate to Target"))
            GoToTarget();

        if (GUI.Button(new Rect(20, 70, 180, 40), "Reset Position"))
            ResetAndGo(transform.position);

        var status = _agent.pathStatus.ToString();
        if (_agent.pathPending) status = "Computing…";
        else if (_agent.remainingDistance < 0.5f && !_agent.pathPending) status = "Arrived";
        GUI.Label(new Rect(20, 120, 400, 30),
            $"Status: {status}  |  Speed: {_agent.velocity.magnitude:F1}  |  Remaining: {_agent.remainingDistance:F1}m");
    }

    void OnDrawGizmos()
    {
        if (_agent == null || !_agent.hasPath) return;

        if (showPath)
        {
            Gizmos.color = pathColor;
            var corners = _agent.path.corners;
            for (int i = 0; i < corners.Length - 1; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);
                Gizmos.DrawSphere(corners[i], 0.15f);
            }
            if (corners.Length > 0) Gizmos.DrawSphere(corners[^1], 0.25f);
        }

        if (showVelocity)
        {
            Gizmos.color = velocityColor;
            Gizmos.DrawRay(transform.position + Vector3.up, _agent.velocity);
        }
    }
}
