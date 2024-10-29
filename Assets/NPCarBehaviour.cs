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

        // Schimbă comportamentul aleatoriu la fiecare interval
        behaviorChangeTimer -= Time.deltaTime;
        if (behaviorChangeTimer <= 0f)
        {
            SetRandomSpeed();
            behaviorChangeTimer = behaviorChangeInterval;
        }
    }

    private void SetRandomSpeed()
    {
        // Alege o viteză aleatorie între minSpeed și maxSpeed
        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    private void HandleTraffic()
    {
        float distance;
        if (IsObstacleInFront(out distance) || IsPlayerOnLeft()) // Verifică obstacolele în față sau playerul în stânga
        {
            if (distance < stopDistance || IsPlayerOnLeft()) // Aplică frânarea și pentru playerul din stânga
            {
                Debug.Log($"{gameObject.name} is braking immediately due to a close obstacle or player on the left.");
                currentSpeed = 0; // Oprire completă dacă obstacolul este extrem de aproape sau dacă playerul este în stânga
            }
            else if (distance < minDistanceToBrake)
            {
                Debug.Log($"{gameObject.name} is braking progressively.");
                Brake(); // Frânare treptată pentru a menține o distanță de siguranță
            }
        }
        else
        {
            // Revine la viteza normală dacă drumul e liber
            AccelerateToDefault();
        }
    }

    private bool IsPlayerOnLeft()
    {
        RaycastHit hit;
        Vector3 raycastStart = frontCarDetector.transform.position + Vector3.left * 0.5f; // Verifică în lateral stânga

        // Lansează un Raycast către stânga pentru a verifica dacă există un vehicul (playerul) în apropiere
        if (Physics.Raycast(raycastStart, Vector3.left, out hit, 3f)) // Folosim o distanță de 2 unități pentru detecție laterală
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log($"{gameObject.name} detected player on the left at distance {hit.distance}");
                return true;
            }
        }
        return false;
    }
}
