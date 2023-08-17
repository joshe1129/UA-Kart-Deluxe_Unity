using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatnipBehaviour : ItemBehaviour
{
    //[SerializeField] float speedBoost;
    //[SerializeField] float timeBoost;
    //public bool boostActive;

    private void Awake()
    {
        timesReusable = transform.childCount;
    }

    private void Start()
    {
        //StartCoroutine(StartDestroyCount());
    }

    private void Update()
    {
        Debug.Log(timesReusable);
    }

    public override void ActivateItemEffect()
    {
        if (kart.GetComponent<KartMovement>().boost) return;

        base.ActivateItemEffect();

        //Destroy(this.transform.GetChild(timesUsed).gameObject);
        Destroy(transform.GetChild(Random.Range(0, timesReusable)).gameObject);
        kart.GetComponent<KartMovement>().boost = true;
        if (timesReusable <= 0) Destroy(gameObject);

        //TODO: call of the kart movement function which active the boost mode.
        //kart.Boost();

    }

    private IEnumerator StartDestroyCount()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }
}
