using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterController : RectObstacle
{
    private static readonly int MovementDirection = Animator.StringToHash("Movement");
    private static readonly int Fire = Animator.StringToHash("Fire");

    [SerializeField] private Animator m_characterAnimator;
    [SerializeField] private CharacterSettings m_characterSettings;
    [SerializeField] private RectObstacle m_wall1;
    [SerializeField] private RectObstacle m_wall2;
    [SerializeField] private bool m_firstPlayer;
    [SerializeField] private Weapon m_equippedWeapon;
    
    private int m_movementDirection;
    private bool m_inputActive = true;
    private float m_minX;
    private float m_maxX;
    private MonoSystem m_monoSystem;
    private LevelManagementSystem m_levelManagementSystem;
    private InputSystem m_inputSystem;

    private void Start()
    {
        m_monoSystem = SystemLocator.Get<MonoSystem>();
        m_levelManagementSystem = SystemLocator.Get<LevelManagementSystem>();
        m_inputSystem = SystemLocator.Get<InputSystem>();
        
        (m_firstPlayer ? 
            m_inputSystem.Player1InputEvent :
            m_inputSystem.Player2InputEvent)
            .AddListener(OnInput);
        
        m_levelManagementSystem.MainMenuLoaded.AddListener(() =>
        {
            gameObject.SetActive(false);
            m_inputActive = false;
        });

        if (m_firstPlayer)
        {
            m_levelManagementSystem.LevelLoaded.AddListener(() =>
            {
                gameObject.SetActive(true);
                m_inputActive = true;
            });
        }
        else
        {
            m_inputSystem.OnPlayer2Spawn.AddListener(() =>
            {
                Activate();
                m_inputActive = true;
            });
        }

        m_monoSystem.OnUpdate.AddListener(HandleMovement);

        List<float> m_wall1Points = new List<float>() {m_wall1.RectData.point1.x, m_wall1.RectData.point2.x, m_wall1.RectData.point3.x, m_wall1.RectData.point4.x};
        List<float> m_wall2Points = new List<float>() {m_wall2.RectData.point1.x, m_wall2.RectData.point2.x, m_wall2.RectData.point3.x, m_wall2.RectData.point4.x};

        if (m_wall1.RectData.rectCenter.x < m_wall2.RectData.rectCenter.x)
        {
            m_minX = m_wall1Points.Max();
            m_maxX = m_wall2Points.Min();
        }
        else
        {
            m_minX = m_wall2Points.Max();
            m_maxX = m_wall1Points.Min();
        }
        
        m_equippedWeapon.Init();
        
        gameObject.SetActive(false);
        m_inputActive = false;
    }

    private void OnInput(InputChangedArgs inputArgs)
    {
        if (m_inputActive && inputArgs.FirePressed)
        {
            HandleFire();
        }
        
        m_characterAnimator.SetInteger(MovementDirection, inputArgs.InputDirection);
        m_movementDirection = inputArgs.InputDirection;
    }

    private void HandleMovement(float delta)
    {
        if(!m_inputActive || m_monoSystem.TimeScale < 0.1f) { return; }
        //An animation curve could be used here to allow for greater control over character movement
        //Using acceleration instead of simple position change could also improve game feel a lot
        Vector3 pos = transform.position + m_movementDirection * Vector3.right * m_characterSettings.MovementSpeed * delta;
        pos.x = Mathf.Clamp(pos.x, m_minX + (m_rectData.point1.x - m_rectData.point4.x) / 2, m_maxX - (m_rectData.point1.x - m_rectData.point4.x) / 2);
        transform.position = pos;
    }

    //Could also be managed with callbacks
    private async void HandleFire()
    {
        if (m_equippedWeapon.Fire(transform.position))
        {
            m_inputActive = false;
            m_characterAnimator.SetTrigger(Fire);
            await Task.Delay((int) (m_equippedWeapon.FireTime * 1000)); //Task.Delay uses milliseconds
            m_inputActive = true; //Could also be managed with callbacks
        }
    }
}
