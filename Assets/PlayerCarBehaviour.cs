using UnityEngine;

public class PlayerCarBehavior : CarBehavior
{
    public float boostDuration = 3f;
    [SerializeField] private float cooldownTime = 5f;
    private float stamina;
    private bool isBoosting = false;
    private bool boostUsed = false;
    private bool cooldownActive = false;
    private float boostCooldownTimer = 0f;

    protected override void Start()
    {
        base.Start();
        stamina = boostDuration;
    }

    protected override void Update()
    {
        base.Update();
        
        // Verifică obstacolele în față și frânează prioritar
        float obstacleDistance;
        if (IsObstacleInFront(out obstacleDistance) && obstacleDistance < minDistanceToBrake)
        {
            Debug.Log($"{gameObject.name} is braking due to close obstacle at distance: {obstacleDistance}");
            Brake();
        }
        else
        {
            // Dacă nu există obstacole, accelerează treptat până la viteza inițială
            if (currentSpeed < forwardSpeed && !IsVehicleOnRight(8f))
            {
                AccelerateToDefault();
            }
        }
        
        // Controlează boost-ul prin input doar dacă nu există obstacole în față
        if (Input.GetMouseButton(0) && stamina > 0 && !cooldownActive)
        {
            StartBoost();
        }
        else if (boostUsed)
        {
            StopBoost();
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
            // Setează ținta pe contrasens
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

        // Activează cooldown-ul pentru reîncărcare după ce s-a retras pe bandă
        cooldownActive = true;
        boostCooldownTimer = cooldownTime;
    }

    private void ReturnToLane()
    {
        // Dacă există un vehicul în dreapta, aplică frânarea și așteaptă
        if (IsVehicleOnRight(6f)) // Folosim o distanță de 6 unități pentru detecția vehiculelor din dreapta
        {
            Debug.Log($"{gameObject.name} is braking because there's a vehicle on the right, waiting to return to the lane.");
            Brake(); // Aplică frânarea pentru a evita revenirea pe banda inițială
        }
        else
        {
            // Dacă nu există vehicul în dreapta, revine pe banda inițială
            SetTargetLanePosition(0f);
        }
    }

    private void HandleCooldownAndRecharge()
    {
        // Verifică dacă playerul a revenit pe banda inițială înainte de a reîncărca stamina
        if (transform.position.x == originalLanePosition.x && cooldownActive)
        {
            boostCooldownTimer -= Time.deltaTime;

            // Reîncarcă stamina progresiv
            stamina = Mathf.Min(stamina + Time.deltaTime, boostDuration);

            // Dezactivează cooldown-ul dacă stamina este complet reîncărcată
            if (boostCooldownTimer <= 0f || stamina >= boostDuration)
            {
                cooldownActive = false;
                stamina = boostDuration; // Asigură că stamina este complet încărcată
            }
        }
    }


}