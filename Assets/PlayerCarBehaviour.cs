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

        Debug.Log("Stamina is: " + stamina);
        // Verifică obstacolele în față și frânează prioritar
        float obstacleDistance;
        if (IsObstacleInFront(out obstacleDistance) && obstacleDistance < minDistanceToBrake)
        {
            //Debug.Log($"{gameObject.name} is braking due to close obstacle at distance: {obstacleDistance}");
            Brake();
        }
        else
        {
            // Dacă nu există obstacole, accelerează treptat până la viteza inițială
            if (currentSpeed < forwardSpeed)
            {
                AccelerateToDefault();
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
        }

        HandleCooldownAndRecharge();
    }



    private void StartBoost()
    {
        isBoosting = true;
        boostUsed = true;
        stamina -= Time.deltaTime;
        currentSpeed = boostSpeed;

        // Setează ținta pe contrasens
        SetTargetLanePosition(laneOffset);
    }

    private void StopBoost()
    {
        isBoosting = false;
        boostUsed = false;
        currentSpeed = forwardSpeed;

        // Setează ținta pentru revenirea pe banda inițială
        SetTargetLanePosition(0f);

        // Frânează dacă banda este ocupată și setează cooldown-ul activ
        if (!IsLaneClear())
        {
            Brake();
        }

        // Activează cooldown-ul, dar reîncărcarea efectivă va începe doar după revenirea pe bandă
        cooldownActive = true;
        boostCooldownTimer = cooldownTime;
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