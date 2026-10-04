using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bee : MonoBehaviour
{
    public Transform start {get;private set;}
    public Transform destination {get;private set;}
    private bool sent;

    [SerializeField] private float moveSpeed;

    [Header("Flight Path")]
    [Tooltip("Max sideways bend of the overall path, as a fraction of the trip distance")]
    [SerializeField] private float maxCurve = 0.3f;
    [Tooltip("Random +/- percentage applied to moveSpeed per bee")]
    [SerializeField] private float speedVariance = 0.1f;

    [Header("Buzz")]
    [Tooltip("How far the bee wobbles off its path")]
    [SerializeField] private float buzzAmplitude = 0.15f;
    [Tooltip("How fast the wobble changes")]
    [SerializeField] private float buzzFrequency = 2f;

    [Header("Facing")]
    [Tooltip("Degrees per second the bee can turn toward its flight direction")]
    [SerializeField] private float turnSpeed = 540f;
    [Tooltip("Rotation offset for the sprite's art: 0 if it faces right, -90 if it faces up")]
    [SerializeField] private float spriteAngleOffset = 0f;

    private SpriteRenderer spriteRenderer;
    private Vector3 startPos;
    private Vector3 controlOffset;
    private float speed;
    private float progress;
    private float noiseSeed;

    public void Send(Transform s, Transform d)
    {
        start = s;
        destination = d;

        startPos = transform.position;
        speed = moveSpeed * (1f + Random.Range(-speedVariance, speedVariance));
        noiseSeed = Random.Range(0f, 1000f);

        // Bend the path to a random side so each bee takes its own arc
        Vector3 toDest = d.position - startPos;
        Vector3 perpendicular = new Vector3(-toDest.y, toDest.x, 0f).normalized;
        controlOffset = perpendicular * toDest.magnitude * Random.Range(-maxCurve, maxCurve);

        // Start facing along the curve's initial direction so there's no snap on takeoff
        Vector3 control = (startPos + d.position) * 0.5f + controlOffset;
        transform.rotation = FacingRotation(control - startPos);
        UpdateFlip();

        progress = 0f;
        sent = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!sent) return;

        Vector3 endPos = destination.position;
        Vector3 control = (startPos + endPos) * 0.5f + controlOffset;

        // Advance along the curve at roughly constant world speed
        float pathLength = Vector3.Distance(startPos, endPos) + controlOffset.magnitude;
        progress = Mathf.Min(1f, progress + speed * Time.deltaTime / Mathf.Max(pathLength, 0.01f));

        Vector3 pathPos = QuadraticBezier(startPos, control, endPos, progress);

        // Perlin wobble on both axes; fades out at the ends so the bee leaves and lands cleanly
        float t = Time.time * buzzFrequency;
        Vector3 buzz = new Vector3(
            Mathf.PerlinNoise(noiseSeed, t) - 0.5f,
            Mathf.PerlinNoise(t, noiseSeed) - 0.5f,
            0f) * (2f * buzzAmplitude * Mathf.Sin(progress * Mathf.PI));

        Vector3 newPos = pathPos + buzz;
        Vector3 velocity = newPos - transform.position;
        transform.position = newPos;

        // Turn toward the actual movement direction (including buzz), smoothed so it doesn't jitter
        if (velocity.sqrMagnitude > 0.000001f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, FacingRotation(velocity), turnSpeed * Time.deltaTime);
            UpdateFlip();
        }
    }

    // Flip the sprite when heading left so it isn't upside down.
    // The dead zone stops it flickering while flying nearly straight up or down.
    private void UpdateFlip()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        float heading = Mathf.Cos((transform.eulerAngles.z - spriteAngleOffset) * Mathf.Deg2Rad);
        if (heading < -0.2f) spriteRenderer.flipY = true;
        else if (heading > 0.2f) spriteRenderer.flipY = false;
    }

    private Quaternion FacingRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + spriteAngleOffset;
        return Quaternion.Euler(0f, 0f, angle);
    }

    private static Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        float u = 1f - t;
        return u * u * a + 2f * u * t * b + t * t * c;
    }
}
