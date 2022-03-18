using System;
using UnityEngine;

public class SelectionMarker : MonoBehaviour
{
    public float lifetime = 1.0f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
