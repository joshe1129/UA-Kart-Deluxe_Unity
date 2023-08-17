using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesBehaviour : ItemBehaviour
{
    [Header("Generic values")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float timeOfStun;
    [SerializeField] float rotationWhenKartRotating;

    private void Update()
    {
        if (transform.parent == null)
        {
            transform.position = kart.transform.position + Vector3.up;

            float forwardValue = 0;
            KartMovement kartMovement = kart.GetComponent<KartMovement>();

            if (kartMovement.forwardScalarVelocity < 0)
                forwardValue = (-1 * kartMovement.forwardScalarVelocity) / kartMovement.forwardScalarVelocity;
            else if (kartMovement.forwardScalarVelocity > 0)
                forwardValue = kartMovement.forwardScalarVelocity / kartMovement.forwardScalarVelocity;
            else if (kartMovement.forwardScalarVelocity <= 0.01f || kartMovement.forwardScalarVelocity >= 0.01f)
                forwardValue = 0;

            transform.Rotate(new Vector3(transform.rotation.x, ((forwardValue * kartMovement.angularScalarRotation * (rotationSpeed / 4)) + rotationSpeed) * Time.deltaTime, transform.rotation.z));
        }
    }

    public override void ActivateItemEffect()
    {
        transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<RaceCreatorController>() || !other.GetComponent<EnvironmentFrictionController>())
        {
            KartMovement kart = other.GetComponent<KartMovement>();
            if (kart)
            {
                kart.StopAllCoroutines();
                kart.StartCoroutine(kart.Stun(timeOfStun));
            }
            if (transform.parent.childCount == 1)
            {
                Destroy(transform.parent.gameObject);
                return;
            }
            Destroy(gameObject);
        }
    }
}
