using UnityEngine;

public class SnakeProgress : MonoBehaviour
{
    // Возвращает true, если уровень повысился
    public bool TryLevelUp(PlayerData data)
    {
        if (data == null) return false;

        int requiredMice = data.snakeLevel * 5;

        // Условие: есть достаточное число съеденных мышей и игрок успешно вышел
        if (data.miceEaten >= requiredMice && data.hasExitedThisRound)
        {
            data.snakeLevel++;
            // Сбросим флаг выхода на раунд (следующая попытка нужна заново выйти)
            data.hasExitedThisRound = false;
            return true;
        }
        return false;
    }
}
