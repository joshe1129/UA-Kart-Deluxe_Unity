using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScrolTextManager : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float textPosBegin;
    [SerializeField] float boundaryTextEnd;
    [SerializeField] TextMeshProUGUI [] mainText;
    RectTransform myRectTransf;
    float startingPositionx;
    RectTransform originalPos;
    int messageIndex;
    // Start is called before the first frame update
    void Start()
    {
        myRectTransf = gameObject.GetComponent<RectTransform>();
        startingPositionx = myRectTransf.localPosition.x;
        originalPos = myRectTransf;
        messageIndex = 0;
        SelectText(mainText);
    }
    void Update()
    {
        myRectTransf.Translate(Vector3.left * speed * Time.deltaTime);
        if(myRectTransf.localPosition.x < - startingPositionx)
        {
            myRectTransf.localPosition = Vector3.right * startingPositionx;
            SelectText(mainText);
        }
    }
    void SelectText(TextMeshProUGUI [] mainText)
    {
        for (int i = 0; i < mainText.Length; i ++)
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
        messageIndex++;
        if(messageIndex>3)
        {
            messageIndex = 0;
        }
    }
}
