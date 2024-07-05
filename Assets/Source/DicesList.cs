using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    /// <summary>
    /// Список ожидаемых значений для костей
    /// </summary>
    public class DicesList : MonoBehaviour
    {
        [SerializeField] private DicesListElement prefab;
        [SerializeField] private Transform container;
        [SerializeField, Range(2, 20)] private int maxDiceCount = 10;

        [Header("InputButtons")] [SerializeField]
        private Button addButton;

        [SerializeField] private Button removeButton;

        private readonly List<DicesListElement> _dices = new();

        private void Awake()
        {
            addButton.onClick.AddListener(AddDice);
            removeButton.onClick.AddListener(RemoveDice);

            AddDice();
        }

        public List<int> GetValues() => _dices.Select(d => d.GetValue()).ToList();

        private void AddDice()
        {
            removeButton.interactable = true;

            var dice = Instantiate(prefab, container);
            _dices.Add(dice);

            ProcessDiceCountUpdate();
        }

        private void RemoveDice()
        {
            addButton.interactable = true;

            var diceToRemove = _dices[^1];
            Destroy(diceToRemove.gameObject);
            _dices.Remove(diceToRemove);

            ProcessDiceCountUpdate();
        }

        private void ProcessDiceCountUpdate()
        {
            if (_dices.Count == maxDiceCount)
                addButton.interactable = false;

            if (_dices.Count == 1)
                removeButton.interactable = false;
        }
    }
}