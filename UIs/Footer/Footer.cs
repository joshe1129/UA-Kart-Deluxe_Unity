using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Footer : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI[] mainText;

    RectTransform myRectTransf;
    float startingPoint;
    int messageIndex;
    int numberOfText;

    // Start is called before the first frame update
    void Start()
    {
        myRectTransf = gameObject.GetComponent<RectTransform>();
        startingPoint = myRectTransf.localPosition.x;
        numberOfText = mainText.Length;
        messageIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        myRectTransf.Translate(Vector3.left * speed * Time.deltaTime);
        if (myRectTransf.localPosition.x < - startingPoint - mainText[messageIndex].gameObject.GetComponent<RectTransform>().rect.width)
        {
            myRectTransf.localPosition = Vector3.right * startingPoint;
            SelectText(mainText);
        }
    }
    void SelectText(TextMeshProUGUI[] mainText)
    {
        messageIndex++;
        if (messageIndex > numberOfText - 1)
        {
            messageIndex = 0;
        }
        for (int i = 0; i < mainText.Length; i++)
        {
            if (i == messageIndex)
            {
                mainText[i].gameObject.SetActive(true);
            }
            else
            {
                mainText[i].gameObject.SetActive(false);
            }
        }
    }
}
