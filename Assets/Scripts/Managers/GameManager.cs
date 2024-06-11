using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int _numRoundsToWin = 5;        
    private float _startDelay = 3f;         
    private float _endDelay = 3f;
    [SerializeField]
    private CameraControl _cameraControl;
    [SerializeField]
    private Text _messageText;
    [SerializeField]
    private GameObject _tankPrefab;
    [SerializeField]
    private TankManager[] _tanksManager;           


    private int _roundNumber;              
    private TankManager _roundWinner;
    private TankManager _gameWinner;       


    private void Start()
    {
        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < _tanksManager.Length; i++)
        {
            _tanksManager[i].m_Instance =
                Instantiate(_tankPrefab, _tanksManager[i].m_SpawnPoint.position, _tanksManager[i].m_SpawnPoint.rotation) as GameObject;
            _tanksManager[i].m_PlayerNumber = i + 1;
            _tanksManager[i].Setup();
        }
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[_tanksManager.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = _tanksManager[i].m_Instance.transform;
        }

        _cameraControl._targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (_gameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();
        _cameraControl.SetStartPositionAndSize();

        _roundNumber++;
        _messageText.text = $"ROUND: {_roundNumber}";
        yield return new WaitForSeconds(_startDelay);
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();
        _messageText.text = string.Empty;

        while(!IsOneTankLeft())
        {
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();
        _roundWinner = null;
        _roundWinner = GetRoundWinner();
        if (_roundWinner != null) _roundWinner.m_Wins++;

        _gameWinner = GetGameWinner();
        _messageText.text = EndMessage();
        yield return new WaitForSeconds(_endDelay);
    }


    private bool IsOneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < _tanksManager.Length; i++)
        {
            if (_tanksManager[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < _tanksManager.Length; i++)
        {
            if (_tanksManager[i].m_Instance.activeSelf)
                return _tanksManager[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < _tanksManager.Length; i++)
        {
            if (_tanksManager[i].m_Wins == _numRoundsToWin)
                return _tanksManager[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (_roundWinner != null)
            message = _roundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < _tanksManager.Length; i++)
        {
            message += _tanksManager[i].m_ColoredPlayerText + ": " + _tanksManager[i].m_Wins + " WINS\n";
        }

        if (_gameWinner != null)
            message = _gameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < _tanksManager.Length; i++)
        {
            _tanksManager[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < _tanksManager.Length; i++)
        {
            _tanksManager[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < _tanksManager.Length; i++)
        {
            _tanksManager[i].DisableControl();
        }
    }
}