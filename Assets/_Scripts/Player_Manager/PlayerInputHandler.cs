using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    CustomInput input;
    PlayerInteraction interaction;
    PlayerMovement movement;
    PlayerInventory inventory;

    private void Awake()
    {
        input = new CustomInput();
        interaction = GetComponentInChildren<PlayerInteraction>();
        inventory = GetComponentInChildren<PlayerInventory>();
        movement = GetComponent<PlayerMovement>();
        AssignInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void AssignInput()
    {
        input.Player.Move.performed += OnMove;
        input.Player.Interact.performed += OnInteract;
        input.Player.SwapItem.performed += OnSwapItem;
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        interaction.TryInteract();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movement.ClicktoMove();
    }

    private void OnSwapItem(InputAction.CallbackContext context)
    {
        inventory.SwapItem();
    }
}
