using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FourofAcesBehaviour : ItemBehaviour
{
    public override void ActivateItemEffect()
    {
        Vector3 spawnPoint = kart.transform.position + kart.transform.forward;
        transform.position = spawnPoint;
        switch (kart.GetComponentInChildren<CarStats>().hitPoints)
        {
            case 0:
                for (int i = 0; i < this.transform.childCount - 3; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                kart.GetComponentInChildren<CarStats>().hitPoints += 3;
                break;
             case 1:
                for (int i = 0; i < this.transform.childCount - 2; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                kart.GetComponentInChildren<CarStats>().hitPoints += 2;
                break;
             case 2:
                for (int i = 0; i < transform.childCount - 1; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                kart.GetComponentInChildren<CarStats>().hitPoints += 1;
                break;
             case 3:
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                break;
             default:
                break;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<KartMovement>())
        {
            if (transform.parent.childCount == 1)
            {
                Destroy(transform.parent.gameObject);
                return;
            }
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.GetComponent<ItemBehaviour>())
        {
            Destroy(other);
            if (transform.parent.childCount == 1)
            {
                Destroy(transform.parent.gameObject);
                return;
            }
            Destroy(gameObject);
            return;
        }
    }

}
