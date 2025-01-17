using UnityEngine;

public class NPCarBehavior : CarBehavior
{
    private float behaviorChangeTimer = 0f;
    public float behaviorChangeInterval = 5f;
    public float minSpeed;
    public float maxSpeed;
    private bool isBrakingTemporarily = false;

    [SerializeField] private GameObject frontCarDetector;

    protected override void Start()
    {
        base.Start();
        SetRandomSpeed();
    }

    protected override void Update()
    {
        base.Update();
        HandleTraffic();
        
        behaviorChangeTimer -= Time.deltaTime;
        if (behaviorChangeTimer <= 0f)
        {
            SetRandomSpeed();
            behaviorChangeTimer = behaviorChangeInterval;
        }
    }

    private void SetRandomSpeed()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    private void HandleTraffic()
    {
        float distance;
        if (IsObstacleInFront(out distance) || IsPlayerOnLeft())
        {
            if (distance < stopDistance || IsPlayerOnLeft())
            {
                Debug.Log($"{gameObject.name} is braking immediately due to a close obstacle or player on the left.");
                currentSpeed = 0;
            }
            else if (distance < minDistanceToBrake)
            {
                Debug.Log($"{gameObject.name} is braking progressively.");
                Brake();
            }
        }
        else
        {
            AccelerateToDefault();
        }
    }

    private bool IsPlayerOnLeft()
    {
        RaycastHit hit;
        Vector3 raycastStart = frontCarDetector.transform.position + Vector3.left * 1.5f; // Verifică în lateral stânga

        // Lansează un Raycast către stânga pentru a verifica dacă există un vehicul (playerul) în apropiere
        if (Physics.Raycast(raycastStart, Vector3.left, out hit, 5f)) // Folosim o distanță de 5 unități pentru detecție laterală
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log($"{gameObject.name} detected player on the left at distance {hit.distance}");
                return true;
            }
        }
        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        float detectionDistance = 5f; 
        Vector3 raycastStart = frontCarDetector.transform.position + Vector3.left * 1.5f; // Verifică în lateral stânga
        Gizmos.color = Color.black;

        // Desenăm linia pentru Raycast-ul din față (dreapta)
        Gizmos.DrawLine(raycastStart, raycastStart + Vector3.left * detectionDistance);
        
    }
}
