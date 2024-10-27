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

        // Controlează boost-ul prin input
        if (Input.GetMouseButton(0) && stamina > 0 && !cooldownActive)
        {
            StartBoost();
        }
        else if (boostUsed)
        {
            StopBoost();
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

        if (stamina <= 0f)
        {
            boostCooldownTimer = cooldownTime;
            cooldownActive = true;
        }

        // Setează ținta direct pe banda inițială
        SetTargetLanePosition(0f);

        // Frânează și prioritizează revenirea pe banda inițială dacă este ocupată
        if (!IsLaneClear())
        {
            Brake();
        }
    }

    private void HandleCooldownAndRecharge()
    {
        if (cooldownActive)
        {
            boostCooldownTimer -= Time.deltaTime;
            if (boostCooldownTimer <= 0f)
            {
                cooldownActive = false;
                stamina = boostDuration;
            }
        }
    }
}