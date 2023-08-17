using System.Collections;
using UnityEngine;

public class PolarTouchRocketBehaviour : MonoBehaviour
{
    [SerializeField]
    [Range(20f, 40f)]
    [Tooltip("Final speed is the target Kart rigidBody velocity plus this value")]
    private float movementSpeed = 5;
    [SerializeField]
    private float flyUpTime;

    private GameObject targetKart;
    private Rigidbody targetRigidbody;
    private float freezeTime;
    private float currentFlyUpTime;

    private void Update()
    {
        if (targetKart == null)
        {
            return;
        }

        Vector3 newPosition;
        float currentKartSpeed = targetRigidbody.velocity.magnitude;
        if (currentFlyUpTime > Time.time)
        {
            newPosition = newPosition = Vector3.MoveTowards(
                transform.position,
                transform.position + Vector3.up,
                (currentKartSpeed + movementSpeed) * Time.deltaTime);
        }
        else
        {
            newPosition = Vector3.MoveTowards(
                transform.position,
                targetKart.transform.position,
                (currentKartSpeed + movementSpeed) * Time.deltaTime);
        }

        Vector3 newDirection = newPosition - transform.position;

        transform.position = newPosition;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void SetValues(GameObject newTargetKart, float newfreezeTime)
    {
        currentFlyUpTime = Time.time + flyUpTime;
        targetKart = newTargetKart;
        targetRigidbody = targetKart.GetComponent<Rigidbody>();
        freezeTime = newfreezeTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != targetKart)
        {
            return;
        }

        KartMovement kartMovement = targetKart.GetComponent<KartMovement>();
        IEnumerator coroutine = kartMovement.SlowDown(freezeTime, float.MaxValue);
        kartMovement.StopCoroutine(coroutine);
        kartMovement.StartCoroutine(coroutine);
        Destroy(gameObject);
    }
}
