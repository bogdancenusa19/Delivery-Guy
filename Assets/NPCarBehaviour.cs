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
                Debug.Log($"{gameObject.name} is braking completely because a vehicle is very close.");
                Brake(); // Frânează complet dacă obstacolul este foarte aproape
            }
            else if (distance < minDistanceToBrake && currentSpeed > forwardSpeed && IsLaneClear())
            {
                // Dacă NPC-ul are o viteză mai mare decât vehiculul din față și banda opusă este liberă, încearcă să depășească
                Debug.Log($"{gameObject.name} is changing lane to avoid the obstacle.");
                SetTargetLanePosition(laneOffset); // Încearcă să schimbe banda pentru a depăși obstacolul
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