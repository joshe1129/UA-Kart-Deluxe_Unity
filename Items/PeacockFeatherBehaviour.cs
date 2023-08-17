using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeacockFeatherBehaviour : ItemBehaviour
{
    [SerializeField]
    private float invicibilityTime;
    [SerializeField]
    private float speedIncrement;

    private KartMovement kartMovement;

    public delegate void PeacockFeatherDelegate();
    PeacockFeatherDelegate peacockFeatherDelegate;

    public override void ActivateItemEffect()
    {
        //if (kartMovement.invincible) return;//refresh the time maybe instead of returning  
        kartMovement = kart.GetComponent<KartMovement>();
        peacockFeatherDelegate += RemovePeacockFeatherFromItemInUse;
        kartMovement.StartCoroutine(kartMovement.Invincibility(invicibilityTime, speedIncrement, peacockFeatherDelegate));
    }
    public void RemovePeacockFeatherFromItemInUse()
    {
        peacockFeatherDelegate -= RemovePeacockFeatherFromItemInUse;
        kartMovement.GetComponent<PlayerController>().itemInUse = null;
    }
}
