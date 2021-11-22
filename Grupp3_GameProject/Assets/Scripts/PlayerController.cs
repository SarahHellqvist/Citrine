using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Author Sarah Hellqvist
//Author Filip Hallberg
//Secondary Author: Molly Röle
public class PlayerController : MonoBehaviour, IKillable
{
    [Header("LayerMasks")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private LayerMask gasCollisionMask;
    [SerializeField] private LayerMask groundCollisionMask;

    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField, Min(1f)] private float maxSpeed;
    [SerializeField, Tooltip("Needs to be higher than maxSpeed!")] private float sprintSpeed;
    [SerializeField, Min(1f)] private float gravity;
    [SerializeField, Min(1f)] private float fallAcceleration;
    [SerializeField, Min(1f)] private float jumpHeight;

    [Header("Physics")]
    [SerializeField, Range(0.2f, 1f)] private float staticFrictionCoefficient;
    [SerializeField, Range(0.1f, 1f)] private float airResistance;
    [SerializeField] private float skinWidth;
    [SerializeField] private float groundCheckDistance;
    [SerializeField, Range(0, 1)] private float maxSlopeAngle;

    [Header("Glide")]
    [SerializeField] private float glideFallSpeed;

    [Header("ObjectReferences")]
    [SerializeField] new private GameObject camera;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject mouth;

    [Header("Debug")]
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 respawnPoint;

    [Header("Audio")]
    [SerializeField] private AudioClip[] audioClips;
    //0 - Glide
    //1 - Jump
    //2 - Walk
    //3 - Die

    [Header("Particles")]
    [SerializeField] private ParticleSystem[] particleSystems;
    //0 - Glide
    //1 - Die
    [SerializeField] private GameObject glideParticlesGO;

    [Header("Shooting Pitch")]
    [SerializeField] private float minShootPitch;
    [SerializeField] private float maxShootPitch;

    [SerializeField] private Animator animator;
    //Components
    private Shooter shooter;
    private Health health;
    new private CapsuleCollider collider;
    
    //CapsuleCollider fields
    private Vector3 point1;
    private Vector3 point2;

    //Movement fields
    private Vector3 gravityForce; //Vektor f�r gravitation
    private Vector3 jumpForce; //Vektor f�r hoppr�relse
    private bool isGliding = false;
    private string horizontal = "Horizontal";
    private string vertical = "Vertical";
    private float cachedMaxSpeed;

    //Groundcheck Hitinfo
    private RaycastHit groundHit;

    //Collisioncheck Hitinfo
    private RaycastHit normalHit;
    private bool isHit;
    private Collider[] collisions;
    private Vector3 separationVector;

    //Timer
    private float timerValue = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = transform.position;
    }

    void Awake()
    {
        if(camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        cachedMaxSpeed = maxSpeed;
        collider = GetComponent<CapsuleCollider>();
        shooter = GetComponent<Shooter>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        ApplyGravity();
        Jump();
        Glide();
        DetectAndPreventCollision();
         UpdateVelocity();
        UpdatePosition();
        Fire();
        animator.SetBool("Grounded", OnGround(groundCollisionMask));
        animator.SetBool("isGliding", isGliding);
        if (velocity == Vector3.zero)
        {
            animator.SetBool("Walking", false);
        }
    }

    //Player Input and movement
    private void Move()
    {
        Vector3 input = Vector3.right * Input.GetAxisRaw(horizontal) + Vector3.forward * Input.GetAxisRaw(vertical); //Tar in knapptrycken f�r att best�mma vilket h�ll vi ska r�ra oss �t
        input = camera.transform.rotation * input; //Vi r�r oss kameran tittar

        if (OnGround(groundCollisionMask))
        {
            input = Vector3.ProjectOnPlane(input, Vector3.Lerp(Vector3.up, groundHit.normal, maxSlopeAngle)).normalized; //G�r s� att vi enbart r�r oss l�ngsmed planet vi st�r p�.
        }
        else
        {
            input = Vector3.ProjectOnPlane(input, Vector3.up).normalized; //F�r att vi ska kunna r�ra oss n�r vi inte st�r p� ett plan.
        }

        if (input.magnitude > float.Epsilon) //Tar reda p� om anv�ndaren gett n�gon input genom att titta p� magnituden av vektorn.
        {
            Sprint();
            Accelerate(input); //S�tt accelerationen

            if (OnGround(groundCollisionMask))
            {
                //SOUND
                EventCallbacks.EventHelper.CreateSoundEvent(gameObject, audioClips[2]);
            }
        }
        else
        {
            Decelerate(); //Saktar ner ifall vi inte ger n�gon input
        }
    }

    private void Sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift) && OnGround(groundCollisionMask))
        {
            maxSpeed = sprintSpeed;    
        }
        else
        {
            maxSpeed = cachedMaxSpeed;
        }
    }

    //Rotates Player transform to match Camera yaw
    private void Rotate()
    {
        Quaternion cameraRotation = Quaternion.Euler(0f, camera.transform.eulerAngles.y, 0f);
        transform.rotation = cameraRotation;
    }

    //Gravity calc
    private void ApplyGravity()
    {
        gravityForce = (velocity.y < 0) ? Vector3.down * gravity * (fallAcceleration) * Time.deltaTime : Vector3.down * gravity * Time.deltaTime; //Ber�knar gravitationen. Vector3.down, ger oss vektorn (0, -1, 0). N�r vi vill g�ra saker �ver tid, m�ste vi multiplicera det med framens deltatid.
        velocity += gravityForce;
    }

    private void Jump()
    {
        jumpForce = Vector3.up * jumpHeight;
        if (Input.GetKeyDown(KeyCode.Space) && OnGround(groundCollisionMask)) //Om space trycks ned och vi st�r p� marken...
        {
            velocity += jumpForce; //... hoppar vi

            //Play Jump Sound
            EventCallbacks.EventHelper.CreateSoundEvent(gameObject, audioClips[1]);
        }
    }

    //Acceleration calc
    private void Accelerate(Vector3 input)
    {
        animator.SetBool("Walking", true);
        Vector3 newVelocity = velocity + (input * acceleration * Time.deltaTime); //R�knar ut vad nya �nskade hastigheten kommer bli efter accelaration
        Vector3 horizontalVelocity = velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            newVelocity.x = horizontalVelocity.x;
            newVelocity.z = horizontalVelocity.z;
        }
        velocity = newVelocity;
    }

    //Deceleration calc
    private void Decelerate()
    {
        if (OnGround(groundCollisionMask))
        {
            if(deceleration > Mathf.Abs(velocity.x))
            {
                velocity.x = 0;
            }
            else if(deceleration > Mathf.Abs(velocity.z))
            {
                velocity.z = 0;
            }
            else
            {
                Vector3 projection = new Vector3(velocity.x, 0.0f).normalized;
                velocity -= projection * deceleration * Time.deltaTime;
            }
        }
    }

    //Glide ability
    private void Glide()
    {
        if (!OnGround(collisionMask))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (velocity.y < 0)
                {
                    if(!isGliding)
                    {
                        gravity /= glideFallSpeed;

                        //Play Glide Sound
                        EventCallbacks.EventHelper.CreateSoundEvent(gameObject, audioClips[0]);

                        //Fire Glide Particles
                        //EventCallbacks.EventHelper.CreateParticleEvent(gameObject, particleSystems[0]);
                        glideParticlesGO.SetActive(true);
                    }
                    isGliding = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space) && isGliding)
            {
                CancelGlide();
            }
        }
        else if(isGliding)
        {
            CancelGlide();
        }
    }

    private void CancelGlide()
    {
        isGliding = false;
        gravity *= glideFallSpeed;
        EventCallbacks.EventHelper.CreateSoundEvent(gameObject, audioClips[0], true);
        glideParticlesGO.SetActive(false);
    }

    //Shooting ability
    private void Fire()
    {
        RotateMouth();
        if (Input.GetKeyDown(KeyCode.Mouse1) && GameController.GameControllerInstance.AmmoCount > 0)
        {
            shooter.Fire(bullet, mouth.transform.position, mouth.transform.rotation.eulerAngles);
        }
    }

    private void RotateMouth()
    {
        float rotationX = camera.transform.rotation.eulerAngles.x;
        rotationX = (rotationX > 180) ? rotationX - 360 : rotationX;
        float rotationX2 = Mathf.Clamp(rotationX, minShootPitch, maxShootPitch); //Limiterar rotationen runt x-axeln
        mouth.transform.rotation = Quaternion.Euler(rotationX2, mouth.transform.rotation.eulerAngles.y, 0);
    }

    //Collision handler
    private void DetectAndPreventCollision()
    {
        
        CalculateCapsuleColliderPoints();
        collisions = Physics.OverlapCapsule(
                        point1,
                        point2,
                        collider.radius,
                        collisionMask
                        );
        if (collisions.Length > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                if (velocity.magnitude * Time.deltaTime < 0.001) //F�rsiktighets�tg�rd f�r att undvika O�ndlig looping.
                {
                    velocity = Vector3.zero;
                }
                foreach (Collider col in collisions)
                {

                    if (col == collider) //Kollar om col �r likamed spelarens egna kollider, f�r att den inte ska r�kna med sig sj�lv
                    {
                        continue; //Hoppar till n�sta varv i loopen ifall vi tittar p� v�r egna collider.
                    }

                    Quaternion otherColRotation = col.gameObject.transform.rotation; //Den andra Rotationen
                    Vector3 otherColPosition = col.gameObject.transform.position; //Den andra Positionen

                    Physics.ComputePenetration( //Kalkylerar den minsta vektorn som kr�vs f�r att separera den f�rsta kollidern fr�n den andra.
                    collider,
                    transform.position,
                    transform.rotation,
                    col,
                    otherColPosition,
                    otherColRotation,
                    out Vector3 direction,
                    out float distance
                    );

                    separationVector = direction.normalized * (distance + skinWidth); //Anger vilket h�ll vi ska r�ra oss fr�n att komam bort fr�n kolisionen.
                    transform.position += separationVector; //Flyttar karakt�ren bort fr�n objektet
                    Vector3 normalForce = HelpClass.CalculateNormalForce3D(velocity, separationVector.normalized); //S�tter normalkraften f�r kollisionen
                    velocity += normalForce.normalized;
                    ApplyResistingForce(normalForce, Vector3.zero);
                }
            }
        }
    }

    //Second collision handler
    private void UpdateVelocity()
    {
        do
        {
            if (velocity.magnitude * Time.deltaTime < 0.001)
            {
                velocity = Vector3.zero;
                break;
            }
            CalculateCapsuleColliderPoints();
            isHit = Physics.CapsuleCast(
                    point1,
                    point2,
                    collider.radius,
                    velocity.normalized * Time.deltaTime,
                    out RaycastHit hit,
                    velocity.magnitude * Time.deltaTime + skinWidth,
                    collisionMask
                   );
            Physics.CapsuleCast(
            point1,
            point2,
            collider.radius,
            -hit.normal * Time.deltaTime,
            out normalHit,
            velocity.magnitude * Time.deltaTime + skinWidth,
            collisionMask
            );
            Vector3 normalForce = HelpClass.CalculateNormalForce3D(velocity, hit.normal);

            float distanceToColliderNeg = skinWidth / Vector3.Dot(velocity.normalized, hit.normal);
            float allowedMovementDistance = hit.distance + distanceToColliderNeg;
            if (allowedMovementDistance > velocity.magnitude * Time.deltaTime)
            {
                break;
            }
            if (allowedMovementDistance > 0.0f)
            {
                transform.position += -normalHit.normal * (normalHit.distance - skinWidth);
            }
            velocity += normalForce;
            ApplyResistingForce(normalForce, Vector3.zero); //L�gg till luftmost�nd och friktion som vanligt �ven om ingen kollision har skett
        } while (isHit);
    }

    //Applying velocity to position
    private void UpdatePosition()
    {
        transform.position += velocity * Time.deltaTime; //F�rflyttning?
    }
    private bool OnGround(LayerMask groundLayerMask)
    {
        CalculateCapsuleColliderPoints();
        //returnerar true on objektet nuddar marken (enligt groundLayerMask), annars returneras false
        return Physics.CapsuleCast(
               point1,
               point2,
               collider.radius,
               Vector3.down * Time.deltaTime,
               out groundHit,
               groundCheckDistance * Time.deltaTime + skinWidth,
               groundLayerMask
               );
    }

    //Friction and drag calc
    private void ApplyResistingForce(Vector3 normalForce, Vector3 additionalForce)
    {
        float kineticFrictionCoefficient = staticFrictionCoefficient * 0.7f; //S�tter dynamiska friktionen till att vara 70% av den statiska.
        float velocityDiff = velocity.magnitude - additionalForce.magnitude;

        if (velocityDiff < normalForce.magnitude * staticFrictionCoefficient) //Om magnituden av v�r hastighet �r mindre �n den statiska friktionen(normalkraften multiplicerat med den statiska friktionskoefficienten)...
        {
            velocity = additionalForce; //...s�tter vi v�r hastighet till noll?
        }
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient; //...Annars adderar vi den motsatta riktningen av hastigheten multiplicerat med den dynamiska friktionen(normalkraften multiplicerat med den dynamiska friktionskoefficienten).

            velocity *= Mathf.Pow(airResistance, Time.deltaTime); //Ber�knar luftmotst�ndet. Eftersom vi inte vill ha en exponentiell funktion m�ste vi h�ja upp v�rt tal med delta tid.
        }
    }

    //DeathHandler
    public void Die()
    {
        EventCallbacks.EventHelper.CreateSoundEvent(gameObject, audioClips[3]);
        EventCallbacks.EventHelper.CreateParticleEvent(gameObject, particleSystems[1]);

        StartCoroutine(ResetAfterDeath());
    }

    private IEnumerator ResetAfterDeath()
    {
        yield return new WaitForSeconds(timerValue);

        transform.position = respawnPoint;
        health.InitializeHealth();
        velocity = Vector3.zero;
    }

    //Help function for collider calculations
    private void CalculateCapsuleColliderPoints()
    {
        //R�knar ut origo f�r de tv� sf�rerna som kapseln best�r av
        float distanceToPoints = collider.height / 2 - collider.radius;
        point1 = collider.transform.position + collider.center + collider.transform.forward * distanceToPoints; // Ber�knar ena sf�rens punkt genom att ber�kna distansen fr�n centrumpunkten till respektive punkt. H�jden delat p� tv� �r distansen fr�n centrumpunkten till botten, respektive toppen. Vi kan sen subtrahera med radien f�r att f� distansen till respektive punkt.
        point2 = collider.transform.position + collider.center - collider.transform.forward * distanceToPoints; //Ber�knar andra sf�rens punkt p� samam s�tt som ovan.
    }

    public void SetRespawnPoint(Vector3 respawnLocation)
    {
        respawnPoint = respawnLocation;
    }

    public Vector3 GetVelocity() { return velocity; }
}


