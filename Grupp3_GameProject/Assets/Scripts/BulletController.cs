using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private float gravity;
    [SerializeField] private float gravityDelayTimer;
    [SerializeField] private float speed;
    [SerializeField, Min(1f)] private float damage;

    [Header("LayerMask")]
    [SerializeField] private LayerMask collisionMask;

    [Header("References")]
    [SerializeField] new private Rigidbody rigidbody;


    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        Move();
        if(rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        rigidbody.AddForce(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetVelocity());
        transform.Rotate(90,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ApplyGravity());
        UpdatePosition();
    }

    //Describes the movement of the bullet
    private void Move()
    {
        velocity += transform.forward * speed;
    }

    private IEnumerator ApplyGravity()
    {
        yield return new WaitForSeconds(gravityDelayTimer);
        velocity += Vector3.down * gravity * Time.deltaTime;
    }

    private void UpdatePosition()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        int enemyLayer = 7;
        int playerLayer = 8;
        
        if(other.isTrigger == false){
            if(other.gameObject.layer == enemyLayer)
            {
                other.gameObject.GetComponent<Health>().DecreaseHealth(damage);           
            }

            if (other.gameObject.layer != playerLayer)
            {
                Destroy(gameObject);
            }
        }
      

    }
}
