using UnityEngine;
using System.Collections.Generic;

public class DroneFollower : MonoBehaviour
{
    [Header("Target")]
    public Transform followAnchor;

    [Header("Sampling")]
    public int directionsCount = 32;
    [Range(10f, 120f)] public float coneAngle = 60f;

    [Header("Look Ahead")]
    public float lookAheadBase = 2.0f;
    public float lookAheadSpeedFactor = 0.5f;

    [Header("Collision")]
    public float clearanceRadius = 0.3f;
    public LayerMask obstacleMask;

    [Header("Motion Limits")]
    public float maxSpeed = 5f;
    public float maxAccel = 8f;
    public float maxTurnRateDeg = 180f;

    [Header("Scoring Weights")]
    public float wFollow = 0.30f;
    public float wLoS = 0.20f;
    public float wDyn = 0.10f;
    public float wSafe = 0.40f;

    [Header("Stabilization")]
    public float directionSmoothing = 10f;
    public float hysteresisThreshold = 0.05f;

    // Internal state
    Vector3 currentVelocity;
    Vector3 previousDirection;
    Vector3 smoothedDirection;
    float previousBestScore = float.MaxValue;

    void Start()
    {
        smoothedDirection = transform.forward;
        previousDirection = smoothedDirection;
    }

    void Update()
    {
        if (!followAnchor) return;

        float speed = currentVelocity.magnitude;
        float lookAhead = lookAheadBase + lookAheadSpeedFactor * speed;

        Vector3 d0 = (followAnchor.position - transform.position).normalized;
        List<Vector3> candidates = GenerateDirections(d0);

        Vector3 bestDir = Vector3.zero;
        float bestScore = float.MaxValue;

        foreach (var dir in candidates)
        {
            if (!IsDirectionFeasible(dir, lookAhead, out RaycastHit hit))
                continue;

            float score = ScoreDirection(dir, lookAhead, hit);

            if (score < bestScore)
            {
                bestScore = score;
                bestDir = dir;
            }
        }

        // Fallback
        if (bestDir == Vector3.zero)
        {
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                Vector3.zero,
                Time.deltaTime * 2f
            );
            transform.position += currentVelocity * Time.deltaTime;
            return;
        }

        // Hysteresis
        if (Mathf.Abs(bestScore - previousBestScore) / previousBestScore < hysteresisThreshold)
            bestDir = previousDirection;

        previousBestScore = bestScore;

        // Smoothing
        smoothedDirection = Vector3.Slerp(
            smoothedDirection,
            bestDir,
            Time.deltaTime * directionSmoothing
        );

        // Velocity integration
        Vector3 desiredVelocity = smoothedDirection * maxSpeed;
        currentVelocity = Vector3.MoveTowards(
            currentVelocity,
            desiredVelocity,
            maxAccel * Time.deltaTime
        );

        transform.position += currentVelocity * Time.deltaTime;
        previousDirection = smoothedDirection;
    }

    // -----------------------------
    // Direction Generation
    // -----------------------------
    List<Vector3> GenerateDirections(Vector3 d0)
    {
        List<Vector3> dirs = new List<Vector3>(directionsCount);
        Vector3 right = Vector3.Cross(d0, Vector3.up);
        if (right.sqrMagnitude < 0.01f)
            right = Vector3.Cross(d0, Vector3.forward);
        Vector3 up = Vector3.Cross(right, d0);

        for (int i = 0; i < directionsCount; i++)
        {
            float t = i / (float)(directionsCount - 1);
            float azimuth = Mathf.Lerp(-coneAngle * 0.5f, coneAngle * 0.5f, t);
            float elevation = Random.Range(-coneAngle * 0.5f, coneAngle * 0.5f);

            Quaternion rot =
                Quaternion.AngleAxis(azimuth, up) *
                Quaternion.AngleAxis(elevation, right);

            dirs.Add((rot * d0).normalized);
        }

        return dirs;
    }

    // -----------------------------
    // Hard constraint
    // -----------------------------
    bool IsDirectionFeasible(Vector3 dir, float L, out RaycastHit hit)
    {
        return !Physics.SphereCast(
            transform.position,
            clearanceRadius,
            dir,
            out hit,
            L,
            obstacleMask,
            QueryTriggerInteraction.Ignore
        );
    }

    // -----------------------------
    // Scoring
    // -----------------------------
    float ScoreDirection(Vector3 dir, float L, RaycastHit hit)
    {
        float score = 0f;

        // Follow
        Vector3 predictedPos = transform.position + dir * L;
        float followDist = Vector3.Distance(predictedPos, followAnchor.position);
        score += wFollow * followDist;

        // Line of Sight
        if (Physics.Raycast(
            transform.position,
            (followAnchor.position - transform.position).normalized,
            out RaycastHit losHit,
            Vector3.Distance(transform.position, followAnchor.position),
            obstacleMask))
        {
            score += wLoS;
        }

        // Dynamics
        float angle = Vector3.Angle(previousDirection, dir);
        float turnPenalty = angle / maxTurnRateDeg;
        score += wDyn * turnPenalty;

        // Soft safety (near miss)
        if (hit.collider != null)
        {
            float safety = 1f - (hit.distance / L);
            score += wSafe * safety;
        }

        return score;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, clearanceRadius);
    }
#endif
}