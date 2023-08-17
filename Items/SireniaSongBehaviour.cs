using System.Collections.Generic;
using UnityEngine;

public class SireniaSongBehaviour : ItemBehaviour
{
    [SerializeField]
    private bool debug;
    [SerializeField]
    private float slowTime;
    [SerializeField]
    private float slowAmount;

    private bool isActive;

    private Dictionary<int, KartMovement> kartMovementDict;

    private void Awake()
    {
        kartMovementDict = new Dictionary<int, KartMovement>();
        transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            foreach (KeyValuePair<int, KartMovement> kartMovementPair in kartMovementDict)
            {
                KartMovement kartMovement = kartMovementPair.Value;
                
                // Skip the Kart owner of this Item
                if (kartMovement.gameObject == transform.parent.gameObject)
                {
                    continue;
                }

                if (debug)
                {
                    Debug.Log($"[SireniaSongBehaviour] Applying slow from {kartMovement.gameObject.name}", kartMovement.gameObject);
                }
                kartMovement.SlowDown(slowTime, slowAmount);
            }
        }
    }

    public override void ActivateItemEffect()
    {
        base.ActivateItemEffect();

        isActive = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        KartMovement kartMovement = other.GetComponent<KartMovement>();
        if (kartMovement != null)
        {
            kartMovementDict[kartMovement.GetInstanceID()] = kartMovement;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        KartMovement kartMovement = other.GetComponent<KartMovement>();
        if (kartMovement != null && kartMovementDict.ContainsKey(kartMovement.GetInstanceID()))
        {
            kartMovementDict[kartMovement.GetInstanceID()] = kartMovement;
        }
    }
}

