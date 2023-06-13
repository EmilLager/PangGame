using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputSystem", menuName = "ScriptableObjects/Settings/InputSystem")]
public class InputSystem : BaseScriptableObjectSystem
{
    [SerializeField] private InputSettings m_inputSettings;

    [SerializeField] private InputEvent m_player1InputEvent;
    [SerializeField] private InputEvent m_player2InputEvent;
    
    public InputEvent Player1InputEvent => m_player1InputEvent;
    public InputEvent Player2InputEvent => m_player2InputEvent;

    public UnityEvent OnPausePressed { get; } = new UnityEvent();

    public UnityEvent OnPlayer2Spawn { get; } = new UnityEvent();

    private InputChangedArgs m_player1LastInput;
    private InputChangedArgs m_player2LastInput;

    private bool m_leftPressed;
    private bool m_rightPressed;
    private bool m_shootPressed;

    public override void Init() { }

    public override void StartSystem()
    {
        SystemLocator.Get<MonoSystem>().OnUpdate.AddListener(ControlledUpdate);
    }

    private void ControlledUpdate(float delta)
    {
        InputChangedArgs player1Input;
        InputChangedArgs player2Input;
#if UNITY_STANDALONE
        player1Input = CreateKeyboardInputData(m_inputSettings.Player1Controls);
        player2Input = CreateKeyboardInputData(m_inputSettings.Player2Controls);
#elif UNITY_ANDROID
        player1Input = CreateTouchInputData();
        player2Input = default;
#endif

        if (player1Input != m_player1LastInput)
        {
            m_player1LastInput = player1Input;
            m_player1InputEvent?.Invoke(player1Input);
        }
        
        if (player2Input != m_player2LastInput)
        {
            m_player2LastInput = player2Input;
            m_player2InputEvent?.Invoke(player2Input);
        }

        if (Input.GetKeyDown(m_inputSettings.PauseKey))
        {
            OnPausePressed?.Invoke();
        }
        
        if (Input.GetKeyDown(m_inputSettings.AddPlayerKey))
        {
            OnPlayer2Spawn?.Invoke();
        }
    }

    public void PressedLeft(bool value)
    {
        m_leftPressed = value;
    }
    
    public void PressedRight(bool value)
    {
        m_rightPressed = value;
    }
    
    public void PressedShoot()
    {
        m_shootPressed = true;
    }

    private InputChangedArgs CreateTouchInputData()
    {
        int direction = 0;
        bool fire = false;
        if (m_leftPressed)
        {
            direction--;
        }
        
        if (m_rightPressed)
        {
            direction++;
        }
        
        fire = m_shootPressed;
        if (fire)
        {
            m_shootPressed = false;
        }

        return new InputChangedArgs
        {
            InputDirection = direction,
            FirePressed = fire,
        };
    }

    private InputChangedArgs CreateKeyboardInputData(PlayerControls controlScheme)
    {
        int direction = 0;
        bool fire = false;
        if (Input.GetKey(controlScheme.MoveLeftKey))
        {
            direction--;
        }
        
        if (Input.GetKey(controlScheme.MoveRightKey))
        {
            direction++;
        }
        
        fire = Input.GetKeyDown(controlScheme.FireKey);
        
        return new InputChangedArgs
        {
            InputDirection = direction,
            FirePressed = fire,
        };
    }
}

[Serializable]
public class InputEvent : UnityEvent<InputChangedArgs> { }

public struct InputChangedArgs
{
    public int InputDirection;
    public bool FirePressed;

    public static bool operator ==(InputChangedArgs obj1, InputChangedArgs obj2) => obj1.InputDirection == obj2.InputDirection && obj1.FirePressed == obj2.FirePressed;
    
    public static bool operator !=(InputChangedArgs obj1, InputChangedArgs obj2) => !(obj1 == obj2);
}
