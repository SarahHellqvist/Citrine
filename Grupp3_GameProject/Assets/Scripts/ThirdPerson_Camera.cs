using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson_Camera : MonoBehaviour
{
    public LayerMask collisionMask;
    public float mouseSensitivity = 1f;
    public Vector3 cameraOffset = new Vector3(0f, 4f, -10f);

    private GameObject player;
    new private SphereCollider collider;
    private float rotationX;
    private float rotationY;
    private float minimumX = -15;
    private float maximumX = 60f;
    private Vector3 cameraTargetPos;

    void Awake()
    {

        player = transform.parent.gameObject;
        collider = transform.gameObject.GetComponent<SphereCollider>();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        rotationX -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        rotationY += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX); //Limiterar rotationen runt x-axeln

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0); //S�tter rotationen till v�rderna ovan, som p�verkas av musens r�relse.
    }

    private void LateUpdate()
    {
        CollisionCheck();
    }

    private void CollisionCheck() //Kollar om det �r n�gonting mellan kamerans �nskade position och spelaren.
    {
        Vector3 offset = transform.rotation * cameraOffset; //G�r s� att kameran roterar runt spelaren ist�ller f�r sig sj�lv.
        cameraTargetPos = offset /*+ player.transform.position*/; //Avg�r vart kameran vill vara, l�gger till spelarens position f�r att utg� fr�n spelaren.

        bool hit = Physics.SphereCast 
            (
            player.transform.position,
            collider.radius,
            offset.normalized, //Riktnigen fr�n spelaren som raycasten ska skickas ut
            out RaycastHit hitInfo,
            cameraTargetPos.magnitude + collider.radius, //L�ngden p� raycasten, adderar collider.radius f�r att beskriva hur l�ngt ifr�n kollisionen kameran ska placeras.
            collisionMask
            );

        if (hit)
        {
            //Debug.DrawLine(player.transform.position, hitInfo.point, Color.red);
            transform.position = Vector3.Lerp(player.transform.position, hitInfo.point, 1f - (collider.radius / hitInfo.distance)); //s�tt kamerans position till raycastens tr�ffpunkt - radiusen p� kollidern f�r att undvika att kameran hamnar f�r n�ra/inuti objektet.
        }
        else
        {
            transform.position = player.transform.position + cameraTargetPos; //S�tt kamerans position till den �nskade positionen
        }
        //Debug.DrawLine(player.transform.position, cameraTargetPos, Color.green);
    }
}
