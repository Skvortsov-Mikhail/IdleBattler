using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class ProjectilesPool : MonoBehaviour
{
    [SerializeField] private Projectile m_ProjectilePrefab;

    private ObjectPool<Projectile> _pool;
    public ObjectPool<Projectile> Pool => _pool;

    private DiContainer _diContainer;

    [Inject]
    public void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    private void Start()
    {
        _pool = new ObjectPool<Projectile>(createFunc: () => _diContainer.InstantiatePrefab(m_ProjectilePrefab, transform).GetComponent<Projectile>(),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100);
    }
}