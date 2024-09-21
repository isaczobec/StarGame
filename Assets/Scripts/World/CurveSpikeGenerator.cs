using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

/// <summary>
/// Return type for when sampling points on the curve
/// </summary>
public class SplinePointsInfo {
    public List<Vector3> positions {get; private set;}
    public List<Vector3> tangentials {get; private set;}

    public SplinePointsInfo(List<Vector3> positions, List<Vector3> tangentials) {
        this.positions = positions;
        this.tangentials = tangentials;
    }
}

public class SplineCurve {

    public SplinePoint[] splinePoints;
    private float stepSize = 0.009f;
    public SplineCurve(SplinePoint[] points, float stepSize = 0.009f) {
        splinePoints = points;
        this.stepSize = stepSize;

    }

    /// <summary>
    /// Returns a position on the curve from a value between 0 and 1 (start and finnish)
    /// </summary>
    /// <param name="t">between start and finnish of the curve</param>
    /// <returns></returns>
    public Vector3 GetCurvePosition(float t) {

        // get the index of wich curve to sample and what the local t is
        int curveIndex = Mathf.FloorToInt(t * (splinePoints.Length - 1));
        if (curveIndex == splinePoints.Length - 1) curveIndex--; // if we are at the end of the curve, go back one (to avoid out of bounds error
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
        if (curveIndex == splinePoints.Length - 1) curveIndex--; // if we are at the end of the curve, go back one (to avoid out of bounds error
        float localT = (t * (splinePoints.Length - 1)) - curveIndex;

        //calculate the derivative of the bernstein polynomial
        Vector3 p0 = splinePoints[curveIndex].position * (-3*Mathf.Pow(localT, 2) + 6*localT - 3);
        Vector3 p1 = splinePoints[curveIndex].handle2Position * (9*Mathf.Pow(localT, 2) - 12*localT + 3);
        Vector3 p2 = splinePoints[curveIndex+1].handle1Position * (-9*Mathf.Pow(localT, 2) + 6*localT);
        Vector3 p3 = splinePoints[curveIndex+1].position * 3*Mathf.Pow(localT, 2);

        Vector3 p = p0 + p1 + p2 + p3;

        return p;
        
    }

    /// <summary>
    /// Steps along the curve for and returns a list of points and tangentials
    /// </summary>
    /// <param name="amountOfPoints"></param>
    /// <returns></returns>
    public SplinePointsInfo StepSpline(int amountOfPoints, bool returnTangential = true) {

        List<Vector3> positions = new List<Vector3>();
        List<Vector3> tangentials = returnTangential ? new List<Vector3>() : null; // if we want to return tangentials, create and init a list

        for (int i = 0; i < amountOfPoints; i++) {
            float t = (float)i / (amountOfPoints - 1); // Normalized value between 0 and 1

            Vector3 curvePosition = GetCurvePosition(t);
            positions.Add(curvePosition);

            if (returnTangential) {
                Vector3 curveTangential = GetCurveTangential(t);
                tangentials.Add(curveTangential);
            }
        }

        return new SplinePointsInfo(positions, tangentials);

    }

    // public SplinePointsInfo StepSplineEvenlySpaced(float desiredSpace, float baseStepLength = 0.007f, float stepModifierMultiplier = 1.1f, bool returnTangential = true) {

    //     List<Vector3> positions = new List<Vector3>() {splinePoints[0].position};
    //     List<Vector3> tangentials = returnTangential ? new List<Vector3>() {GetCurveTangential(0)} : null; // if we want to return tangentials, create and init a list


    //     float t = baseStepLength; // dont start at 0
    //     float cumulativeDistance = 0;
    //     int counter = 1;
    //     float stepModifier = 1;

    //     Vector2 currentPosition = GetCurvePosition(0);

    //     while (t < 1f) {

    //         Debug.Log("T: " + t);
    //         Vector3 nextPosition = GetCurvePosition(t);
    //         cumulativeDistance += Vector3.Distance(currentPosition, nextPosition);
    //         currentPosition = nextPosition;

    //         if (cumulativeDistance >= desiredSpace*counter) { // if we are within the desired space or within the error margin, add the point to the list
    //             positions.Add(nextPosition);
    //             if (returnTangential) {
    //                 tangentials.Add(GetCurveTangential(t));
    //             }
    //             // increase counter and reset step modifier
    //             counter++;
    //             stepModifier = 1;
    //         } 
    //         t = Mathf.Clamp(t + baseStepLength * stepModifier, 0f, 5f);
        
    //     }
    //     return new SplinePointsInfo(positions, tangentials);
    
    // }


    public SplinePointsInfo StepSplineEvenlySpaced(float desiredSpace, float baseStepLength = 0f, float backTrackMultiplier = 0.6f, float maxError = 0.08f, bool returnTangential = true, int maxIterations = 50000) {

        if (baseStepLength == 0) baseStepLength = stepSize; // if we dont specify a step length, use the default stepSize

        List<Vector3> positions = new List<Vector3>() {splinePoints[0].position};
        List<Vector3> tangentials = returnTangential ? new List<Vector3>() {GetCurveTangential(0)} : null; // if we want to return tangentials, create and init a list


        float t = baseStepLength; // dont start at 0
        float cumulativeDistance = 0;
        int counter = 1;
        float stepModifier = 1f;

        int iterations = 0;

        Vector2 currentPosition = GetCurvePosition(0f);

        while (t < 1) {

            Vector3 nextPosition = GetCurvePosition(t);
            cumulativeDistance += Vector3.Distance(currentPosition, nextPosition) * stepModifier/Mathf.Abs(stepModifier);
            currentPosition = nextPosition;

            if (cumulativeDistance >= desiredSpace*counter && cumulativeDistance <= desiredSpace*counter + maxError) { // if we are within the desired space or within the error margin, add the point to the list
                positions.Add(nextPosition);
                if (returnTangential) {
                    tangentials.Add(GetCurveTangential(t));
                }
                // increase counter and reset step modifier
                counter++;
                stepModifier = 1f;
            } else if (cumulativeDistance > desiredSpace*counter + maxError) {  // if we overshot it
                if (stepModifier > 0) {
                    stepModifier = -stepModifier * backTrackMultiplier; // backtrack and reduce the step modifier
                }
            } else { // if we are under the desired space
                if (stepModifier < 0)  {
                    stepModifier = -stepModifier * backTrackMultiplier; // if we are backtracking, reset the step modifier
                    }

            }

            t = Mathf.Clamp(t + baseStepLength * stepModifier, 0f, 1f);

            iterations++;
            if (iterations > maxIterations) {
                Debug.LogError("Max iterations reached");
                break;
            }
            




        }
        return new SplinePointsInfo(positions, tangentials);


    }
    public SplinePointsInfo StepSplineSideEvenlySpaced(bool left, float gap, float desiredSpace, float baseStepLength = 0f, float backTrackMultiplier = 0.6f, float maxError = 0.08f, bool returnTangential = true, int maxIterations = 500000) {

        if (baseStepLength == 0) baseStepLength = stepSize; // if we dont specify a step length, use the default stepSize

        int direction = left ? -1 : 1;

        List<Vector3> positions = new List<Vector3>();
        List<Vector3> tangentials = returnTangential ? new List<Vector3>() : null; // if we want to return tangentials, create and init a list


        float t = baseStepLength; // dont start at 0
        float cumulativeDistance = 0;
        int counter = 1;
        float stepModifier = 1f;

        int iterations = 0;

        Vector2 firstTangent = GetCurveTangential(0);
        Vector2 currentPosition = GetCurvePosition(0f) + new Vector3(firstTangent.y,-firstTangent.x).normalized * direction * gap;
        positions.Add(currentPosition);
        tangentials.Add(firstTangent);

        while (t < 1) {

            Vector3 tangent = GetCurveTangential(t).normalized;
            Vector3 normal = new Vector2(tangent.y, -tangent.x);
            Vector3 nextPosition = GetCurvePosition(t) + normal * direction * gap;
            cumulativeDistance += Vector3.Distance(currentPosition, nextPosition) * stepModifier/Mathf.Abs(stepModifier);
            currentPosition = nextPosition;

            if (cumulativeDistance >= desiredSpace*counter && cumulativeDistance <= desiredSpace*counter + maxError) { // if we are within the desired space or within the error margin, add the point to the list
                positions.Add(nextPosition);
                if (returnTangential) {
                    tangentials.Add(GetCurveTangential(t));
                }
                // increase counter and reset step modifier
                counter++;
                stepModifier = 1f;
            } else if (cumulativeDistance > desiredSpace*counter + maxError) {  // if we overshot it
                if (stepModifier > 0) {
                    stepModifier = -stepModifier * backTrackMultiplier; // backtrack and reduce the step modifier
                }
            } else { // if we are under the desired space
                if (stepModifier < 0)  {
                    stepModifier = -stepModifier * backTrackMultiplier; // if we are backtracking, reset the step modifier
                    }

            }

            t = Mathf.Clamp(t + baseStepLength * stepModifier, 0f, 1f);

            iterations++;
            if (iterations > maxIterations) {
                Debug.LogError("Max iterations reached");
                break;
            }
            




        }
        return new SplinePointsInfo(positions, tangentials);


    }
    

}
public class CurveSpikeGenerator : MonoBehaviour, ISpawnFromEditorObjectData
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

    [SerializeField] private bool tunnel = true;
    [SerializeField] private float spikeDistance = 0.5f;
    [SerializeField] private float spikeGap = 1.6f;
    [SerializeField] private bool flipSpikes = false;



    private List<SplineCurve> splineCurves = new List<SplineCurve>();

    /// <summary>
    /// If the spikes have been generated.
    /// </summary>
    private bool generated = false;



    private void Start()
    {
        
        if (!generated) { // only generate the spikes once

            // splinePoint1 = new SplinePoint(P1.position, P1Hanlde1.position, P1Hanlde2.position);
            // splinePoint2 = new SplinePoint(P2.position, P2Hanlde1.position, P2Hanlde2.position);
            // splinePoint3 = new SplinePoint(P3.position, P3Hanlde1.position, P3Hanlde2.position);

            // AddSplineCurve(new SplinePoint[] { splinePoint1, splinePoint2, splinePoint3 });

            // GenerateAppropriateSpikes();
            
        } 

        // for (int i = 0; i < amountOfSpikes; i++) { 

        //     Debug.Log("Position: " + splinePointsInfo.positions[i] + " Tangential: " + splinePointsInfo.tangentials[i]);
        //     Instantiate(testPrefab, splinePointsInfo.positions[i], Quaternion.LookRotation(Vector3.forward,-splinePointsInfo.tangentials[i]));

        // }


    }

    /// <summary>
    /// Generates a spike tunnel based on the current settings.
    /// </summary>
    public void GenerateAppropriateSpikes(bool destroyOld = false) {
        if (destroyOld) DestroyChildren();
        if (tunnel) GenerateTunnelSpikes();
            else GenerateStrandSpikes();
    }

    private void GenerateTunnelSpikes(int curveIndex = 0)
    {
        float time = Time.realtimeSinceStartup;
        SplinePointsInfo splinePointsInfoL = splineCurves[curveIndex].StepSplineSideEvenlySpaced(true, spikeGap, spikeDistance);
        SplinePointsInfo splinePointsInfoR = splineCurves[curveIndex].StepSplineSideEvenlySpaced(false, spikeGap, spikeDistance);

        int si = 0;
        foreach (SplinePointsInfo info in new SplinePointsInfo[] { splinePointsInfoL, splinePointsInfoR })
        {
            for (int i = 0; i < info.positions.Count; i++)
            {

                Vector2 curveNormal = flipSpikes ? new Vector2(info.tangentials[i].y, -info.tangentials[i].x) : new Vector2(-info.tangentials[i].y, info.tangentials[i].x);
                curveNormal = si == 0 ? curveNormal : -curveNormal;
                Instantiate(testPrefab, info.positions[i], Quaternion.LookRotation(Vector3.forward, -curveNormal), transform);

            }
            si++;
        }
        generated = true;
    }

    private void GenerateStrandSpikes(int curveIndex = 0)
    {
        float time = Time.realtimeSinceStartup;
        SplinePointsInfo splinePointsInfo = splineCurves[curveIndex].StepSplineEvenlySpaced(spikeDistance);
        Debug.Log("Time to generate bezier: " + (Time.realtimeSinceStartup - time));

            for (int i = 0; i < splinePointsInfo.positions.Count - 1; i++)
            {

                Vector2 curveNormal = flipSpikes ? new Vector2(splinePointsInfo.tangentials[i].y, -splinePointsInfo.tangentials[i].x) : new Vector2(-splinePointsInfo.tangentials[i].y, splinePointsInfo.tangentials[i].x);
                Instantiate(testPrefab, splinePointsInfo.positions[i], Quaternion.LookRotation(Vector3.forward, -curveNormal));

            }
        generated = true;
    }

    /// <summary>
    /// Destroys all children of the object, in this case the spikes.s
    /// </summary>
    private void DestroyChildren() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    // private void Start() {

    //     splinePoint1 = new SplinePoint(P1.position, P1Hanlde1.position, P1Hanlde2.position);
    //     splinePoint2 = new SplinePoint(P2.position, P2Hanlde1.position, P2Hanlde2.position);
    //     splinePoint3 = new SplinePoint(P3.position, P3Hanlde1.position, P3Hanlde2.position);

    //     AddSplineCurve(new SplinePoint[] {splinePoint1, splinePoint2, splinePoint3});

    //     int amountOfSteps = amountOfSpikes;

    //     for (int i = 0; i < amountOfSteps; i++) {
    //         float t = (float)i / (amountOfSteps - 1); // Normalized value between 0 and 1

    //         Vector3 curvePosition = splineCurves[0].GetCurvePosition(t);
    //         Vector3 curveTangential = splineCurves[0].GetCurveTangential(t).normalized;
    //         Vector3 curveNormalXYPlane = new Vector3(curveTangential.y, -curveTangential.x, 0);

    //         Instantiate(testPrefab, curvePosition + curveNormalXYPlane * spikeGap, Quaternion.LookRotation(Vector3.forward,-curveNormalXYPlane));
    //         Instantiate(testPrefab, curvePosition - curveNormalXYPlane * spikeGap, Quaternion.LookRotation(Vector3.forward,curveNormalXYPlane));



    //     }


    // }

    /// <summary>
    /// Adds a spline curve to the list of spline curves
    /// </summary>
    /// <param name="splinePoints"></param>
    private void AddSplineCurve(SplinePoint[] splinePoints) {
        splineCurves.Add(new SplineCurve(splinePoints));
    }

    public void CopyEditorObjectData(EditorObjectData editorObjectData)
    {

        // copy spacing
        spikeGap = editorObjectData.GetSetting<float>("Tunnel gap");
        spikeDistance = editorObjectData.GetSetting<float>("Spike spacing");


        // get positions and create spline points
        Vector2[] nodePositions = new Vector2[editorObjectData.editorObjectNodes.Count];
        for (int i = 0; i < editorObjectData.editorObjectNodes.Count; i++)
        {
            nodePositions[i] = editorObjectData.editorObjectNodes[i].relativePosition;
        }
        Vector2 rootPosition = editorObjectData.position; // get the root position of the object

        CreateSplineCurveFromNodePositions(nodePositions, rootPosition);

        GenerateAppropriateSpikes();

    }

    /// <summary>
    /// Creates a spline curve from the node positions and the root position and adds it to the spline curves list.
    /// </summary>
    /// <param name="nodePositions"></param>
    /// <param name="rootPosition"></param>
    public void CreateSplineCurveFromNodePositions(Vector2[] nodePositions, Vector2 rootPosition)
    {

        SplinePoint[] splinePoints = new SplinePoint[nodePositions.Length / 3];
        for (int i = 0; i < nodePositions.Length / 3; i++)
        {
            Vector2 handle1 = nodePositions[i * 3] + rootPosition;
            Vector2 point = nodePositions[i * 3 + 1] + rootPosition;
            Vector2 handle2 = nodePositions[i * 3 + 2] + rootPosition;
            splinePoints[i] = new SplinePoint(point, handle1, handle2);
        }
        // add the spline points to the spline curves
        AddSplineCurve(splinePoints);
    }

    /// <summary>
    /// Clears all the spline curves.
    /// </summary>
    public void ClearSplines() {
        splineCurves.Clear();
    }

    public void SetSpikeGap(float gap) {
        spikeGap = gap;
    }
    public void SetSpikeDistance(float distance) {
        spikeDistance = distance;
    }
}
