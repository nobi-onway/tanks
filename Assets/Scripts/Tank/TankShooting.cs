using UnityEngine;
using UnityEngine.UI;

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
    }
    

    private void Update()
    {
        ListenInput();
        if (_currentLaunchForce >= _maxLaunchForce && !_fired) Fire();
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


    private void Fire()
    {
        _fired = true;

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