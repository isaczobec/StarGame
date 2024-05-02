using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{

    private PlayerInput playerInput;
    
    // Events
    public event EventHandler<Vector2> OnMomentaryInput;

    private void Awake()
    {
        // Initialize the and enable the player input
        playerInput = new PlayerInput();
        playerInput.Movement.Enable();

        playerInput.Movement.Down.performed += ctx => playerDirectionalHookInput(new Vector2(0,-1));
        playerInput.Movement.Up.performed += ctx => playerDirectionalHookInput(new Vector2(0,1));
        playerInput.Movement.Left.performed += ctx => playerDirectionalHookInput(new Vector2(-1,0));
        playerInput.Movement.Right.performed += ctx => playerDirectionalHookInput(new Vector2(1,0));

    }

    private void playerDirectionalHookInput(Vector2 direction)
    {
        OnMomentaryInput?.Invoke(this, direction);
    }



    public Vector2 GetPlayerMovementInput() {
        
        return playerInput.Movement.Move.ReadValue<Vector2>();
    }


}
