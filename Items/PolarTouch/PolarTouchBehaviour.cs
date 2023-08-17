using UnityEngine;

public class PolarTouchBehaviour : ItemBehaviour
{
    [SerializeField]
    private GameObject rocketPrefab;

    [SerializeField]
    private float freezeTime;

    public override void ActivateItemEffect()
    {
        base.ActivateItemEffect();

        GameObject go = Instantiate(rocketPrefab);
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        PolarTouchRocketBehaviour polarTouchRocketBehaviour = go.GetComponent<PolarTouchRocketBehaviour>();



        polarTouchRocketBehaviour.SetValues(GameController.Instance.GetKartAtPosition(1), freezeTime);
    }
}

