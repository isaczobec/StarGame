//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Settings/PlayerInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""e941079e-7137-4138-92b6-ca29fb5b3d6f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""944f5b58-2844-4ac5-8c27-2b04b2fa5822"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""7b28305a-803b-4deb-b534-74eb1d85230f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""091b4a0c-5091-49b5-8de5-7297798cef8e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""536b520c-10f6-4971-97aa-bef4d28b195e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""c2b33713-2d25-4c64-b67c-58e687272756"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""99564766-1565-4fab-927b-bd3852f5a0a0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e8def0da-9292-4733-b3d5-ade2c225d8f8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c2a7b66b-39da-4091-bc76-31e750de7fa0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a716e425-1dd9-4bb5-81c6-544aea98a7cd"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""637b22ba-efc2-4c2a-b244-1c65224b6166"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""RTIO"",
                    ""id"": ""176319df-90a4-4320-9003-68c6c8ffa3a0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7109422f-630f-43a4-8fa7-aeca0e2cd421"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""93a4c82d-1733-4c10-a944-876ac9ef41d1"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fbf6a129-4d09-4ffa-ae76-25054364486b"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""41343776-ca1f-41b6-969a-a651be2d714f"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5eb27caa-a20d-4516-96e7-2c6ef7b1bf0e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4383dfb9-b434-4f29-8503-decdc90b234e"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de7a2023-d3ec-4a25-ac00-4996d13b0368"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c50b3c54-60c0-4271-bb83-83afb0d4c9ee"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07932d34-e437-45ff-8a56-c6409dd06cff"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61e66209-0228-4e73-8372-9317326b1947"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9c09e1e-5c23-4da0-a8d9-623443ddafff"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e5d0981-fa05-44dc-b336-edba84681c33"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Editor"",
            ""id"": ""cdc5d5b9-32b4-4b54-bb82-faf17355ffa2"",
            ""actions"": [
                {
                    ""name"": ""Place"",
                    ""type"": ""Value"",
                    ""id"": ""4f746a3c-09ed-47dc-9f6f-6a6bf64a7f1f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CameraMoveMode"",
                    ""type"": ""Value"",
                    ""id"": ""e0cc1202-725f-488e-a26c-470960b22ce3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseMovement"",
                    ""type"": ""Value"",
                    ""id"": ""5194c87a-777b-43d0-b086-7ac052e56343"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""0d7680cc-788f-4940-b80e-aa5af3fa61d0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ShiftButton"",
                    ""type"": ""Value"",
                    ""id"": ""52fb1fe8-ad7c-4c39-926a-b23ce1b73244"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ControlButton"",
                    ""type"": ""Button"",
                    ""id"": ""20dd92fc-ac41-45b9-bcd8-481d07da9dd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0f197ef9-8554-4d67-9201-5d1b7f5e81ac"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Place"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23ac95ab-ed79-45df-bfeb-19088a0dc43b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMoveMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5cb88b02-436c-4821-8bc0-6928d6742ce9"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30c3d37c-703e-4d28-8b16-0f34c2ee9ac3"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0cf11f6-c98d-4653-b5ab-abd05c488f09"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2acc65ad-3457-442f-83d6-8ed23c4586e4"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6609ee33-cea2-4324-bffd-ea7377e60aae"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShiftButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fabe0604-ca58-48bf-849b-168dbb6d2556"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ControlButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        m_Movement_Right = m_Movement.FindAction("Right", throwIfNotFound: true);
        m_Movement_Left = m_Movement.FindAction("Left", throwIfNotFound: true);
        m_Movement_Up = m_Movement.FindAction("Up", throwIfNotFound: true);
        m_Movement_Down = m_Movement.FindAction("Down", throwIfNotFound: true);
        // Editor
        m_Editor = asset.FindActionMap("Editor", throwIfNotFound: true);
        m_Editor_Place = m_Editor.FindAction("Place", throwIfNotFound: true);
        m_Editor_CameraMoveMode = m_Editor.FindAction("CameraMoveMode", throwIfNotFound: true);
        m_Editor_MouseMovement = m_Editor.FindAction("MouseMovement", throwIfNotFound: true);
        m_Editor_Scroll = m_Editor.FindAction("Scroll", throwIfNotFound: true);
        m_Editor_ShiftButton = m_Editor.FindAction("ShiftButton", throwIfNotFound: true);
        m_Editor_ControlButton = m_Editor.FindAction("ControlButton", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Movement
    private readonly InputActionMap m_Movement;
    private List<IMovementActions> m_MovementActionsCallbackInterfaces = new List<IMovementActions>();
    private readonly InputAction m_Movement_Move;
    private readonly InputAction m_Movement_Right;
    private readonly InputAction m_Movement_Left;
    private readonly InputAction m_Movement_Up;
    private readonly InputAction m_Movement_Down;
    public struct MovementActions
    {
        private @PlayerInput m_Wrapper;
        public MovementActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputAction @Right => m_Wrapper.m_Movement_Right;
        public InputAction @Left => m_Wrapper.m_Movement_Left;
        public InputAction @Up => m_Wrapper.m_Movement_Up;
        public InputAction @Down => m_Wrapper.m_Movement_Down;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void AddCallbacks(IMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Right.started += instance.OnRight;
            @Right.performed += instance.OnRight;
            @Right.canceled += instance.OnRight;
            @Left.started += instance.OnLeft;
            @Left.performed += instance.OnLeft;
            @Left.canceled += instance.OnLeft;
            @Up.started += instance.OnUp;
            @Up.performed += instance.OnUp;
            @Up.canceled += instance.OnUp;
            @Down.started += instance.OnDown;
            @Down.performed += instance.OnDown;
            @Down.canceled += instance.OnDown;
        }

        private void UnregisterCallbacks(IMovementActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Right.started -= instance.OnRight;
            @Right.performed -= instance.OnRight;
            @Right.canceled -= instance.OnRight;
            @Left.started -= instance.OnLeft;
            @Left.performed -= instance.OnLeft;
            @Left.canceled -= instance.OnLeft;
            @Up.started -= instance.OnUp;
            @Up.performed -= instance.OnUp;
            @Up.canceled -= instance.OnUp;
            @Down.started -= instance.OnDown;
            @Down.performed -= instance.OnDown;
            @Down.canceled -= instance.OnDown;
        }

        public void RemoveCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_MovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Editor
    private readonly InputActionMap m_Editor;
    private List<IEditorActions> m_EditorActionsCallbackInterfaces = new List<IEditorActions>();
    private readonly InputAction m_Editor_Place;
    private readonly InputAction m_Editor_CameraMoveMode;
    private readonly InputAction m_Editor_MouseMovement;
    private readonly InputAction m_Editor_Scroll;
    private readonly InputAction m_Editor_ShiftButton;
    private readonly InputAction m_Editor_ControlButton;
    public struct EditorActions
    {
        private @PlayerInput m_Wrapper;
        public EditorActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Place => m_Wrapper.m_Editor_Place;
        public InputAction @CameraMoveMode => m_Wrapper.m_Editor_CameraMoveMode;
        public InputAction @MouseMovement => m_Wrapper.m_Editor_MouseMovement;
        public InputAction @Scroll => m_Wrapper.m_Editor_Scroll;
        public InputAction @ShiftButton => m_Wrapper.m_Editor_ShiftButton;
        public InputAction @ControlButton => m_Wrapper.m_Editor_ControlButton;
        public InputActionMap Get() { return m_Wrapper.m_Editor; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EditorActions set) { return set.Get(); }
        public void AddCallbacks(IEditorActions instance)
        {
            if (instance == null || m_Wrapper.m_EditorActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_EditorActionsCallbackInterfaces.Add(instance);
            @Place.started += instance.OnPlace;
            @Place.performed += instance.OnPlace;
            @Place.canceled += instance.OnPlace;
            @CameraMoveMode.started += instance.OnCameraMoveMode;
            @CameraMoveMode.performed += instance.OnCameraMoveMode;
            @CameraMoveMode.canceled += instance.OnCameraMoveMode;
            @MouseMovement.started += instance.OnMouseMovement;
            @MouseMovement.performed += instance.OnMouseMovement;
            @MouseMovement.canceled += instance.OnMouseMovement;
            @Scroll.started += instance.OnScroll;
            @Scroll.performed += instance.OnScroll;
            @Scroll.canceled += instance.OnScroll;
            @ShiftButton.started += instance.OnShiftButton;
            @ShiftButton.performed += instance.OnShiftButton;
            @ShiftButton.canceled += instance.OnShiftButton;
            @ControlButton.started += instance.OnControlButton;
            @ControlButton.performed += instance.OnControlButton;
            @ControlButton.canceled += instance.OnControlButton;
        }

        private void UnregisterCallbacks(IEditorActions instance)
        {
            @Place.started -= instance.OnPlace;
            @Place.performed -= instance.OnPlace;
            @Place.canceled -= instance.OnPlace;
            @CameraMoveMode.started -= instance.OnCameraMoveMode;
            @CameraMoveMode.performed -= instance.OnCameraMoveMode;
            @CameraMoveMode.canceled -= instance.OnCameraMoveMode;
            @MouseMovement.started -= instance.OnMouseMovement;
            @MouseMovement.performed -= instance.OnMouseMovement;
            @MouseMovement.canceled -= instance.OnMouseMovement;
            @Scroll.started -= instance.OnScroll;
            @Scroll.performed -= instance.OnScroll;
            @Scroll.canceled -= instance.OnScroll;
            @ShiftButton.started -= instance.OnShiftButton;
            @ShiftButton.performed -= instance.OnShiftButton;
            @ShiftButton.canceled -= instance.OnShiftButton;
            @ControlButton.started -= instance.OnControlButton;
            @ControlButton.performed -= instance.OnControlButton;
            @ControlButton.canceled -= instance.OnControlButton;
        }

        public void RemoveCallbacks(IEditorActions instance)
        {
            if (m_Wrapper.m_EditorActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IEditorActions instance)
        {
            foreach (var item in m_Wrapper.m_EditorActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_EditorActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public EditorActions @Editor => new EditorActions(this);
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnUp(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
    }
    public interface IEditorActions
    {
        void OnPlace(InputAction.CallbackContext context);
        void OnCameraMoveMode(InputAction.CallbackContext context);
        void OnMouseMovement(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnShiftButton(InputAction.CallbackContext context);
        void OnControlButton(InputAction.CallbackContext context);
    }
}
