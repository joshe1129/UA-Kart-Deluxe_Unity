using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PeacockFeatherBehaviour;

public class SugarBehaviour : ItemBehaviour
{
    [SerializeField]
    private float boostTime;

    private KartMovement kartMovement;

    public override void ActivateItemEffect()
    {
        kartMovement = kart.GetComponent<KartMovement>();
        kartMovement.boost = true;
        kartMovement.boostingDelegate += RemoveSugarFromItemInUse;
    }
    public void RemoveSugarFromItemInUse()
    {
        kartMovement.boostingDelegate -= RemoveSugarFromItemInUse;
        kartMovement.GetComponent<PlayerController>().itemInUse = null;
    }
}
