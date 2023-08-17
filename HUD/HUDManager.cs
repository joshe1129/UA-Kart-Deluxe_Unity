using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lapText;
    [SerializeField]
    private TextMeshProUGUI minutesText;
    [SerializeField]
    private TextMeshProUGUI secondsText;
    [SerializeField]
    private TextMeshProUGUI seconds100Text;
    [SerializeField]
    private TextMeshProUGUI rankingText;
    [SerializeField]
    private TextMeshProUGUI rankingOrdinal;
    //[SerializeField]
    //private TextMeshProUGUI coinsText; 
    [SerializeField]
    private int totalLaps;

    public void Start()
    {
        RaceManager.instance.lap.OnValueChanged += (int i) => lapText.text = i.ToString() + "/" + totalLaps;
        RaceManager.instance.ranking.OnValueChanged += (int i) => rankingText.text = i.ToString();
        RaceManager.instance.ranking.OnValueChanged += (int i) => SetOrdinal(i);
        RaceManager.instance.timerMinutes.OnValueChanged += (string s) => minutesText.text = s;
        RaceManager.instance.timerSeconds.OnValueChanged += (string s) => secondsText.text = s;
        RaceManager.instance.timerSeconds100.OnValueChanged += (string s) => seconds100Text.text = s;
        //RaceManager.instance.coins.OnValueChanged += (int i) => coinsText.text = i.ToString();
    }
    void SetOrdinal(int i)
    {
        switch (i)
        {
            case 1:
                rankingOrdinal.text = "st";
                break;
            case 2:
                rankingOrdinal.text = "  nd";
                break;
            case 3:
                rankingOrdinal.text = " rd";
                break;
            case 4:
                rankingOrdinal.text = " th";
                break;
        }
    }
}
