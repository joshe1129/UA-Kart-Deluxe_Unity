
using UnityEngine;

public class FenceBehaviour : ItemBehaviour
{
    [SerializeField]
    private float stunTime;
    [SerializeField]
    private float scalarSpawningPosition;

    public override void ActivateItemEffect()
    {
        transform.parent = null;

        Vector3 spawnPoint = kart.transform.position + kart.transform.forward * scalarSpawningPosition;
        transform.position = spawnPoint;

        kart.GetComponent<PlayerController>().itemInUse = null;

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<KartMovement>())
        {
            //if (other.gameObject.GetComponent<KartMovement>().invincible)
            //{
            //    Physics.IgnoreCollision(other.gameObject.GetComponent<KartMovement>().GetComponent<Collider>(), this.GetComponent<Collider>());
            //}
            //else
            //{
            //    other.transform.GetComponent<KartMovement>().StartCoroutine(other.transform.GetComponent<KartMovement>().Stun(stunTime));
            //    Destroy(gameObject);
            //}

            other.transform.GetComponent<KartMovement>().StartCoroutine(other.transform.GetComponent<KartMovement>().Stun(stunTime));
            Destroy(gameObject);
        }
    }
}
