using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8.0f;

 
    // Update is called once per frame
    void Update()
    {

        LaserVelocity();

        DestroyLaser();

    }

    void LaserVelocity()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
    }


    public void DestroyLaser()
    {
        if (transform.position.y >= 8)
        {
            if(transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
