using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class UI_PlayerHP : MonoBehaviour
{
    [SerializeField] private Image m_FilledImage;
    [SerializeField] private TMP_Text m_HPText;

    private Player _player;
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    private void Start()
    {
        _player.HPUpdated += OnHPUpdated;

        OnHPUpdated(_player.MaxHP);
    }

    private void OnDestroy()
    {
        _player.HPUpdated -= OnHPUpdated;
    }

    private void OnHPUpdated(float currentHP)
    {
        m_FilledImage.fillAmount = currentHP / _player.MaxHP;

        m_HPText.text = $"HP {currentHP}/{_player.MaxHP}";
    }
}