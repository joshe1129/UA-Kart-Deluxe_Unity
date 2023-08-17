using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowPoopBehaviour : ItemBehaviour
{
    [Header("Generic values")]
    [SerializeField] float speed;
    [SerializeField] float timeOfSlow;
    [SerializeField] float slowAmount;
    [SerializeField] float timeUntilDestruction;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(transform.forward.x * speed, rb.velocity.y, transform.forward.z * speed);
    }

    public override void ActivateItemEffect()
    {
        transform.parent = null;

        transform.rotation = kart.transform.rotation;
        transform.forward = kart.transform.forward;
        transform.position = kart.transform.position + kart.transform.forward * 3;

        kart.GetComponent<PlayerController>().itemInUse = null;

        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(timeUntilDestruction);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.GetComponent<RaceCreatorController>() || !collision.transform.GetComponent<EnvironmentFrictionController>())
        {
            KartMovement kart = collision.transform.GetComponent<KartMovement>();
            if (kart)
            {
                IEnumerator coroutine = kart.SlowDown(timeOfSlow, slowAmount);
                kart.StopCoroutine(coroutine);
                kart.StartCoroutine(coroutine);
                Destroy(gameObject);
            }
        }
    }
}
