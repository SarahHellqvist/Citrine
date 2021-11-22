using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthBar healthBar;

    [Header("Fields")]
    [SerializeField] private float currentHealth;
    [SerializeField, Min(1f)]private float maxHealth = 100;

    [Header("Audio")]
    [SerializeField] private AudioClip takingDamageSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem takingDamageParticles;

    //References
    private IKillable killableGameObject;
    //Flags
    private bool isAlive;

    void Start()
    {
        InitializeHealth();
        killableGameObject = GetComponent<IKillable>();
    }
    
    public void InitializeHealth()
    {
        currentHealth = maxHealth;
        isAlive = true;
        UpdateHealthBar();
    }    
    public void IncreaseHealth(float health)
    {
        this.currentHealth += health;
        UpdateHealthBar();

        if (this.currentHealth > maxHealth)
        {
            this.currentHealth = maxHealth;
        }
    }

    public void DecreaseHealth(float damage)
    {
        if(isAlive){
            this.currentHealth -= damage;

            //Play taking damage sound
            EventCallbacks.EventHelper.CreateSoundEvent(gameObject, takingDamageSound);
            //Fire taking damage particles
            EventCallbacks.EventHelper.CreateParticleEvent(gameObject, takingDamageParticles);

            UpdateHealthBar();
            if(currentHealth < 1)
            {
                isAlive = false;
                //EventCallbacks.EventHelper.CreateDebugEvent(gameObject, gameObject + " health is: " + currentHealth);
                killableGameObject.Die();
            }
        }
        
    }

    public float GetCurrentHealth(){return this.currentHealth;}
    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        if(healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
}
