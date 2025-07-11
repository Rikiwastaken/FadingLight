//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""gameplay"",
            ""id"": ""cd06d251-7210-410f-ac5a-c8393199d849"",
            ""actions"": [
                {
                    ""name"": ""attack"",
                    ""type"": ""Button"",
                    ""id"": ""485e2196-6a27-4fe3-a9da-744381a3b499"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""jump"",
                    ""type"": ""Button"",
                    ""id"": ""c6786b4f-9eb3-4cd8-8860-65635bc192b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightShoulder"",
                    ""type"": ""Button"",
                    ""id"": ""57ccf2ce-7b56-4f56-91b5-0598ca879a7e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftShoulder"",
                    ""type"": ""Button"",
                    ""id"": ""6fb85922-59f8-47ed-881b-e36bdddbc49a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""menu"",
                    ""type"": ""Button"",
                    ""id"": ""458931c4-6396-4945-97c4-c38b0e893ae3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""down"",
                    ""type"": ""Button"",
                    ""id"": ""968db628-9bee-42cf-b020-7cb456054317"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""up"",
                    ""type"": ""Button"",
                    ""id"": ""952fefc7-4063-4193-b28b-c543e939982e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""moveleft"",
                    ""type"": ""Button"",
                    ""id"": ""cadfdfee-d132-4138-8add-30548612a806"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""moveright"",
                    ""type"": ""Button"",
                    ""id"": ""869e1407-443f-4185-8df4-437f72db3325"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""dodge"",
                    ""type"": ""Button"",
                    ""id"": ""25b8167e-7fbd-4241-8eff-18bbafd3296f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""1c443ff7-8c1e-4c20-af5c-70816b2a33f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""crossup"",
                    ""type"": ""Button"",
                    ""id"": ""94e77269-df9c-4ece-bc89-5d6d9dfcd7c7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""crossdown"",
                    ""type"": ""Button"",
                    ""id"": ""bc591208-bf52-4ca5-b71c-89921eba752c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""crossleft"",
                    ""type"": ""Button"",
                    ""id"": ""369f7fe6-e072-425e-918b-e85304d2830d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""crossright"",
                    ""type"": ""Button"",
                    ""id"": ""bc4e94f9-a1b9-46d7-8f80-a045f6e3d3bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""NorthButton"",
                    ""type"": ""Button"",
                    ""id"": ""ddd93b15-7987-4d1d-b2ab-ad5cf00aebfb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftTrigger"",
                    ""type"": ""Button"",
                    ""id"": ""ad68531f-7442-44c1-902c-50014a40de66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightTrigger"",
                    ""type"": ""Button"",
                    ""id"": ""b8a71df7-a711-4b86-989f-1e2c81447de6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9481b057-cce6-4c45-9641-8e1f568dd4a3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5265d56-b334-4d43-b384-2c20b5295e8d"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c592d03-c0bb-44a0-b4a9-b3611392cbe0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb0ce881-1f03-43ae-8e8e-35b612c4549a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdf841c6-0067-4b07-ae5c-6410864af63b"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ecace223-fc95-432a-bf87-458c0d52e0e2"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eccf714f-20f4-421d-8c58-cedf1ec28476"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca165807-79d3-4dd2-891a-73d0b1e776a7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9dc094c2-f9ce-4643-a5f6-85939354bd80"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c7f5b48-6e1b-408e-8987-6523990413fe"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61905332-6cb5-4c20-bdbc-5a8164015603"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""moveleft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac06e0ef-6f6b-4e41-be9b-fa4d7890f8e9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""moveleft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f5ced3b-0a0e-400a-ad29-d3f53d2b1bec"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""moveright"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c06cf6ef-6fba-4faa-a823-288b4f055504"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""moveright"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69815abd-39e9-4320-9695-7310e401914c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd31b214-7b18-4b36-85c6-368472f87510"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b69ae0b-0298-4396-a9d8-386357a7632d"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7baad2d9-5819-43b6-aa6f-96c3c999796e"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a331d2b1-0d49-49e9-8da4-035de50a33f1"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad1766eb-efce-4f29-9c8f-a503cf190f83"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c069c99-476e-461f-8043-5091028e347e"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdc9c9e0-e690-4c23-b4f4-2585c40abfeb"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91f0fc91-0e7b-4d6e-9f8f-a29975c18c69"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crossup"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdc09cda-4551-4009-9f69-e75b210beddb"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crossup"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b918ffe9-04a2-4d8d-8316-4e82ba25edd6"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crossdown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c0d41b4-e54c-4181-b72b-2d83ae40db74"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crossdown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6267075-e638-4cc5-85b3-03e5bd1fd1c7"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crossleft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7772b108-7993-4034-a241-5ffdaf426f9c"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crossleft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75201b7b-8d35-4d81-a1d0-c291926634d8"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crossright"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7b30146-fe28-43e0-8ae7-a6a0a25f48f2"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crossright"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbda6930-bb20-4f76-9f46-87fdd4b63e07"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NorthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9dc2b4a5-55ec-45de-8f48-9e09f4768b03"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NorthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8506368e-afd7-4c97-baeb-485a074a96bc"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c96e140d-8884-4001-aacb-e98a3304ef02"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14bb92a3-1d91-448c-ad1d-c18f56224a0e"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4005cbc-8e00-4bc7-828c-d3c9979c7c71"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // gameplay
        m_gameplay = asset.FindActionMap("gameplay", throwIfNotFound: true);
        m_gameplay_attack = m_gameplay.FindAction("attack", throwIfNotFound: true);
        m_gameplay_jump = m_gameplay.FindAction("jump", throwIfNotFound: true);
        m_gameplay_RightShoulder = m_gameplay.FindAction("RightShoulder", throwIfNotFound: true);
        m_gameplay_LeftShoulder = m_gameplay.FindAction("LeftShoulder", throwIfNotFound: true);
        m_gameplay_menu = m_gameplay.FindAction("menu", throwIfNotFound: true);
        m_gameplay_down = m_gameplay.FindAction("down", throwIfNotFound: true);
        m_gameplay_up = m_gameplay.FindAction("up", throwIfNotFound: true);
        m_gameplay_moveleft = m_gameplay.FindAction("moveleft", throwIfNotFound: true);
        m_gameplay_moveright = m_gameplay.FindAction("moveright", throwIfNotFound: true);
        m_gameplay_dodge = m_gameplay.FindAction("dodge", throwIfNotFound: true);
        m_gameplay_Inventory = m_gameplay.FindAction("Inventory", throwIfNotFound: true);
        m_gameplay_crossup = m_gameplay.FindAction("crossup", throwIfNotFound: true);
        m_gameplay_crossdown = m_gameplay.FindAction("crossdown", throwIfNotFound: true);
        m_gameplay_crossleft = m_gameplay.FindAction("crossleft", throwIfNotFound: true);
        m_gameplay_crossright = m_gameplay.FindAction("crossright", throwIfNotFound: true);
        m_gameplay_NorthButton = m_gameplay.FindAction("NorthButton", throwIfNotFound: true);
        m_gameplay_LeftTrigger = m_gameplay.FindAction("LeftTrigger", throwIfNotFound: true);
        m_gameplay_RightTrigger = m_gameplay.FindAction("RightTrigger", throwIfNotFound: true);
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

    // gameplay
    private readonly InputActionMap m_gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_gameplay_attack;
    private readonly InputAction m_gameplay_jump;
    private readonly InputAction m_gameplay_RightShoulder;
    private readonly InputAction m_gameplay_LeftShoulder;
    private readonly InputAction m_gameplay_menu;
    private readonly InputAction m_gameplay_down;
    private readonly InputAction m_gameplay_up;
    private readonly InputAction m_gameplay_moveleft;
    private readonly InputAction m_gameplay_moveright;
    private readonly InputAction m_gameplay_dodge;
    private readonly InputAction m_gameplay_Inventory;
    private readonly InputAction m_gameplay_crossup;
    private readonly InputAction m_gameplay_crossdown;
    private readonly InputAction m_gameplay_crossleft;
    private readonly InputAction m_gameplay_crossright;
    private readonly InputAction m_gameplay_NorthButton;
    private readonly InputAction m_gameplay_LeftTrigger;
    private readonly InputAction m_gameplay_RightTrigger;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @attack => m_Wrapper.m_gameplay_attack;
        public InputAction @jump => m_Wrapper.m_gameplay_jump;
        public InputAction @RightShoulder => m_Wrapper.m_gameplay_RightShoulder;
        public InputAction @LeftShoulder => m_Wrapper.m_gameplay_LeftShoulder;
        public InputAction @menu => m_Wrapper.m_gameplay_menu;
        public InputAction @down => m_Wrapper.m_gameplay_down;
        public InputAction @up => m_Wrapper.m_gameplay_up;
        public InputAction @moveleft => m_Wrapper.m_gameplay_moveleft;
        public InputAction @moveright => m_Wrapper.m_gameplay_moveright;
        public InputAction @dodge => m_Wrapper.m_gameplay_dodge;
        public InputAction @Inventory => m_Wrapper.m_gameplay_Inventory;
        public InputAction @crossup => m_Wrapper.m_gameplay_crossup;
        public InputAction @crossdown => m_Wrapper.m_gameplay_crossdown;
        public InputAction @crossleft => m_Wrapper.m_gameplay_crossleft;
        public InputAction @crossright => m_Wrapper.m_gameplay_crossright;
        public InputAction @NorthButton => m_Wrapper.m_gameplay_NorthButton;
        public InputAction @LeftTrigger => m_Wrapper.m_gameplay_LeftTrigger;
        public InputAction @RightTrigger => m_Wrapper.m_gameplay_RightTrigger;
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @attack.started += instance.OnAttack;
            @attack.performed += instance.OnAttack;
            @attack.canceled += instance.OnAttack;
            @jump.started += instance.OnJump;
            @jump.performed += instance.OnJump;
            @jump.canceled += instance.OnJump;
            @RightShoulder.started += instance.OnRightShoulder;
            @RightShoulder.performed += instance.OnRightShoulder;
            @RightShoulder.canceled += instance.OnRightShoulder;
            @LeftShoulder.started += instance.OnLeftShoulder;
            @LeftShoulder.performed += instance.OnLeftShoulder;
            @LeftShoulder.canceled += instance.OnLeftShoulder;
            @menu.started += instance.OnMenu;
            @menu.performed += instance.OnMenu;
            @menu.canceled += instance.OnMenu;
            @down.started += instance.OnDown;
            @down.performed += instance.OnDown;
            @down.canceled += instance.OnDown;
            @up.started += instance.OnUp;
            @up.performed += instance.OnUp;
            @up.canceled += instance.OnUp;
            @moveleft.started += instance.OnMoveleft;
            @moveleft.performed += instance.OnMoveleft;
            @moveleft.canceled += instance.OnMoveleft;
            @moveright.started += instance.OnMoveright;
            @moveright.performed += instance.OnMoveright;
            @moveright.canceled += instance.OnMoveright;
            @dodge.started += instance.OnDodge;
            @dodge.performed += instance.OnDodge;
            @dodge.canceled += instance.OnDodge;
            @Inventory.started += instance.OnInventory;
            @Inventory.performed += instance.OnInventory;
            @Inventory.canceled += instance.OnInventory;
            @crossup.started += instance.OnCrossup;
            @crossup.performed += instance.OnCrossup;
            @crossup.canceled += instance.OnCrossup;
            @crossdown.started += instance.OnCrossdown;
            @crossdown.performed += instance.OnCrossdown;
            @crossdown.canceled += instance.OnCrossdown;
            @crossleft.started += instance.OnCrossleft;
            @crossleft.performed += instance.OnCrossleft;
            @crossleft.canceled += instance.OnCrossleft;
            @crossright.started += instance.OnCrossright;
            @crossright.performed += instance.OnCrossright;
            @crossright.canceled += instance.OnCrossright;
            @NorthButton.started += instance.OnNorthButton;
            @NorthButton.performed += instance.OnNorthButton;
            @NorthButton.canceled += instance.OnNorthButton;
            @LeftTrigger.started += instance.OnLeftTrigger;
            @LeftTrigger.performed += instance.OnLeftTrigger;
            @LeftTrigger.canceled += instance.OnLeftTrigger;
            @RightTrigger.started += instance.OnRightTrigger;
            @RightTrigger.performed += instance.OnRightTrigger;
            @RightTrigger.canceled += instance.OnRightTrigger;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @attack.started -= instance.OnAttack;
            @attack.performed -= instance.OnAttack;
            @attack.canceled -= instance.OnAttack;
            @jump.started -= instance.OnJump;
            @jump.performed -= instance.OnJump;
            @jump.canceled -= instance.OnJump;
            @RightShoulder.started -= instance.OnRightShoulder;
            @RightShoulder.performed -= instance.OnRightShoulder;
            @RightShoulder.canceled -= instance.OnRightShoulder;
            @LeftShoulder.started -= instance.OnLeftShoulder;
            @LeftShoulder.performed -= instance.OnLeftShoulder;
            @LeftShoulder.canceled -= instance.OnLeftShoulder;
            @menu.started -= instance.OnMenu;
            @menu.performed -= instance.OnMenu;
            @menu.canceled -= instance.OnMenu;
            @down.started -= instance.OnDown;
            @down.performed -= instance.OnDown;
            @down.canceled -= instance.OnDown;
            @up.started -= instance.OnUp;
            @up.performed -= instance.OnUp;
            @up.canceled -= instance.OnUp;
            @moveleft.started -= instance.OnMoveleft;
            @moveleft.performed -= instance.OnMoveleft;
            @moveleft.canceled -= instance.OnMoveleft;
            @moveright.started -= instance.OnMoveright;
            @moveright.performed -= instance.OnMoveright;
            @moveright.canceled -= instance.OnMoveright;
            @dodge.started -= instance.OnDodge;
            @dodge.performed -= instance.OnDodge;
            @dodge.canceled -= instance.OnDodge;
            @Inventory.started -= instance.OnInventory;
            @Inventory.performed -= instance.OnInventory;
            @Inventory.canceled -= instance.OnInventory;
            @crossup.started -= instance.OnCrossup;
            @crossup.performed -= instance.OnCrossup;
            @crossup.canceled -= instance.OnCrossup;
            @crossdown.started -= instance.OnCrossdown;
            @crossdown.performed -= instance.OnCrossdown;
            @crossdown.canceled -= instance.OnCrossdown;
            @crossleft.started -= instance.OnCrossleft;
            @crossleft.performed -= instance.OnCrossleft;
            @crossleft.canceled -= instance.OnCrossleft;
            @crossright.started -= instance.OnCrossright;
            @crossright.performed -= instance.OnCrossright;
            @crossright.canceled -= instance.OnCrossright;
            @NorthButton.started -= instance.OnNorthButton;
            @NorthButton.performed -= instance.OnNorthButton;
            @NorthButton.canceled -= instance.OnNorthButton;
            @LeftTrigger.started -= instance.OnLeftTrigger;
            @LeftTrigger.performed -= instance.OnLeftTrigger;
            @LeftTrigger.canceled -= instance.OnLeftTrigger;
            @RightTrigger.started -= instance.OnRightTrigger;
            @RightTrigger.performed -= instance.OnRightTrigger;
            @RightTrigger.canceled -= instance.OnRightTrigger;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRightShoulder(InputAction.CallbackContext context);
        void OnLeftShoulder(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnUp(InputAction.CallbackContext context);
        void OnMoveleft(InputAction.CallbackContext context);
        void OnMoveright(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnCrossup(InputAction.CallbackContext context);
        void OnCrossdown(InputAction.CallbackContext context);
        void OnCrossleft(InputAction.CallbackContext context);
        void OnCrossright(InputAction.CallbackContext context);
        void OnNorthButton(InputAction.CallbackContext context);
        void OnLeftTrigger(InputAction.CallbackContext context);
        void OnRightTrigger(InputAction.CallbackContext context);
    }
}
