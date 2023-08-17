using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JokerCardBehaviour : ItemBehaviour
{
    public override void ActivateItemEffect()
    {
        Vector3 spawnPoint = kart.transform.position + kart.transform.forward;
        transform.position = spawnPoint;

        if (kart.GetComponentInChildren<CarStats>().hitPoints < 3)
        {
            Debug.Log("trigger " + kart.GetComponentInChildren<CarStats>().hitPoints);
            kart.GetComponentInChildren<CarStats>().hitPoints++;
            Destroy(gameObject);
        }
        //StartCoroutine(ExplotionScaleOverTime(lifeTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<KartMovement>())
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.GetComponent<ItemBehaviour>())
        {
            Destroy(other);
            Destroy(gameObject);
            return;
        }
    }
}
