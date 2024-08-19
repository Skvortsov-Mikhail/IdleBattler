using UnityEngine;
using Zenject;

public class UI_EnemyHP : MonoBehaviour
{
    [SerializeField] private UI_FloatingText m_FloatingTextPrefab;

    [Header("Text Animation Parameters")]
    [SerializeField] private float m_AnimationDuration;
    [SerializeField] private float m_TextScaleFactor;
    [SerializeField] private float m_TextLiftingDistance;
    [SerializeField] private float m_TextOpacityValue;

    [SerializeField] private Color m_RegularDamageTextColor;
    [SerializeField] private Color m_AbilityDamageTextColor;

    private Enemy _enemy;

    private DiContainer _diContainer;
    [Inject]
    public void Construct(DiContainer container)
    {
        _diContainer = container;
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();

        _enemy.EnemyDamaged += OnEnemyDamaged;
    }

    private void OnDestroy()
    {
        _enemy.EnemyDamaged -= OnEnemyDamaged;
    }

    private void OnEnemyDamaged(float damage, bool isAbility)
    {
        var ui = _diContainer.InstantiatePrefab(m_FloatingTextPrefab);
        ui.transform.position = transform.position;

        string text = "-" + damage.ToString();

        var floatingText = ui.GetComponent<UI_FloatingText>();

        Color textColor = isAbility == false ? m_RegularDamageTextColor : m_AbilityDamageTextColor;

        floatingText.InitFloatingText(text, textColor, true, m_AnimationDuration, m_TextScaleFactor, m_TextLiftingDistance, m_TextOpacityValue);
    }
}