using TMPro;
using UnityEngine;

namespace Source
{
    public class DicesListElement : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private void Awake()
        {
            inputField.onEndEdit.AddListener(ProcessEndEdit);
            inputField.SetTextWithoutNotify("0");
        }

        private void ProcessEndEdit(string value)
        {
            if (string.IsNullOrEmpty(value))
                inputField.SetTextWithoutNotify("0");
            else
            {
                var intValue = Mathf.Clamp(int.Parse(value), 0, 6);
                inputField.SetTextWithoutNotify(intValue.ToString());
            }
        }

        public int GetValue() => int.Parse(inputField.text);
    }
}