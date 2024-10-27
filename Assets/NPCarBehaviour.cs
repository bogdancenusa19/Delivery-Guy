using UnityEngine;

public class NPCarBehavior : CarBehavior
{
    private float behaviorChangeTimer = 0f;
    public float behaviorChangeInterval = 5f;
    public float minSpeed;
    public float maxSpeed;

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
        if (IsObstacleInFront(out distance))
        {
            if (distance < stopDistance)
            {
                Debug.Log($"{gameObject.name} is braking immediately due to a very close obstacle.");
                currentSpeed = 0; // Oprire completă dacă obstacolul este extrem de aproape
            }
            else if (distance < minDistanceToBrake && currentSpeed > forwardSpeed && IsLaneClear())
            {
                Debug.Log($"{gameObject.name} is changing lane to avoid the obstacle.");
                SetTargetLanePosition(laneOffset); // Schimbă banda pentru a depăși
            }
            else if (distance < minDistanceToBrake)
            {
                Debug.Log($"{gameObject.name} is braking progressively.");
                Brake(); // Frânare treptată pentru a menține o distanță de siguranță
            }
        }
        else
        {
            // Dacă nu există obstacole, revine la banda inițială
            if (targetLanePosition.x != originalLanePosition.x)
            {
                SetTargetLanePosition(0f); // Revine pe banda inițială
            }

            // Revine la viteza normală dacă drumul e liber
            AccelerateToDefault();
        }
    }

}