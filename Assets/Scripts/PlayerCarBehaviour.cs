using System.Collections;
using UnityEngine;

public class PlayerCarBehavior : CarBehavior
{
    [Header("Player Specs")] 
    public float boostDuration = 3f;
    [SerializeField] private float cooldownTime = 5f;
    public float stamina = 3f;
    private bool isBoosting = false;
    private bool boostUsed = false;
    private bool cooldownActive = false;
    private float boostCooldownTimer = 0f;

    private bool reachedDestination = false;

    protected override void Start()
    {
        base.Start();
        stamina = boostDuration;
    }

    protected override void Update()
    {
        base.Update();

        if (reachedDestination) return;
        
        float obstacleDistance;
        if (IsObstacleInFront(out obstacleDistance) && obstacleDistance < minDistanceToBrake)
        {
            Debug.Log($"{gameObject.name} is braking due to close obstacle at distance: {obstacleDistance}");
            Brake();
        }
        else
        {
            if (currentSpeed < forwardSpeed)
            {
                AccelerateToDefault();
            }
        }
        
        if (Input.GetMouseButton(0) && stamina > 0)
        {
            StartBoost();
            cooldownActive = false;
        }
        else if (boostUsed)
        {
            StopBoost();
            cooldownActive = true;
        }

        if(isOvertakingDone)
            ReturnToLane();
        
        HandleCooldownAndRecharge();
        
    }

    private void StartBoost()
    {
        isBoosting = true;
        boostUsed = true;
        stamina -= Time.deltaTime;
        currentSpeed = boostSpeed;

        float distance;
        if (IsObstacleInFront(out distance))
        {
            isOvertakingDone = false;
            SetTargetLanePosition(laneOffset);
        }
    }

    private void StopBoost()
    {
        isOvertakingDone = true;
        isBoosting = false;
        boostUsed = false;
        currentSpeed = forwardSpeed;
        
        boostCooldownTimer = cooldownTime;
    }

    private void ReturnToLane()
    {
            SetTargetLanePosition(0f);
    }

    private void HandleCooldownAndRecharge()
    {
        if (cooldownActive)
        {
            stamina = Mathf.Min(stamina + Time.deltaTime, boostDuration);
            
            if (stamina == boostDuration)
            {
                cooldownActive = false;
            }
        }
    }

    public void StopAtDestination()
    {
        reachedDestination = true;
        
        SetTargetLanePosition(-laneOffset);
        
        StartCoroutine(SlowToStop());
    }

    private IEnumerator SlowToStop()
    {
        while (currentSpeed > 0)
        {
            currentSpeed -= .1f * brakeStrength  * Time.deltaTime;
            yield return null;
        }

        currentSpeed = 0;
        Debug.Log("Player has reached the destination and stopped!");
    }
}