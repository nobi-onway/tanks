using System;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    private enum TankState
    {
        Idling,
        Driving,
    }

    public int m_PlayerNumber = 1;
    [SerializeField]
    private AudioClip _engineIdling;
    [SerializeField]
    private AudioClip _engineDriving;      


    [SerializeField]
    private AudioSource _movementAudio;    
    private float _speed = 12f;            
    private float _turnSpeed = 180f;       
    private float _pitchRange = 0.2f;
    
    private string _movementAxisName;     
    private string _turnAxisName;         
    private Rigidbody _rb;         
    private float _movementInputValue;    
    private float _turnInputValue;        
    private float _originalPitch;

    private TankState _currentState;
    private TankState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            OnStateChange?.Invoke(value);
        }
    }
    private event Action<TankState> OnStateChange; 


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        _rb.isKinematic = false;
        _movementInputValue = 0f;
        _turnInputValue = 0f;
    }


    private void OnDisable ()
    {
        _rb.isKinematic = true;
    }


    private void Start()
    {
        _movementAxisName = "Vertical" + m_PlayerNumber;
        _turnAxisName = "Horizontal" + m_PlayerNumber;

        _originalPitch = _movementAudio.pitch;

        OnStateChange += (state) =>
        {
            if (state == TankState.Idling) EngineAudio(_engineIdling);
            if (state == TankState.Driving) EngineAudio(_engineDriving);
        };
    }


    private void Update()
    {
        _movementInputValue = Input.GetAxis(_movementAxisName);
        _turnInputValue = Input.GetAxis(_turnAxisName);

        if (Mathf.Abs(_movementInputValue) < 0.1f && Mathf.Abs(_turnInputValue) < 0.1f)
        {
            CurrentState = TankState.Idling;
        }
        else
        {
            CurrentState = TankState.Driving;
        }
    }


    private void EngineAudio(AudioClip audioClip)
    {
        if (_movementAudio.clip == audioClip) return;

        _movementAudio.clip = audioClip;
        _movementAudio.pitch = UnityEngine.Random.Range(_originalPitch - _pitchRange, _originalPitch + _pitchRange);
        _movementAudio.Play();
    }


    private void FixedUpdate()
    {
        Move();
        Turn();
    }


    private void Move()
    {
        Vector3 direction = transform.forward * _movementInputValue * Time.deltaTime;
        _rb.MovePosition(transform.position + direction * _speed);
    }


    private void Turn()
    {
        float turnRange = _turnInputValue * _turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turnRange, 0);
        _rb.MoveRotation(transform.rotation * turnRotation);
    }
}