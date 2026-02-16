using UnityEngine;
using TMPro;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text handText;
    [SerializeField] private TMP_Text bagText;
    [SerializeField] private InventoryChangedEvent OnInventoryChanged;

    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = FindFirstObjectByType<PlayerInventory>();
    }

    private void OnEnable()
    {
        OnInventoryChanged.Raised += UpdateUI;
    }

    private void OnDisable()
    {
        OnInventoryChanged.Raised -= UpdateUI;
    }

    private void Start()
    {
        UpdateUI(inventory.PeekHand(), inventory.PeekBag());
    }

    private void UpdateUI(ItemData hand, ItemData bag)
    {
        handText.text = $"Hand: {FormatItem(hand)}";
        bagText.text = $"Bag: {FormatItem(bag)}";
    }

    string FormatItem(ItemData item)
    {
        return item != null ? item.itemName : "Empty";
    }
}
