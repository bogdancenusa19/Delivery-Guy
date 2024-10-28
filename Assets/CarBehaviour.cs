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
    protected bool isOvertakingDone = false;

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
        RaycastHit hitCenter;
        RaycastHit hitLeft;
        RaycastHit hitRight;

        distance = detectionRange;
        bool obstacleDetected = false;

        // Definim punctele de pornire pentru cele trei raze: centru, stânga și dreapta
        Vector3 raycastStartCenter = transform.position;
        Vector3 raycastStartLeft = transform.position + Vector3.left * 0.5f;
        Vector3 raycastStartRight = transform.position + Vector3.right * 0.5f;

        // Verificăm pentru fiecare rază dacă există obstacole
        if (Physics.Raycast(raycastStartCenter, transform.forward, out hitCenter, detectionRange))
        {
            //Debug.Log($"{gameObject.name} center ray hit {hitCenter.collider.tag} at distance {hitCenter.distance}");
            if (hitCenter.collider.CompareTag("Vehicle") || hitCenter.collider.CompareTag("Player"))
            {
                distance = hitCenter.distance;
                obstacleDetected = true;
            }
        }

        if (Physics.Raycast(raycastStartLeft, transform.forward, out hitLeft, detectionRange))
        {
            //Debug.Log($"{gameObject.name} left ray hit {hitLeft.collider.tag} at distance {hitLeft.distance}");
            if (hitLeft.collider.CompareTag("Vehicle") || hitLeft.collider.CompareTag("Player"))
            {
                distance = Mathf.Min(distance, hitLeft.distance);
                obstacleDetected = true;
            }
        }

        if (Physics.Raycast(raycastStartRight, transform.forward, out hitRight, detectionRange))
        {
            //Debug.Log($"{gameObject.name} right ray hit {hitRight.collider.tag} at distance {hitRight.distance}");
            if (hitRight.collider.CompareTag("Vehicle") || hitRight.collider.CompareTag("Player"))
            {
                distance = Mathf.Min(distance, hitRight.distance);
                obstacleDetected = true;
            }
        }

        if (!obstacleDetected)
        {
            //Debug.Log($"{gameObject.name} found no obstacles in front.");
        }

        return obstacleDetected;
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
        currentSpeed = Mathf.Min(currentSpeed + brakeStrength * Time.deltaTime, forwardSpeed);
    }


    protected void SetTargetLanePosition(float offset)
    {
        targetLanePosition = new Vector3(originalLanePosition.x + offset, transform.position.y, transform.position.z);
    }
    
    protected bool IsVehicleOnRight(float detectionDistance)
    {
        RaycastHit hit;
        Vector3 raycastStart = transform.position;
        Vector3 rayDirection = Vector3.right;

        // Lansează un Raycast către dreapta pentru a verifica dacă există un vehicul
        if (Physics.Raycast(raycastStart, rayDirection, out hit, detectionDistance))
        {
            if (hit.collider.CompareTag("Vehicle"))
            {
                Debug.Log($"{gameObject.name} detected a vehicle on the right at distance {hit.distance}");
                return true;
            }
        }
        return false;
    }
    
}
