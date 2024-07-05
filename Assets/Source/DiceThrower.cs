using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source
{
    /// <summary>
    /// Бросатель костей
    /// </summary>
    public class DiceThrower : MonoBehaviour
    {
        [SerializeField] private Dice dicePrefab;

        private readonly List<Dice> _dices = new();

        public void ClearDices()
        {
            foreach (var dice in _dices)
                dice.Destroy();

            _dices.Clear();
        }
        
        /// <summary>
        /// Бросок костей
        /// </summary>
        /// <param name="values">Список значений, которые должны выпасть на костях</param>
        public async UniTask ThrowDiceAsync(List<int> values)
        {
            ClearDices();

            // Инстанциирование костей, инициализация рандомных значений трансформа и риджидбади
            for (var index = 0; index < values.Count; index++)
            {
                var dice = Instantiate(dicePrefab);

                var position = GetRandomDicePosition();
                var rotation = GetRandomDiceRotation();
                var velocity = GetRandomDiceVelocity();
                var angularVelocity = GetRandomDiceAngularVelocity();

                dice.SetTransformValues(position, rotation, velocity, angularVelocity);

                _dices.Add(dice);
            }

            // Симуляция броска костей
            var physicsFramesCount = Simulate();

            // Подготовка костей к воспроизведению записанного движения
            for (var index = 0; index < _dices.Count; index++)
            {
                var dice = _dices[index];
                dice.SetVisualRotation(values[index]);
                dice.Rigidbody.isKinematic = true;
            }

            // Покадровое воспроизведение записанного движения

            for (var i = 0; i < physicsFramesCount; i++)
            {
                foreach (var dice in _dices)
                    dice.RestoreState(i);

                await UniTask.WaitForFixedUpdate();
            }
        }

        /// <summary>
        /// Симуляция броска костей и запись состояний костей
        /// </summary>
        /// <returns> Количество симулированных кадров</returns>
        private int Simulate()
        {
            Physics.simulationMode = SimulationMode.Script;

            var physicsFramesCount = 0;

            while (_dices.Any(d =>
                       d.Rigidbody.velocity.magnitude > 0.0001f || d.Rigidbody.angularVelocity.magnitude > 0.0001f))
            {
                _dices.ForEach(d => d.RecordState());
                Physics.Simulate(Time.fixedDeltaTime);

                physicsFramesCount++;
            }

            Physics.simulationMode = SimulationMode.FixedUpdate;

            return physicsFramesCount;
        }

        #region Случайные параметры

        private Vector3 GetRandomDicePosition()
        {
            var positionX = Random.Range(-9, 9);
            var positionY = Random.Range(5, 15);
            var positionZ = Random.Range(-9, 9);

            return new Vector3(positionX, positionY, positionZ);
        }

        private Quaternion GetRandomDiceRotation()
        {
            var rotationX = Random.Range(0, 360);
            var rotationY = Random.Range(0, 360);
            var rotationZ = Random.Range(0, 360);

            return Quaternion.Euler(new Vector3(rotationX, rotationY, rotationZ));
        }

        private Vector3 GetRandomDiceVelocity()
        {
            var velocityX = Random.Range(-10, 10);
            var velocityY = Random.Range(-10, 10);
            var velocityZ = Random.Range(-10, 10);

            return new Vector3(velocityX, velocityY, velocityZ);
        }

        private Vector3 GetRandomDiceAngularVelocity()
        {
            var angularVelocityX = Random.Range(-10, 10);
            var angularVelocityY = Random.Range(-10, 10);
            var angularVelocityZ = Random.Range(-10, 10);

            return new Vector3(angularVelocityX, angularVelocityY, angularVelocityZ);
        }

        #endregion
    }
}