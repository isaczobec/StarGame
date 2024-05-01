using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public struct SplinePoint
{
    public Vector3 position;
    public Vector3 handle1Position; // the point that comes before the curve moves forward
    public Vector3 handle2Position; // the point that comes after the curve moves forward

    public SplinePoint(Vector3 pos, Vector3 handle1, Vector3 handle2) {

        position = pos;
        handle1Position = handle1;
        handle2Position = handle2;
        
    }
    
}

public class SplineCurve {

    public SplinePoint[] splinePoints;
    public SplineCurve(SplinePoint[] points) {
        splinePoints = points;

    }

    /// <summary>
    /// Returns a position on the curve from a value between 0 and 1 (start and finnish)
    /// </summary>
    /// <param name="t">between start and finnish of the curve</param>
    /// <returns></returns>
    public Vector3 GetCurvePosition(float t) {

        // get the index of wich curve to sample and what the local t is
        int curveIndex = Mathf.FloorToInt(t * (splinePoints.Length - 1));
        float localT = (t * (splinePoints.Length - 1)) - curveIndex;

        // calculate the position of the curve using the bezier formula
        Vector3 p0 = splinePoints[curveIndex].position * (-Mathf.Pow(localT, 3) + 3*Mathf.Pow(localT, 2) - 3*localT + 1);
        Vector3 p1 = splinePoints[curveIndex].handle2Position * (3*Mathf.Pow(localT, 3) - 6*Mathf.Pow(localT, 2) + 3*localT);
        Vector3 p2 = splinePoints[curveIndex+1].handle1Position * (-3*Mathf.Pow(localT, 3) + 3*Mathf.Pow(localT, 2));
        Vector3 p3 = splinePoints[curveIndex+1].position * Mathf.Pow(localT, 3);

        Vector3 p = p0 + p1 + p2 + p3;

        return p;
    }

    public Vector3 GetCurveTangential(float t) {

        // get the index of wich curve to sample and what the local t is
        int curveIndex = Mathf.FloorToInt(t * (splinePoints.Length - 1));
        float localT = (t * (splinePoints.Length - 1)) - curveIndex;

        //calculate the derivative of the bernstein polynomial
        Vector3 p0 = splinePoints[curveIndex].position * (-3*Mathf.Pow(localT, 2) + 6*localT - 3);
        Vector3 p1 = splinePoints[curveIndex].handle2Position * (9*Mathf.Pow(localT, 2) - 12*localT + 3);
        Vector3 p2 = splinePoints[curveIndex+1].handle1Position * (-9*Mathf.Pow(localT, 2) + 6*localT);
        Vector3 p3 = splinePoints[curveIndex+1].position * 3*Mathf.Pow(localT, 2);

        Vector3 p = p0 + p1 + p2 + p3;

        return p;
        
    }
    

}
public class CurveSpikeGenerator : MonoBehaviour
{

    [SerializeField] private GameObject testPrefab;

    [SerializeField] private Transform P1Hanlde1;
    [SerializeField] private Transform P1;
    [SerializeField] private Transform P1Hanlde2;

    SplinePoint splinePoint1;
    
    [SerializeField] private Transform P2Hanlde1;
    [SerializeField] private Transform P2;
    [SerializeField] private Transform P2Hanlde2;
    SplinePoint splinePoint2;
    [SerializeField] private Transform P3Hanlde1;
    [SerializeField] private Transform P3;
    [SerializeField] private Transform P3Hanlde2;
    SplinePoint splinePoint3;

    [SerializeField] private float spikeGap = 2f;
    [SerializeField] private int amountOfSpikes = 30;



    private void Start() {
        splinePoint1 = new SplinePoint(P1.position, P1Hanlde1.position, P1Hanlde2.position);
        splinePoint2 = new SplinePoint(P2.position, P2Hanlde1.position, P2Hanlde2.position);
        splinePoint3 = new SplinePoint(P3.position, P3Hanlde1.position, P3Hanlde2.position);

        SplineCurve splineCurve = new SplineCurve(new SplinePoint[] {splinePoint1, splinePoint2, splinePoint3});

        int amountOfSteps = amountOfSpikes;

        for (int i = 0; i < amountOfSteps; i++) {
            float t = (float)i / (amountOfSteps - 1); // Normalized value between 0 and 1

            Vector3 curvePosition = splineCurve.GetCurvePosition(t);
            Vector3 curveTangential = splineCurve.GetCurveTangential(t).normalized;
            Vector3 curveNormalXYPlane = new Vector3(curveTangential.y, -curveTangential.x, 0);

            Instantiate(testPrefab, curvePosition + curveNormalXYPlane * spikeGap, Quaternion.identity);
            Instantiate(testPrefab, curvePosition - curveNormalXYPlane * spikeGap, Quaternion.identity);



        }


    }
    
}
