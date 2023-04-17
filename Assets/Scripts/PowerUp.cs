using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    float _itemVelocity = 3;
  

    private Player _player;
    // Start is called before the first frame update

    
    public enum ID 
    { 
        Triple,
        Speed,
        Shield
    
    }

    [SerializeField]
    ID _powerUpId;

    void Start()
    {
        
        
        _player = FindObjectOfType <Player>();
        if(_player == null)
        {
            Debug.LogError("No Player found");
        }
    }




    private void Update()
    {
        transform.Translate(Vector3.down * _itemVelocity * Time.deltaTime);
        DestroyPowerUp();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (_powerUpId) 
            {
                case ID.Triple:
                    _player.TripleShotActive();
                    break;
                case ID.Speed:
                    _player.SpeedActive();
                    break;
                case ID.Shield:
                    _player.ShieldActive();
                    break;
                default:
                    Debug.Log("Unexpected case");
                    break;
            }

            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void DestroyPowerUp()
    {
        if(transform.position.y < -6.43)
        {
            Destroy(this.gameObject);
        }
    }
    
}
