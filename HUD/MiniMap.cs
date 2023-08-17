using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField]
    private float y;
    [SerializeField]
    private float startWidth;
    [SerializeField]
    private float endWidth;
    [SerializeField]
    private GameObject raceCircuit;


    private LineRenderer lineRenderer;
    private GameObject trackPath;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        trackPath = this.gameObject;
        
        int numOfPath = raceCircuit.transform.childCount;
        lineRenderer.positionCount = numOfPath + 1;
        for(int i = 0; i < numOfPath; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(raceCircuit.transform.GetChild(i).transform.position.x, y, raceCircuit.transform.GetChild(i).transform.position.z));
        }
        lineRenderer.SetPosition(numOfPath, lineRenderer.GetPosition(0));
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.loop = true;
        lineRenderer.Simplify(0.3f);
    }
}
