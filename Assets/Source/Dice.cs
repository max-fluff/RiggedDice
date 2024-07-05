using System;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private List<Vector3> sideUpRotations;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform visual;

    private readonly List<TransformState> _recordedStates = new();

    public Rigidbody Rigidbody => rigidbody;

    public void SetTransformValues(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        transform.position = position;
        transform.rotation = rotation;
        rigidbody.velocity = velocity;
        rigidbody.angularVelocity = angularVelocity;
    }

    public void RecordState()
    {
        _recordedStates.Add(new TransformState
        {
            Position = transform.position,
            Rotation = transform.rotation
        });
    }

    public void RestoreState(int stateIndex)
    {
        var state = _recordedStates[stateIndex];

        transform.position = state.Position;
        transform.rotation = state.Rotation;
    }

    /// <summary>
    /// Поворот визуала кости для того, чтобы в результате симуляци выпало нужное значение
    /// </summary>
    /// <param name="value">Ожидаемое значение на кости</param>
    public void SetVisualRotation(int value)
    {
        var targetRotation = sideUpRotations[value - 1];

        var transformRotation = visual.transform.eulerAngles;

        if (Math.Abs(targetRotation.x - (-1)) > 0.001f) transformRotation.x = targetRotation.x;
        if (Math.Abs(targetRotation.y - (-1)) > 0.001f) transformRotation.y = targetRotation.y;
        if (Math.Abs(targetRotation.z - (-1)) > 0.001f) transformRotation.z = targetRotation.z;

        visual.transform.eulerAngles = transformRotation;
    }

    public void Destroy() => Destroy(gameObject);

    private struct TransformState
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}