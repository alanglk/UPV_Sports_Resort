using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InvisibleCapsuleWall : MonoBehaviour
{

    private int score = 0;
    // Track which balls we've already counted
    private HashSet<int> scoredThisPass = new HashSet<int>();

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only care about tagged basketballs
        if (!other.CompareTag("Basketball"))
            return;

        // Figure out which root object to identify
        GameObject root = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        int id = root.GetInstanceID();
        // If we've already scored this ball, skip
        if (scoredThisPass.Contains(id))
            return;

        // Otherwise, count it once
        scoredThisPass.Add(id);
        score++;
    }

    private void OnTriggerExit(Collider other)
    {
        // When it leaves, allow it to score again next time
        if (!other.CompareTag("Basketball"))
            return;

        GameObject root = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        scoredThisPass.Remove(root.GetInstanceID());
    }


    /// <summary>
    /// Permite a otros scripts leer la puntuación actual.
    /// </summary>
    public int CurrentScore
    {
        get { return score; }
    }
}
