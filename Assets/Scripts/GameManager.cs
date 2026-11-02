using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using static PlayerStatus;

public class GameManager : MonoBehaviour
{
    private _gameState _currentGameState;
    private enum _gameState
    {
        Menu,
        Paused,
        Playing,
        LowHealth,
        Victory,
        GameOver
    }
    private int _score;
    public static GameManager _instance;
    [SerializeField] private PlayerStatus _playerStatus;
    [SerializeField] private GameObject _enemies;
    [SerializeField] private GameObject _musicComposer;
    [SerializeField] private GameObject _menu;

    private Dictionary<string, string> _gameCurrentStatus = new()
    {
        { "PlayerState", ""},
        { "GameStatus", ""}
    };
    private float timer = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameCurrentStatus["PlayerState"] = "Healthy";
        _gameCurrentStatus["GameStatus"] = "Menu";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

    }

    private void UpdateGameState()
    {
        int state = 0;

        switch (state) { 
        
        
        } 

    }

    public void UpdateScore(int _addToScore)
    {
        _score += _addToScore;
    }

    public string UpdateComposer()
    {
        switch (_currentGameState)
        {
            case _gameState.Menu:
                return "Menu";
            case _gameState.Paused:
                return "Paused";
            case _gameState.Playing:
                return "Playing";
            case _gameState.LowHealth:
                return "Playing";
            case _gameState.Victory:
                return "Playing";
            case _gameState.GameOver:
                return "GameOver";
            default:
                Debug.Log("UpdateComposer gameState Missmatch");
                break;
        }
        return "MissingState";
    }

    public void getUpdate(string value, string var)
    {
        if (_gameCurrentStatus.ContainsKey(var)) _gameCurrentStatus[var] = value;
        else Debug.Log("GameManager.getUpdate() " + var + " is the wrong key");
    }
}
