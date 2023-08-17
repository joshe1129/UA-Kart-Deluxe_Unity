using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Whooly ball: Throws a Whool ball forward that starts growing until it hits a wall.
//It if hits a player the playergets stuck to the ball until it hits a wall.

public class WhoolyBallBehaviour : ItemBehaviour
{
    //[SerializeField]
    //public float growthRate;
    //[SerializeField]
    //public float growthAmplitude;
    [SerializeField]
    private float scalarSpawningPosition;
    [SerializeField]
    private float maxSize;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float growthTime;
    [SerializeField]
    private List<KartMovement> karts; //The karts field is a list of KartMovement objects, which are other karts in the game.
    [SerializeField]
    private GameObject whoolyBall;

    private float currentTime = 0.0f;
    private Rigidbody rb;

    void Update()
    {
        Vector3 scale = whoolyBall.transform.localScale;

        float growth;
        if (scale.x < maxSize)
        {
            //Logarithmic growth
            //Debug.Log("Log");
            //growth = Mathf.Log(Time.time * growthRate) * growthAmplitude;
            //scale += Vector3.one * growth * Time.deltaTime;
            //transform.localScale = scale;

            //Exponential growth
            //growth = Mathf.Pow(growthRate, Time.time) * growthAmplitude;
            //scale += Vector3.one * growth * Time.deltaTime;
            //transform.localScale = scale;
        }

        //Linear size growth
        currentTime += Time.deltaTime;
        growth = Mathf.Lerp(1.0f, maxSize, currentTime / growthTime);
        scale = Vector3.one * growth;
        whoolyBall.transform.localScale = scale;

        ////Move the Rigidbody forwards constantly at speed
        rb.velocity = transform.forward * speed;

        if (karts.Count > 0)
        {
            foreach (KartMovement kart in karts)
            {
                kart.transform.position = this.transform.position;
            }
        }
    }

    private void Awake()
    {
        karts = new List<KartMovement>(); 
        rb = GetComponent<Rigidbody>();
    }
    public override void ActivateItemEffect()
    {
        transform.parent = null;

        transform.rotation = kart.transform.rotation;
        transform.forward = kart.transform.forward;
        transform.position = kart.transform.position + kart.transform.forward * scalarSpawningPosition;

        kart.GetComponent<PlayerController>().itemInUse = null;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if(karts.Count > 0)
            {
                //When the ball hits the wall the karts are "freed"
                foreach (KartMovement kart in karts)
                {
                    kart.blocked = false;
                    kart.transform.parent = null;
                }
            }
            Destroy(gameObject);
        }

        /*
         * The karts field is a list of KartMovement objects, which are other karts in the game. 
         * When the Wooly Ball collides with a kart, the kart is added to the list of karts that are stuck to the ball.
         */
        if (other.gameObject.GetComponent<KartMovement>())
        {
            if (other.gameObject.GetComponent<KartMovement>().invincible)
            {
                Physics.IgnoreCollision(other.gameObject.GetComponent<KartMovement>().GetComponent<Collider>(), this.GetComponent<Collider>());
            }
            else
            {
                KartMovement target = other.gameObject.GetComponent<KartMovement>();
                target.transform.SetParent(this.transform);
                target.blocked = true;
                karts.Add(target);
            }
        }
    }
}
