using System;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    private const string PLAYERS_KEY = "SavedPlayers";
    private const string CURRENT_PLAYER_KEY = "CurrentPlayer";

    public static void SavePlayer(PlayerData player)
    {
        // ��������� ������ ���� �������
        List<PlayerData> players = LoadAllPlayers();
        Debug.Log($"���������� ������: {player.playerName}");

        // ���������, ���� �� ����� � ����� ������
        int existingIndex = players.FindIndex(p => p.playerName == player.playerName);
        if (existingIndex >= 0)
        {
            // ��������� ������ ������������� ������
            players[existingIndex] = player;
        }
        else
        {
            // ��������� ������ ������
            players.Add(player);
        }

        // ��������� ���������� ������
        SaveAllPlayers(players);

        // ��������� �������� ������
        SetCurrentPlayer(player);

        // ��������� ���������� ������
        //string json = JsonUtility.ToJson(new PlayerListWrapper(players));
        //PlayerPrefs.SetString(PLAYERS_KEY, json);
        //PlayerPrefs.Save();
    }

    public static List<PlayerData> LoadAllPlayers()
    {
        if (PlayerPrefs.HasKey(PLAYERS_KEY))
        {
            string json = PlayerPrefs.GetString(PLAYERS_KEY);

            // �������� �� �������
            if (string.IsNullOrEmpty(json)) return new List<PlayerData>();

            try
            {
                PlayerListWrapper wrapper = JsonUtility.FromJson<PlayerListWrapper>(json);
                return wrapper?.players ?? new List<PlayerData>();
            }
            catch
            {
                return new List<PlayerData>();
            }
        }
        return new List<PlayerData>();
    }

    public static PlayerData GetCurrentPlayer()
    {
        if (PlayerPrefs.HasKey(CURRENT_PLAYER_KEY))
        {
            string json = PlayerPrefs.GetString(CURRENT_PLAYER_KEY);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        return null;
    }

    public static void SetCurrentPlayer(PlayerData player)
    {
        string json = JsonUtility.ToJson(player);
        PlayerPrefs.SetString(CURRENT_PLAYER_KEY, json);
        PlayerPrefs.Save();
    }

    public static void DeletePlayer(string playerName)
    {
        List<PlayerData> players = LoadAllPlayers();

        int removedCount = players.RemoveAll(p => p.playerName == playerName);

        if (removedCount > 0)
        {
            SaveAllPlayers(players);
            Debug.Log($"����� {playerName} �����. ��������: {players.Count}");

            // ���� ������� �������� ������ - ������� � ������� ������
            PlayerData current = GetCurrentPlayer();
            if (current != null && current.playerName == playerName)
            {
                PlayerPrefs.DeleteKey("CurrentPlayer");
            }
        }
        else
        {
            Debug.LogWarning($"����� {playerName} �� ������ ��� ��������");
        }

        //players.RemoveAll(p => p.playerName == playerName);
        //SaveAllPlayers(players);
    }

    private static void SaveAllPlayers(List<PlayerData> players)
    {
        string json = JsonUtility.ToJson(new PlayerListWrapper(players));
        PlayerPrefs.SetString(PLAYERS_KEY, json);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class PlayerListWrapper
    {
        public List<PlayerData> players;
        public PlayerListWrapper(List<PlayerData> players) => this.players = players;
    }

    
    //public static void ResetRoundProgress()
    //{
    //    var current = GetCurrentPlayer();
    //    if (current != null)
    //    {
    //        current.miceEaten = 0;
    //        // �� ������� snakeLevel, successfulExits, failedExits, lastPlayTime � ������ ����
    //        SavePlayer(current);
    //    }
    //}

    //public static void ResetAllProgress()
    //{
    //    var current = GetCurrentPlayer();
    //    if (current != null)
    //    {
    //        current.miceEaten = 0;
    //        current.snakeLevel = 1;
    //        current.successfulExits = 0;
    //        current.failedExits = 0;
    //        current.lastPlayTime = DateTime.Now;
    //        // ���� ���� ���� hasExitedThisRound, �������� ���
    //        // current.hasExitedThisRound = false;
    //        SavePlayer(current);
    //    }
    //}
}