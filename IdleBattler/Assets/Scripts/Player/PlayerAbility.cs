using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] private AbilityConfiguration m_AbilityConfig;

    private Timer _cooldownTimer;

    private AudioSource _audio;

    private Button _useButton;
    private Player _player;
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    private void Start()
    {
        _useButton = GetComponent<Button>();
        _audio = GetComponent<AudioSource>();

        _useButton.onClick.AddListener(UseAbility);

        _cooldownTimer = new Timer(m_AbilityConfig.Cooldown);
    }

    private void OnDestroy()
    {
        _useButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        _cooldownTimer.RemoveTime(Time.deltaTime);

        if (_cooldownTimer.IsFinished)
        {
            _useButton.interactable = true;
        }
    }

    private void UseAbility()
    {
        _player.DoSpecialAttack(m_AbilityConfig);

        _audio.clip = m_AbilityConfig.AudioClip;

        _audio.Play();

        _useButton.interactable = false;
        _cooldownTimer.RestartTimer();
    }
}