using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class UICoinController : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject coinBlueprint;
    [SerializeField] private GameObject coinsContainer;
    [SerializeField] private float coinsMovementSpeed;
    [SerializeField] private ParticleSystem coinsParticleSystem;

    private int coinsCount;
    private IObjectPool<GameObject> coinsPool;
    private Dictionary<int, GameObject> activeCoins;
    private List<GameObject> coinsToRelease;

    private void Start()
    {
        coinsCount = 0;
        text.text = "0";

        coinsPool = new ObjectPool<GameObject>(OnCreateCoin, OnTakeFromPool, OnReturnedToPool, null, true, 100);
        activeCoins = new Dictionary<int, GameObject>();
        coinsToRelease = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        foreach(GameObject coin in activeCoins.Values)
        {
            Vector3 newPosition = coin.transform.position;
            newPosition = Vector3.MoveTowards(newPosition, coinBlueprint.transform.position, coinsMovementSpeed * Time.deltaTime);
            coin.transform.position = newPosition;
        }

        coinsToRelease.Clear();
        foreach (GameObject coin in activeCoins.Values)
        {
            float distance = Vector3.Distance(coin.transform.position, coinBlueprint.transform.position);
            if(distance < 0.1)
            {
                coinsToRelease.Add(coin);
                coinsCount++;
                text.text = coinsCount.ToString();
            }
        }

        foreach(GameObject coin in coinsToRelease)
        {
            coinsPool.Release(coin);
        }
    }

    public void AddCoins(int newCoins)
    {
        for (int i = 0; i < newCoins; i++)
        {
            GameObject go = coinsPool.Get();
            activeCoins[go.GetInstanceID()] = go;
        }

    }

    public void SubstractCoins(int coins)
    {
        if (coinsCount > 0)
        {
            coinsParticleSystem.Play();
            StartCoroutine(IE_SubstractCoins(coins));
        }
    }

    IEnumerator IE_SubstractCoins(int coins)
    {
        while(coins > 0 && coinsCount > 0)
        {
            coinsCount--;
            text.text = coinsCount.ToString();

            coins--;
            yield return new WaitForFixedUpdate();
        }
    }

    private GameObject OnCreateCoin()
    {
        GameObject go = Instantiate(coinBlueprint, coinsContainer.transform);
        go.SetActive(false);
        return go;
    }

    void OnReturnedToPool(GameObject coin)
    {
        activeCoins.Remove(coin.GetInstanceID());
        coin.SetActive(false);
    }

    void OnTakeFromPool(GameObject coin)
    {
        coin.SetActive(true);
        RectTransform rectTransform = coin.GetComponent<RectTransform>();

        Vector2 newPosition = coinsContainer.transform.position;
        newPosition += Random.insideUnitCircle * 3;
        rectTransform.transform.position = newPosition;
    }
}
