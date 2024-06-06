using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    private float _startingHealth = 100f;
    [SerializeField]
    public Slider _slider;
    [SerializeField]
    public Image _fillImage;                      

    private Color _fullHealthColor = Color.green;
    private Color _zeroHealthColor = Color.red;

    [SerializeField]
    private ParticleSystem _tankExplosion;
    
    
    private AudioSource _explosionAudio;          
    private float _currentHealth;  
    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = value;
            SetHealthUI();

            if (value <= 0) OnDeath();
        }
    }
    private bool _isDead;            


    private void Awake()
    {
        _explosionAudio = _tankExplosion.gameObject.GetComponent<AudioSource>();

        _tankExplosion.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        CurrentHealth = _startingHealth;
        _isDead = false;
    }
    

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
    }


    private void SetHealthUI()
    {
        _slider.value = _currentHealth;
        _fillImage.color = Color.Lerp(_zeroHealthColor, _fullHealthColor, _currentHealth / _startingHealth);
    }


    private void OnDeath()
    {
        if (_isDead) return;

        _tankExplosion.gameObject.SetActive(true);
        _tankExplosion.Play();
        _explosionAudio.Play();

        _isDead = true;
        gameObject.SetActive(false);
    }
}