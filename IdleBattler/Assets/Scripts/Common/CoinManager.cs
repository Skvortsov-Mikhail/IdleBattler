using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public Action<int> CoinsCountChanged;

    [SerializeField] private Coin m_CoinPrefab;
    public Coin CoinPrefab => m_CoinPrefab;

    [SerializeField] private Transform m_TargetIcon;
    public Transform TargetIcon => m_TargetIcon;

    [SerializeField] private Transform[] m_MiddlePointsForAnimation;
    public Transform[] MiddlePointsForAnimation => m_MiddlePointsForAnimation;

    private string filename = "coinsCount.dat";

    private int _coinsCount;
    public int CoinsCount => _coinsCount;

    private void Awake()
    {
        LoadValue();
    }

    public void AddCoins(int count)
    {
        _coinsCount = Mathf.Clamp(_coinsCount + count, 0, int.MaxValue);

        Save();

        CoinsCountChanged?.Invoke(_coinsCount);
    }

    public bool RemoveCoins(int count)
    {
        if (_coinsCount - count < 0) return false;

        _coinsCount -= count;

        Save();

        CoinsCountChanged?.Invoke(_coinsCount);

        return true;
    }

    #region Savings
    private void LoadValue()
    {
        Saver<int>.TryLoad(filename, ref _coinsCount);
    }

    private void Save()
    {
        Saver<int>.Save(filename, _coinsCount);
    }

    public void ResetValue()
    {
        _coinsCount = 0;

        Save();

        CoinsCountChanged?.Invoke(_coinsCount);
    }
    #endregion
}