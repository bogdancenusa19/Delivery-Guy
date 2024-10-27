using UnityEngine;

public abstract class CarBehavior : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float boostSpeed = 20f;
    public float laneOffset = -3f;
    public float detectionRange = 10f;
    public float laneChangeDuration = 1.0f;
    public float brakeStrength = 5f;
    public float minDistanceToBrake = 5f;
    public float stopDistance = 2f;

    protected Vector3 originalLanePosition;
    protected Vector3 targetLanePosition;
    protected float currentSpeed;

    protected virtual void Start()
    {
        originalLanePosition = transform.position;
        targetLanePosition = originalLanePosition;
        currentSpeed = forwardSpeed;
    }

    protected virtual void Update()
    {
        MoveForward();
        SmoothLaneChange();
    }

    protected void MoveForward()
    {
        // Mișcare constantă pe direcția înainte (Z)
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    protected void SmoothLaneChange()
    {
        // Tranziție lină doar pe direcția X, fără a afecta viteza pe Z
        float step = currentSpeed * Time.deltaTime / laneChangeDuration;
        Vector3 newPosition = new Vector3(targetLanePosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    }

    protected bool IsObstacleInFront(out float distance)
    {
        RaycastHit hit;
        Vector3 raycastStart = transform.position;

        if (Physics.Raycast(raycastStart, transform.forward, out hit, detectionRange))
        {
            if (hit.collider.CompareTag("Vehicle"))
            {
                distance = hit.distance;
                return true;
            }
        }

        distance = 0f;
        return false;
    }

    protected bool IsLaneClear()
    {
        RaycastHit hit;
        Vector3 raycastStart = new Vector3(originalLanePosition.x, transform.position.y, transform.position.z);
        if (Physics.Raycast(raycastStart, transform.forward, out hit, detectionRange))
        {
            return !hit.collider.CompareTag("Vehicle");
        }
        return true;
    }

    protected void Brake()
    {
        currentSpeed = Mathf.Max(0, currentSpeed - brakeStrength * Time.deltaTime);
    }

    protected void AccelerateToDefault()
    {
        currentSpeed = Mathf.Min(forwardSpeed, currentSpeed + brakeStrength * Time.deltaTime);
    }

    protected void SetTargetLanePosition(float offset)
    {
        targetLanePosition = new Vector3(originalLanePosition.x + offset, transform.position.y, transform.position.z);
    }
}
