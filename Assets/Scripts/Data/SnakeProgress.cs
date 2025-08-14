using UnityEngine;

public class SnakeProgress : MonoBehaviour
{
    // ���������� true, ���� ������� ���������
    public bool TryLevelUp(PlayerData data)
    {
        if (data == null) return false;

        int requiredMice = data.snakeLevel * 5;

        // �������: ���� ����������� ����� ��������� ����� � ����� ������� �����
        if (data.miceEaten >= requiredMice && data.hasExitedThisRound)
        {
            data.snakeLevel++;
            // ������� ���� ������ �� ����� (��������� ������� ����� ������ �����)
            data.hasExitedThisRound = false;
            return true;
        }
        return false;
    }
}
