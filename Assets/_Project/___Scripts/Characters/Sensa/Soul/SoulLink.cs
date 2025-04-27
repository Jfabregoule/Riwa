using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SoulLink : MonoBehaviour
{
    [Header("References")]
    public Transform soul;
    public Transform body;

    [Header("Curve Settings")]
    [Range(4, 100)]
    public int segmentCount = 20;
    [Tooltip("Fraction of the soul-body distance to use as slack")]
    public float slack = 0.5f;
    [Tooltip("Speed of the wiggly noise along the chain")]
    public float jiggleSpeed = 4f;
    [Tooltip("Amplitude of the wiggly noise")]
    public float jiggleAmount = 0.2f;

    [SerializeField] private float _upOffset;

    private LineRenderer lr;
    private Vector3[] points;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = segmentCount;
        points = new Vector3[segmentCount];
    }

    void Update()
    {
        Vector3 A = soul.position + Vector3.up * _upOffset;
        Vector3 B = body.position + Vector3.up * _upOffset;

        Vector3 dir = B - A;
        Vector3 mid = (A + B) * 0.5f;

        Vector3 perp = Vector3.Cross(dir.normalized, Vector3.up);

        float sag = dir.magnitude * slack;

        float noise = (Mathf.PerlinNoise(Time.time * jiggleSpeed, 0f) - .5f) * 2f * jiggleAmount;

        Vector3 control = mid + perp * (sag + noise);

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (segmentCount - 1f);
            Vector3 P = (1 - t) * (1 - t) * A
                      + 2 * (1 - t) * t * control
                      + t * t * B;
            points[i] = P;
        }

        lr.SetPositions(points);
    }
}
