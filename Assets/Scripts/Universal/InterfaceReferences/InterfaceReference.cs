using UnityEngine;

[System.Serializable]
public class InterfaceReference<T> where T : class
{
    [SerializeField] private Object reference;

    public T Reference => reference as T;

    public static implicit operator T(InterfaceReference<T> wrapper) => wrapper.Reference;
}