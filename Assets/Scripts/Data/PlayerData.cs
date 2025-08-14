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

    // ����� ���� ������ ������
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
        // ������� ���������� ������ 5 ��������� �����
        //int requiredMice = snakeLevel * 5;
        //if (miceEaten >= requiredMice)
        //{
        //    snakeLevel++;
        //}
    }

    public string GetStats()
    {
        return $"���: {playerName}\n" +
               $"�������: {snakeLevel}\n" +
               $"������� �����: {miceEaten}\n" +
               $"�������� �������: {successfulExits}\n" +
               $"��������� �������: {failedExits}\n" +
               $"��������� ����: {lastPlayTime:g}";
    }
}