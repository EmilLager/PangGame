using UnityEngine;

[CreateAssetMenu(fileName = "DefaultRopeWeapon", menuName = "ScriptableObjects/Settings/DefaultRopeWeapon")]
public class DefaultRopeWeapon : Weapon
{
    [SerializeField] private float m_projectileMaxHeight;
    [SerializeField] private float m_projectileWidth;
    [SerializeField] private float m_projectileSpeed;
    
    private bool m_projectileActive = false;
    private RectObstacle m_activeProjectile;

    private ProjectilePool m_projectilePool;
    private MonoSystem m_monoSystem;
    private float m_currentProjectileHeight;
    private Vector3 m_firePosition;

    public override void Init()
    {
        m_monoSystem = SystemLocator.Get<MonoSystem>();
        m_projectilePool = SystemLocator.Get<ProjectilePool>();
        m_monoSystem.OnUpdate.AddListener(ManageProjectile);
    }

    public override bool Fire(Vector3 firePosition)
    {
        if (m_activeProjectile != null) { return false; }

        m_firePosition = firePosition - Vector3.up;
        m_activeProjectile = m_projectilePool.GetProjectile();
        m_activeProjectile.transform.position = firePosition;
        m_activeProjectile.SetRectData(new RectData(
            new Vector2(- m_projectileWidth / 2, 0f),
            new Vector2(m_projectileWidth / 2, 0f),
            new Vector2(m_projectileWidth / 2, 0.1f),
            new Vector2(- m_projectileWidth / 2, 0.1f)
        ));
        m_currentProjectileHeight = 0.1f;
        
        m_activeProjectile.OnHitObstacle.AddListener(DisableProjectile);
        m_activeProjectile.Activate();
        
        m_projectileActive = true;
        return true;
    }

    private void DisableProjectile()
    {
        m_activeProjectile.OnHitObstacle.RemoveListener(DisableProjectile);
        m_projectilePool.ReturnRect(m_activeProjectile);
        m_projectileActive = false;
        m_activeProjectile = null;
        m_currentProjectileHeight = 0.1f;
    }

    private void ManageProjectile(float delta)
    {
        
        if (m_activeProjectile == null) { return; }
        
        m_currentProjectileHeight += delta * m_projectileSpeed;

        m_activeProjectile.transform.position = m_firePosition + Vector3.up * (m_currentProjectileHeight / 2f);

        m_activeProjectile.SetRectData(new RectData(
            new Vector2(- m_projectileWidth / 2, - m_currentProjectileHeight / 2),
            new Vector2(m_projectileWidth / 2, - m_currentProjectileHeight / 2),
            new Vector2(m_projectileWidth / 2, m_currentProjectileHeight / 2),
            new Vector2(- m_projectileWidth / 2, m_currentProjectileHeight / 2)
        ));

        //m_activeProjectile.SetRectData(rect);

        if (m_currentProjectileHeight > m_projectileMaxHeight)
        {
            DisableProjectile();
        }
    }
}
