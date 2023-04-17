using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed =  4;

    Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogError("The player is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyVelocity();
        EnemyBoundries();

    }

    void EnemyBoundries()
    {
        float randomXPos = Random.Range(-9.0f, 9.0f);

        Vector3 newPosition = new Vector3(randomXPos, 6.58f, transform.position.z);

        if (transform.position.y < -6.58)
        {

            transform.position = newPosition;
        }
    }
    void EnemyVelocity()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit: " + collision.transform.name);
        if(collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
                       
            Destroy(this.gameObject);
        }


        else if(collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            
            Destroy(this.gameObject);
            _player.AddScore(10);
        }

        
    }

}
