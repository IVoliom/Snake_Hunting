using System;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int snakeLevel;
    public int miceEaten;
    public int successfulExits;
    public int failedExits;
    public DateTime lastPlayTime;

    // Новый флаг раунда выхода
    public bool hasExitedThisRound;

    public PlayerData(string name)
    {
        playerName = name;
        snakeLevel = 1;
        miceEaten = 0;
        successfulExits = 0;
        failedExits = 0;
        lastPlayTime = DateTime.Now;
        hasExitedThisRound = false;
    }

    public void LevelUp()
    {
        // Уровень повышается каждые 5 съеденных мышей
        //int requiredMice = snakeLevel * 5;
        //if (miceEaten >= requiredMice)
        //{
        //    snakeLevel++;
        //}
    }

    public string GetStats()
    {
        return $"Имя: {playerName}\n" +
               $"Уровень: {snakeLevel}\n" +
               $"Съедено мышей: {miceEaten}\n" +
               $"Успешных выходов: {successfulExits}\n" +
               $"Неудачных выходов: {failedExits}\n" +
               $"Последняя игра: {lastPlayTime:g}";
    }
}