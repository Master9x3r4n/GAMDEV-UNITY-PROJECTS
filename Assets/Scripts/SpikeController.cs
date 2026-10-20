using UnityEngine;

public class SpikeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 localDirection = Vector3.forward;
    [SerializeField] private float distance = 2f;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float pauseTime = 0.5f;

    [Header("Collision")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector3 rayOriginOffset = Vector3.zero;
    [SerializeField] private float rayRadius = 0.25f;
    [SerializeField] private float hitCooldown = 1f;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 target;
    private float pauseTimer;
    private float nextHitTime;
    private bool isActive = true;
    private readonly Collider[] overlapBuffer = new Collider[4];

    void Start()
    {
        startPos = transform.position;
        Vector3 worldDirection = transform.TransformDirection(localDirection).normalized;
        endPos = startPos + worldDirection * distance;
        target = endPos;
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameStart -= OnGameStart;
    }

    void Update()
    {
        if (!isActive) return;

        // waiting at one end
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            CheckHit(Vector3.zero, 0f);
            return;
        }

        Vector3 current = transform.position;
        Vector3 next = Vector3.MoveTowards(current, target, speed * Time.deltaTime);
        Vector3 step = next - current;

        // check the path BEFORE moving, so a fast spike can't skip over the player
        CheckHit(step.normalized, step.magnitude);

        transform.position = next;

        if (next == target)
        {
            target = (target == endPos) ? startPos : endPos;
            pauseTimer = pauseTime;
        }
    }

    private void CheckHit(Vector3 direction, float stepLength)
    {
        if (Time.time < nextHitTime) return;

        // when paused there is no movement direction, so keep checking along the travel axis
        if (direction == Vector3.zero)
            direction = (endPos - startPos).normalized;

        Vector3 origin = transform.TransformPoint(rayOriginOffset);
        float length = stepLength + 0.05f;

        bool hit;
        RaycastHit info;

        if (rayRadius > 0f)
            hit = Physics.SphereCast(origin, rayRadius, direction, out info, length,
                                     playerLayer, QueryTriggerInteraction.Ignore);
        else
            hit = Physics.Raycast(origin, direction, out info, length,
                                  playerLayer, QueryTriggerInteraction.Ignore);

        PlayerHandler player = null;

        if (hit)
            player = info.collider.GetComponentInParent<PlayerHandler>();

        // A cast never detects a collider that already contains its starting point,
        // so also check around the origin. This catches the player walking into the spike.
        if (player == null)
        {
            float r = Mathf.Max(rayRadius, 0.1f);
            int count = Physics.OverlapSphereNonAlloc(origin, r, overlapBuffer, playerLayer,
                                                      QueryTriggerInteraction.Ignore);
            for (int i = 0; i < count && player == null; i++)
                player = overlapBuffer[i].GetComponentInParent<PlayerHandler>();
        }

        if (player == null) return;

        player.ReduceHealth();
        nextHitTime = Time.time + hitCooldown;
    }

    private void OnGameOver()
    {
        isActive = false;
    }

    private void OnGameStart()
    {
        isActive = true;
    }

    // shows the travel path and ray origin in the Scene view
    private void OnDrawGizmosSelected()
    {
        Vector3 from = Application.isPlaying ? startPos : transform.position;
        Vector3 to = Application.isPlaying
            ? endPos
            : from + transform.TransformDirection(localDirection).normalized * distance;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(from, to);
        Gizmos.DrawWireSphere(to, 0.1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.TransformPoint(rayOriginOffset), Mathf.Max(rayRadius, 0.05f));
    }
}