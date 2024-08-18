using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject m_LosePanel;

    private Player _player;
    private EnemiesSpawner _spawner;
    [Inject]
    public void Construct(Player player, EnemiesSpawner spawner)
    {
        _player = player;
        _spawner = spawner;
    }

    private void Start()
    {
        m_LosePanel.SetActive(false);

        _player.PlayerDied += OnPlayerDied;
    }

    private void OnDestroy()
    {
        _player.PlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        m_LosePanel.SetActive(true);

        foreach(var destructible in FindObjectsOfType<Destructible>())
        {
            destructible.enabled = false;
        }

        _spawner.enabled = false;
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}