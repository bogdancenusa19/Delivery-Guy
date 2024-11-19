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
    public float currentSpeed;
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
        Vector3 raycastStartLeft = transform.position + Vector3.left * 2f;
        Vector3 raycastStartRight = transform.position + Vector3.right * 2f;

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
            Debug.Log($"{gameObject.name} found no obstacles in front.");
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
        RaycastHit hitFront;
        RaycastHit hitBack;
    
        // Pozițiile de start pentru Raycast-uri
        Vector3 raycastStartFront = transform.position + transform.forward * 5.5f + Vector3.right * .5f;
        Vector3 raycastStartBack = transform.position - transform.forward * 5f + Vector3.right * .5f;

        // Lansează Raycast-ul din față
        bool frontHit = Physics.Raycast(raycastStartFront, Vector3.right, out hitFront, detectionDistance) && hitFront.collider.CompareTag("Vehicle");

        // Lansează Raycast-ul din spate
        bool backHit = Physics.Raycast(raycastStartBack, Vector3.right, out hitBack, detectionDistance) && hitBack.collider.CompareTag("Vehicle");

        // Verifică dacă oricare dintre Raycast-uri a detectat un vehicul
        if (frontHit || backHit)
        {
            Debug.Log($"{gameObject.name} detected a vehicle on the right at distance {detectionDistance}");
            return true;
        }

        return false;
    }

    protected virtual void OnDrawGizmos()
    {

        // Definim punctele de pornire pentru cele trei raze: centru, stânga și dreapta
        Vector3 raycastStartCenter = transform.position + transform.forward * 3f;
        Vector3 raycastStartLeft = transform.position + Vector3.left * 2f + transform.forward * 3f;
        Vector3 raycastStartRight = transform.position + Vector3.right * 2f + transform.forward * 3f;

        // Setăm culori pentru fiecare rază
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastStartCenter, raycastStartCenter + transform.forward * detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(raycastStartLeft, raycastStartLeft + transform.forward * detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(raycastStartRight, raycastStartRight + transform.forward * detectionRange);
        
        float detectionDistance = 5f;  // Ajustează distanța dacă e nevoie
        Vector3 raycastStartFront = transform.position + transform.forward * 5.5f + Vector3.right * .5f;
        Vector3 raycastStartBack = transform.position - transform.forward * 5f + Vector3.right * .5f;

        // Setăm culoarea pentru Raycast-urile laterale dreapta
        Gizmos.color = Color.yellow;

        // Desenăm linia pentru Raycast-ul din față (dreapta)
        Gizmos.DrawLine(raycastStartFront, raycastStartFront + Vector3.right * detectionDistance);

        // Desenăm linia pentru Raycast-ul din spate (dreapta)
        Gizmos.DrawLine(raycastStartBack, raycastStartBack + Vector3.right * detectionDistance);

    }

    
}
