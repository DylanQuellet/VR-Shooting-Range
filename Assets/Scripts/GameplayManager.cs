using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    private int currentScore = 0;

    // Exemple d’état global : nombre de cibles actives
    public int maxTargets = 2;
    public int currentTarget = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void RegisterTargetSpawn()
    {
        currentTarget++;
    }
    public void RegisterTargetDespawn()
    {
        currentTarget--;
    }
}
