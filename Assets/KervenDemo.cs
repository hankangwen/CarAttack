using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KervenDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // var value = float.IsNaN(1.0f/0.0f);
        var a = 1.0f;
        var b = 0.0f;
        var value = a / b;
        Debug.LogError(value);
        Debug.LogError(float.IsInfinity(value));
        Debug.LogError(float.IsFinite(value));
        Debug.LogError(float.IsNaN(value));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var value = float.IsInfinity(float.PositiveInfinity);
            Debug.LogError(value);
        }
    }
}
