using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for heuristic brain ...
/// </summary>
public class DriveDecision : MonoBehaviour, Decision
{

    public float[] Decide(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        foreach (var v in vectorObs)
            Debug.Log($"vectorObs { v }");

        List<float> act = new List<float>();
        
        act.Add(vectorObs[0] * 10);
        
        act.Add(vectorObs[1] * 10);

        return act.ToArray();
    }

    public List<float> MakeMemory(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        return new List<float>();
    }
}
