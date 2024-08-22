using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public enum EShootState { FIRE, START, CHARGE }

public class TankShooting : MonoBehaviour
{
    public int _playerNumber = 1;
    [SerializeField]
    private Rigidbody _shell;
    [SerializeField]
    private Transform _fireTransform;
    [SerializeField]
    private Slider _aimSlider;
    [SerializeField]
    private AudioSource _shootingAudio;
    [SerializeField]
    private AudioClip _chargingClip;
    [SerializeField]
    private AudioClip _fireClip;     
    
    private float _minLaunchForce = 15f; 
    private float _maxLaunchForce = 30f; 
    private float _maxChargeTime = 0.75f;

    private float _launchTime;

    private EShootState _state;
    public EShootState State
    {
        get => _state;
        set
        {
            _state = value;
            OnShootStateChange?.Invoke(value);
        }
    }

    public event Action<EShootState> OnShootStateChange;

    
    private string _fireButton;         
    private float _currentLaunchForce;  
    private float CurrentLaunchForce
    {
        get => _currentLaunchForce;
        set
        {
            _currentLaunchForce = value > _maxLaunchForce ? _maxLaunchForce : value;
            _aimSlider.value = value;
        }
    }
    private float _chargeSpeed;         
    private bool _fired;                


    private void OnEnable()
    {
        CurrentLaunchForce = _minLaunchForce;
        _aimSlider.value = _minLaunchForce;
    }


    private void Start()
    {
        _fireButton = "Fire" + _playerNumber;

        _chargeSpeed = (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;

        OnShootStateChange += (state) =>
        {
            switch(state)
            {
                case EShootState.FIRE:
                    Fire();
                    break;
                case EShootState.START:
                    _fired = false;
                    CurrentLaunchForce = _minLaunchForce;

                    EngineAudio(_chargingClip);
                    break;
                case EShootState.CHARGE:
                    CurrentLaunchForce += _chargeSpeed * Time.deltaTime;
                    break;
            }
        };
    }
    

    private void Update()
    {
        _launchTime += Time.deltaTime;

        ListenInput();
        if (_currentLaunchForce >= _maxLaunchForce && !_fired) State = EShootState.FIRE;
    }

    private void ListenInput()
    {
        if(Input.GetButtonDown(_fireButton))
        {
            _fired = false;
            CurrentLaunchForce = _minLaunchForce;

            EngineAudio(_chargingClip);
        }else if (Input.GetButton(_fireButton) && !_fired)
        {
            CurrentLaunchForce += _chargeSpeed * Time.deltaTime;

        }else if (Input.GetButtonUp(_fireButton) && !_fired)
        {
            Fire();
        }
    }

    public void AIFire(Vector3 target)
    {
        if (_launchTime < 0.5f) return;

        if(_fired)
        {
            _fired = false;
            CurrentLaunchForce = _minLaunchForce;

            EngineAudio(_chargingClip);
        }

        CurrentLaunchForce += _chargeSpeed * Time.deltaTime;

        float angle = MathF.Abs(360 - _fireTransform.eulerAngles.x);
        float distance = Vector3.Distance(_fireTransform.position, target);

        float velocitySquare = (distance) * Physics.gravity.magnitude / Mathf.Sin(2 * angle * Mathf.PI / 180);
        
        if (CurrentLaunchForce - Mathf.Sqrt(velocitySquare) >= 0) Fire();
    }

    public void ResetLauch()
    {
        CurrentLaunchForce = _minLaunchForce;
    }

    private void Fire()
    {
        _fired = true;
        _launchTime = 0.0f;

        Rigidbody shellRb = Instantiate(_shell, _fireTransform.position, _fireTransform.rotation);

        shellRb.velocity = _currentLaunchForce * _fireTransform.forward;

        EngineAudio(_fireClip);

        CurrentLaunchForce = _minLaunchForce;
    }

    private void EngineAudio(AudioClip audioClip)
    {
        _shootingAudio.clip = audioClip;
        _shootingAudio.Play();
    }
}