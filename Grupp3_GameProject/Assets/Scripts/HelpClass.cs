using UnityEngine;

public class HelpClass : MonoBehaviour
{
 public static Vector2 CalculateNormalForce(Vector2 velocity, Vector2 normal)
    {
        float skalar = Vector2.Dot(velocity, normal); //R�knar ut skal�rprodukten
        Vector2 projection; 
       
        if (skalar > 0) //Kollar om skal�rprodukten �r postitiv
        {
            projection = Vector2.zero * normal; //S�tter vi projektionen till 0.
        }
        else
        {
            projection = skalar * normal; //D� s�tter vi den till skal�rprodukten * normalen
        } 
        return -projection; //�r negativ f�r att den ska vara motsatt kraften som skickas in.
    }

    public static Vector3 CalculateNormalForce3D(Vector3 velocity, Vector3 normal)
    {
        float skalar = Vector3.Dot(velocity, normal); //R�knar ut skal�rprodukten
        Vector3 projection;

        if (skalar > 0) //Kollar om skal�rprodukten �r postitiv
        {
            projection = Vector3.zero;
        }
        else
        {
            projection = skalar * normal;
        }
        return -projection;
    }
}
