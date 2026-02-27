using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BrewingUIController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] DrinkDatabase drinkDatabase;
    [SerializeField] BrewingRequestedEvent OnBrewingRequested;

    [Header("Ui Refs")]
    [SerializeField] Transform drinkListContainer;
    [SerializeField] Button drinkButtonPrefab;

    [SerializeField] Toggle hotToggle;
    [SerializeField] Toggle sweetToggle;

    [SerializeField] Button confirmButton;
}
