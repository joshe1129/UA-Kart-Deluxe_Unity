using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;

    public RaceVariable <int> lap = new RaceVariable <int>();
    public RaceVariable <int> ranking = new RaceVariable <int>();
    public RaceVariable <int> coins = new RaceVariable <int>();
    public RaceVariable <string> timerMinutes = new RaceVariable<string>();
    public RaceVariable <string> timerSeconds = new RaceVariable<string>();
    public RaceVariable <string> timerSeconds100 = new RaceVariable<string>();
    public RaceVariable <bool> raceStart = new RaceVariable<bool>();


    private float startTime;
    private float stopTime;
    private float timerTime;
    public bool isRunning = false;

    private List<CheackPointSingle> cheackPointSingleList;
    private int nextCheckPointIndex;
   

    public GameObject[] carOrder = new GameObject[4];

    [SerializeField] 
    private GameObject[] allCars;

    private void Awake()
    {
        instance = this;
        Transform checkpointsTransform = transform.Find("Checkpoints");
        cheackPointSingleList = new List<CheackPointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheackPointSingle cheackPointSingle = checkpointSingleTransform.GetComponent<CheackPointSingle>();
            cheackPointSingle.SetTrackCheckpoints(this);
            cheackPointSingleList.Add(cheackPointSingle);
        }
        nextCheckPointIndex = 0;    
    }

    void Start()
    {
        coins.Value = 0;
        ranking.Value = 4;
        allCars = GameObject.FindGameObjectsWithTag("Player");
        
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.B))
        {
            coins.Value++;
        }
        /*
        foreach (CatTest car in allCars)
        {
            //Debug.Log(car.GetCarPosition(allCars) - 1);
            carOrder[car.GetCarPosition(allCars) - 1] = car;
        }
        */
        timerTime = stopTime + (Time.time - startTime);
        int minutesInt = (int)timerTime / 60;
        int secondsInt = (int)timerTime % 60;
        int seconds100Int = (int)(Mathf.Floor((timerTime - (secondsInt + minutesInt * 60)) * 100));

        if (isRunning)
        {
            timerMinutes.Value = (minutesInt < 10) ? "0" + minutesInt : minutesInt.ToString();
            timerSeconds.Value = (secondsInt < 10) ? "0" + secondsInt : secondsInt.ToString();
            timerSeconds100.Value = (seconds100Int < 10) ? "0" + seconds100Int : seconds100Int.ToString();
        }
    }
    public void TimerStart()
    {
        isRunning = true;
        startTime = Time.time;
    }
    public void TimerReset()
    {
        stopTime = 0;
        isRunning = false;
        timerMinutes.Value = timerSeconds.Value = timerSeconds100.Value = "00";
    }

    public void PlayerThroughCheckpoint(CheackPointSingle cheackPointSingle)
    {
        if(cheackPointSingleList.IndexOf(cheackPointSingle) == nextCheckPointIndex)
        {
            if (nextCheckPointIndex == 0)
            {
                lap.Value++;
                if (lap.Value >= 2)
                {
                    Debug.Log("time of the lap: " + timerMinutes.Value + " : " + timerSeconds.Value + " : " + timerSeconds100.Value);
                }
                RaceManager.instance.TimerReset();
                RaceManager.instance.TimerStart();
            }
            Debug.Log("correct");
            nextCheckPointIndex = (nextCheckPointIndex + 1) % cheackPointSingleList.Count;            
        }
        else
        {
            Debug.Log("wrong");
        }
    }

}


public class RaceVariable<T>
{
    private protected T m_InternalValue;

    public delegate void OnValueChangedDelegate(T newValue);
    public OnValueChangedDelegate OnValueChanged;

    public RaceVariable(T value = default)
    {
        m_InternalValue = value;
    }

    public virtual T Value
    {   
        get => m_InternalValue; 
        set
        {
            Set(value);
        }
    }
    private protected void Set(T value)
    {
        m_InternalValue = value;
        OnValueChanged?.Invoke(m_InternalValue);
    }
}