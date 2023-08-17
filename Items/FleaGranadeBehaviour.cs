using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FleaGranadeBehaviour : ItemBehaviour
{
    [SerializeField]
    private float stunTime;
    [SerializeField] float lifeTime;

    public override void ActivateItemEffect()
    {
        Vector3 spawnPoint = kart.transform.position + kart.transform.forward;
        transform.position = spawnPoint;
        StartCoroutine(ExplotionScaleOverTime(lifeTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            if (other.gameObject.GetComponent<KartMovement>())
            {
                other.transform.GetComponent<KartMovement>().StartCoroutine(other.transform.GetComponent<KartMovement>().SteerStun(stunTime));
                Debug.Log("trigger" + other.name);
            }
        }
    }

    IEnumerator ExplotionScaleOverTime(float lifeTime)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = new Vector3(13.0f, 13.0f, 13.0f);
        float currentTime = 0.0f;
        do
        {
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / lifeTime);
            currentTime += Time.deltaTime;
            yield return null;
        }while(currentTime <= lifeTime);
        Destroy(gameObject);
    }
}
