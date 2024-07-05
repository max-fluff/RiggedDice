using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    /// <summary>
    /// Связывает UI и бросок костей
    /// </summary>
    public class DiceBinder : MonoBehaviour
    {
        [SerializeField] private DicesList dicesList;
        [SerializeField] private Button rollButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private DiceThrower thrower;

        private void Awake()
        {
            rollButton.onClick.AddListener(Roll);
            clearButton.onClick.AddListener(thrower.ClearDices);
        }

        private void Roll() => RollAsync().Forget();

        private async UniTask RollAsync()
        {
            rollButton.interactable = false;
            clearButton.interactable = false;

            var values = dicesList.GetValues();
            await thrower.ThrowDiceAsync(values);

            rollButton.interactable = true;
            clearButton.interactable = true;
        }
    }
}