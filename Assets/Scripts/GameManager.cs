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
    private int _score;
    public static GameManager _instance;

    [SerializeField] private GameObject musicPlayer;
    private MusicPlayer _musicPlayer;

    private Dictionary<string, string> _gameCurrentStatus = new()
    {
        { "PlayerState", ""},
        { "InCombat", ""},
        { "GameStatus", ""}
    };
    private Dictionary<string, string> _gamePastStatus = new()
    {
        { "PlayerState", ""},
        { "InCombat", ""},
        { "GameStatus", ""}
    };
    private float timer = 0f;


    private void Awake()
    {
    _instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameCurrentStatus["PlayerState"] = "Healthy";
        _gameCurrentStatus["InCombat"] = "False";
        _gameCurrentStatus["GameStatus"] = "Playing";
        _musicPlayer = musicPlayer.GetComponent<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        ActualizarEstadoCombate();
        if (_gameCurrentStatus != _gamePastStatus)
        {
            _gamePastStatus = new Dictionary<string, string>(_gameCurrentStatus);

            Debug.Log(_gamePastStatus["PlayerState"]);
            Debug.Log(_gamePastStatus["InCombat"]);
            Debug.Log(_gamePastStatus["GameStatus"]);

            UpdateGameState();
        }

    }

    private void UpdateGameState()
    {
        switch (_gameCurrentStatus["GameStatus"])
        {
            case "Playing":
                if (_gameCurrentStatus["InCombat"] == "True")
                {
                    _musicPlayer.PlayCombatMusic();
                    break;
                }

                switch (_gameCurrentStatus["PlayerState"])
                {
                    case "Healthy":
                        _musicPlayer.PlayNormalMusic();
                        break;

                    case "LowHealth":
                        _musicPlayer.PlayLowHealthSnapshot();
                        break;
                }
                break;

            case "Victory":
                _musicPlayer.PlayVictoryMusic();
                break;

            case "GameOver":
                _musicPlayer.PlayGameOverMusic();
                break;
        }
    }

    public void UpdateScore(int _addToScore)
    {
        _score += _addToScore;
    }

    public void GetUpdateGameStatus(string var, string value)
    {
        if (_gameCurrentStatus.ContainsKey(var)) _gameCurrentStatus[var] = value;
        else Debug.Log("GameManager.getUpdate() " + var + " is the wrong key");
    }

    private void ActualizarEstadoCombate()
    {
        EnemyController[] enemigos =
            FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        bool hayCombate = false;

        foreach (EnemyController enemigo in enemigos)
        {
            if (enemigo.EstaEnCombate)
            {
                hayCombate = true;
                break;
            }
        }

        GetUpdateGameStatus("InCombat", hayCombate ? "True" : "False");
    }

}
