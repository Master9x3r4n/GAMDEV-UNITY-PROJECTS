using UnityEngine;

[RequireComponent(typeof(Light))]
public class SpotlightReveal : MonoBehaviour
{
    [SerializeField] private LayerMask hiddenMask;
    [SerializeField] private LayerMask obstacleMask = ~0;
    [SerializeField, Range(0.1f, 1.5f)] private float angleScale = 1f;

    private Light spot;
    private bool isActive = true;
    private readonly Collider[] buffer = new Collider[64];

    void Awake()
    {
        spot = GetComponent<Light>();
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
        if (!isActive || !spot.enabled) return;

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;
        float halfAngle = spot.spotAngle * 0.5f * angleScale;
        
        int count = Physics.OverlapSphereNonAlloc(origin, spot.range, buffer, hiddenMask,
                                                  QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            Collider c = buffer[i];
            SpeedBoost boost = c.GetComponentInParent<SpeedBoost>();
            if (boost == null) continue;
            
            Vector3 targetPoint = c.ClosestPoint(origin);
            Vector3 toTarget = targetPoint - origin;

            if (Vector3.Angle(forward, toTarget) > halfAngle) continue;
            
            if (Physics.Linecast(origin, targetPoint, obstacleMask, QueryTriggerInteraction.Ignore))
                continue;

            boost.Reveal();
        }
    }

    private void OnGameOver()
    {
        isActive = false;
    }

    private void OnGameStart()
    {
        isActive = true;
    }


    private void OnDrawGizmosSelected()
    {
        Light l = spot != null ? spot : GetComponent<Light>();
        if (l == null) return;

        float halfAngle = l.spotAngle * 0.5f * angleScale;
        Gizmos.color = Color.yellow;

        Vector3 o = transform.position;
        foreach (float roll in new[] { 0f, 90f, 180f, 270f })
        {
            Vector3 dir = Quaternion.AngleAxis(roll, transform.forward) *
                          (Quaternion.AngleAxis(halfAngle, transform.up) * transform.forward);
            Gizmos.DrawRay(o, dir * l.range);
        }
    }
}