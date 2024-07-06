using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private List<Transform> sides;

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
        var sidesOrdered = sides.OrderBy(s => s.position.y).ToList();
        var topSide = sidesOrdered.Last();
        
        var vectorToCurrentTop = topSide.localPosition;
        var vectorToWantedTop = sides[value - 1].transform.localPosition;

        var vectorToRotate = Quaternion.FromToRotation(vectorToWantedTop, vectorToCurrentTop);

        visual.transform.localRotation *= vectorToRotate;
    }

    public void Destroy() => Destroy(gameObject);

    private struct TransformState
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}