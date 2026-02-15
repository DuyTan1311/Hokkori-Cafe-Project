using UnityEngine;
using TMPro;
public class MoneyTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private MoneyChangedEvent onMoneyChanged;

    private void OnEnable()
    {
        onMoneyChanged.Raised += UpdateText;
    }

    private void OnDisable()
    {
        onMoneyChanged.Raised -= UpdateText;
    }

    private void UpdateText(int amount)
    {
        text.text = $"Money: {amount}";
    }
}
