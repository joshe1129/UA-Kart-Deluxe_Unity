using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotBehaviour : ItemBehaviour
{
    [SerializeField] float carrotSpeed;
    [SerializeField] float stunTime;
    //Warning! lifeTime value must be high than timeStunned
    [SerializeField] float lifeTime;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(transform.forward.x * carrotSpeed, rb.velocity.y, transform.forward.z * carrotSpeed);
    }

    public override void ActivateItemEffect()
    {
        transform.parent = null;

        transform.rotation = kart.transform.rotation;
        transform.forward = kart.transform.forward;
        transform.position = kart.transform.position + kart.transform.forward * 3;

        kart.GetComponent<PlayerController>().itemInUse = null;

        StartCoroutine(DestroyonLifeTime());
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);

        //TODO: Change the system of detecting collision.
        //Maybe use Layer "BeStunned" or something similar.
        //TODO: form to know if the possible collider is a kart (should be stunned or not)
        if (other.gameObject.GetComponent<KartMovement>())
        {
            other.transform.GetComponent<KartMovement>().StartCoroutine(other.transform.GetComponent<KartMovement>().Stun(stunTime));
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyonLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
