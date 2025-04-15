using Bas.Pennings.DevTools;

public class ScoreManager : AbstractSingleton<ScoreManager>
{
    public delegate void ScoreManagerDelegate(int data);
    public ScoreManagerDelegate OnScoreChanged;
    public ScoreManagerDelegate OnDeathCountChanged;

    private int score = 0;
    private int deathCount = 0;

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    public void RemoveScore(int amount)
    {
        score -= amount;
        OnScoreChanged?.Invoke(score);
    }

    public void RegisterDeath()
    {
        deathCount++;
        OnDeathCountChanged?.Invoke(deathCount);
    }
}
