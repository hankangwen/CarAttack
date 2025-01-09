using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour
{
    [SerializeField] private Vector3 _size;
    public Vector3 size => _size;
    public float width => size.x;
    public float lenght => size.z;
}
