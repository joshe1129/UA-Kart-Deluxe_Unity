using System.Collections;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public PlayerController currentPlayerController;
    public GameObject kart;
    public int timesReusable = 0;

    [SerializeField]
    protected float destroyTime;


    private bool isDestroyCoroutineActive;

    private void Awake()
    {
        currentPlayerController = GetComponent<PlayerController>();
        isDestroyCoroutineActive = false;
    }

    public virtual void ActivateItemEffect()
    {
        if (timesReusable > 0)
        {
            timesReusable--;

            if (timesReusable == 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (!isDestroyCoroutineActive)
            {
                StartCoroutine(DestroyAfterSeconds());
            }
        }
    }

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
