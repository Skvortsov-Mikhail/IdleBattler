using DG.Tweening;
using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour
{
    [SerializeField] private int m_Value;
    [SerializeField] private float m_StaticTime;
    [Header("Animation")]
    [SerializeField] private float m_MoveDuration;
    [SerializeField] private float m_ShakeDuration;
    [SerializeField] private float m_ShakeStrength = 1;


    private Vector2 _lastPos;
    private Vector3[] _path;
    private Vector3 _endScale;

    private Timer _staticTimer;

    private CoinManager _coinManager;
    [Inject]
    public void Construct(CoinManager coinManager)
    {
        _coinManager = coinManager;
    }

    private void Start()
    {
        _staticTimer = new Timer(m_StaticTime);

        _lastPos = _coinManager.TargetIcon.position;
        _endScale = _coinManager.TargetIcon.localScale;
    }

    private void Update()
    {
        if (_staticTimer != null)
        {
            _staticTimer.RemoveTime(Time.deltaTime);

            if (_staticTimer.IsFinished)
            {
                ShowAnimation();

                _staticTimer = null;
            }
        }
    }

    private void ShowAnimation()
    {
        int rnd = Random.Range(0, _coinManager.MiddlePointsForAnimation.Length);

        Vector2 middlePos = _coinManager.MiddlePointsForAnimation[rnd].position;

        _path = new Vector3[] { transform.position, middlePos, _lastPos };

        transform.DOPath(_path, m_MoveDuration, PathType.CatmullRom).SetEase(Ease.InSine);

        transform.DOScale(_endScale, m_MoveDuration)
            .OnComplete(() =>
            {
                _coinManager.AddCoins(m_Value);
                transform.DOShakeScale(m_ShakeDuration, strength: m_ShakeStrength)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            });
    }
}