using System.Collections;
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
    private bool isBraking = false;
    private float lastSpeed = 0f;

    protected Vector3 originalLanePosition;
    protected Vector3 targetLanePosition;
    public float currentSpeed;
    protected bool isOvertakingDone = false;

    [Header("Lights")] 
    public GameObject car;
    public Material lightsOff;

    public Material lightsOn;
    
    

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
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    protected void SmoothLaneChange()
    {
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
        
        Vector3 raycastStartCenter = transform.position;
        Vector3 raycastStartLeft = transform.position + Vector3.left * 2f;
        Vector3 raycastStartRight = transform.position + Vector3.right * 2f;
        
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
        if (currentSpeed > lastSpeed + 2f)
        {
            isBraking = false; 
        }
        
        if (!isBraking)
        {
            StartCoroutine(TurnLightsOn());
            isBraking = true;
        }
        
        currentSpeed = Mathf.Max(0, currentSpeed - brakeStrength * Time.deltaTime);
        
        lastSpeed = currentSpeed;
    }
    
    private IEnumerator TurnLightsOn()
    {
        float timePassed = 0f;
        Material currentMaterial = car.GetComponent<Renderer>().material;
        
        while (timePassed < 1.5f)
        {
           timePassed += Time.deltaTime;
           car.GetComponent<Renderer>().material = lightsOn;
           
           yield return null;
        }
        
        car.GetComponent<Renderer>().material = lightsOff;
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
        
        Vector3 raycastStartFront = transform.position + transform.forward * 5.5f + Vector3.right * .5f;
        Vector3 raycastStartBack = transform.position - transform.forward * 5f + Vector3.right * .5f;
        
        bool frontHit = Physics.Raycast(raycastStartFront, Vector3.right, out hitFront, detectionDistance) && hitFront.collider.CompareTag("Vehicle");
        
        bool backHit = Physics.Raycast(raycastStartBack, Vector3.right, out hitBack, detectionDistance) && hitBack.collider.CompareTag("Vehicle");
        
        if (frontHit || backHit)
        {
            Debug.Log($"{gameObject.name} detected a vehicle on the right at distance {detectionDistance}");
            return true;
        }

        return false;
    }

    protected virtual void OnDrawGizmos()
    {
        
        Vector3 raycastStartCenter = transform.position + transform.forward * 3f;
        Vector3 raycastStartLeft = transform.position + Vector3.left * 2f + transform.forward * 3f;
        Vector3 raycastStartRight = transform.position + Vector3.right * 2f + transform.forward * 3f;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastStartCenter, raycastStartCenter + transform.forward * detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(raycastStartLeft, raycastStartLeft + transform.forward * detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(raycastStartRight, raycastStartRight + transform.forward * detectionRange);
        
        float detectionDistance = 5f;
        Vector3 raycastStartFront = transform.position + transform.forward * 5.5f + Vector3.right * .5f;
        Vector3 raycastStartBack = transform.position - transform.forward * 5f + Vector3.right * .5f;
        
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawLine(raycastStartFront, raycastStartFront + Vector3.right * detectionDistance);
        
        Gizmos.DrawLine(raycastStartBack, raycastStartBack + Vector3.right * detectionDistance);

    }

    
}
