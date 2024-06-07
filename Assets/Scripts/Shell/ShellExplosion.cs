using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    [SerializeField]
    private LayerMask _tankMask;
    private ParticleSystem _explosionParticles;       
    private AudioSource _explosionAudio;        
    
    private float _maxDamage = 100f;                  
    private float _explosionForce = 1000f;            
    private float _maxLifeTime = 2f;                  
    private float _explosionRadius = 5f;              


    private void Start()
    {
        _explosionParticles = GetComponentInChildren<ParticleSystem>();
        _explosionAudio = _explosionParticles.GetComponent<AudioSource>();
        Destroy(gameObject, _maxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _tankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            ExplosionOn(colliders[i]);
            TakeDamageOn(colliders[i]);
        }
    }

    private void ExplosionOn(Collider collider)
    {
        Rigidbody targetRb = collider.GetComponent<Rigidbody>();

        if (!targetRb) return;

        targetRb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);

        _explosionParticles.transform.parent = null;
        Destroy(gameObject);

        _explosionParticles.Play();
        _explosionAudio.Play();
        Destroy(_explosionParticles, _explosionParticles.main.duration);
    }

    private void TakeDamageOn(Collider collider)
    {
        TankHealth tankHealth = collider.GetComponent<TankHealth>();

        if (!tankHealth) return;

        float damage = CalculateDamage(collider.transform.position);
        tankHealth.TakeDamage(damage);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        float explosionToTargetDistance = (targetPosition - transform.position).magnitude;

        float relativeDistance = (_explosionRadius - explosionToTargetDistance) / _explosionRadius;

        return Mathf.Max(0, relativeDistance * _maxDamage);
    }
}