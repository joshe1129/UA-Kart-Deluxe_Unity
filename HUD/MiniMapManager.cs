using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * The size of the line has to be variable
 * The y position of the dynamic camera can be changed by the game designer according to the different levels  
 */

public class MiniMapManager : MonoBehaviour
{
    [SerializeField] 
    private RenderTexture [] renderTextures;
    [SerializeField]
    private bool rawImageSwitch;

    private RawImage rawImage;
    private delegate void OnValueChangedDelegate(bool b);
    private OnValueChangedDelegate OnValueChanged;

    void Start()
    {
        OnValueChanged += (bool b) => rawImage.texture = renderTextures[Convert.ToInt32(b)];
        rawImage = gameObject.GetComponent<RawImage>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnValueChanged?.Invoke(rawImageSwitch);
            rawImageSwitch = !rawImageSwitch;
        }
    }
}
