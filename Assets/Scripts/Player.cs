using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // _ at the begginning of a variable means that its a private variable
    // [SerializeField] makes the field act as a private, so it cant be used by other scripts but, it is still able to be updated in the Unity editor
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shield;
    Animator player;
    private IEnumerator _tripleShotCoroutine;
    private IEnumerator _speedBoostCoroutine;

    
  
    private int _score = 0;
    internal UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _tripleShotCoroutine = TripleShotPowerDownRoutine();
        _speedBoostCoroutine = SpeedPowerDownRoutine();

        player = GetComponent<Animator>();
        //take the cirrent positition = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("The Canvas is Null");
        }
    }

  
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        PlayerBoundries();
        //Time.time is the ammount of time the game has been running for in seconds
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
           FireLaser();
        }
    }

    void FireLaser()
    {
        //if i press space then
        //spawn game object

        //_can fire is -1 so it will always be true first time round as -1 is less than 0 at the beginning
        //can fire is then updated to the current time plus the _fireRate this is makes it so that the -canFire variable is now greater than the current time, making the if statement false
        //the if statement will only be true once the Time.time catches up with the fix _canFire variable in once the if statement is true again the _canFire variable will be updated again to be greater than current time
        //this creates the cooldown effect

        Vector3 laserSpawnOffset = new Vector3(0, 0.8f, 0);
        
        _canFire = Time.time + _fireRate;
        //Quaternion.identity = the default rotation

        if (_isTripleShotActive)
        {
            Vector3 tripleLaserSpawnOffset = new Vector3(-3.2f, 0.8f, 0);
            Instantiate(_tripleLaserPrefab, transform.position + tripleLaserSpawnOffset, Quaternion.identity); ;
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + laserSpawnOffset, Quaternion.identity);
        }
    }


    public void TripleShotActive()
    {
        Debug.Log("triple");
        _isTripleShotActive = true;
        if (_tripleShotCoroutine != null)
        {
            StopCoroutine(_tripleShotCoroutine);
            
        }

        _tripleShotCoroutine = TripleShotPowerDownRoutine();
        StartCoroutine(_tripleShotCoroutine);
    }

    public void SpeedActive()
    {
        Debug.Log("speed");
        _isSpeedActive = true;
        _speed *= _speedMultiplier;
        if (_speed > 10)
        {
            _speed = 10;
        }
        if (_speedBoostCoroutine != null)
        {
            StopCoroutine(_speedBoostCoroutine);
        }
        _speedBoostCoroutine = SpeedPowerDownRoutine();
        StartCoroutine(_speedBoostCoroutine);
    }


    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
        Debug.Log("shield");
    }


    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
        Debug.Log("stop triple shot");
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
        _speed /= _speedMultiplier;
        Debug.Log("stop speed boost");
    }


    void CalculateMovement()
    {
        //will return +1(D) or -1(A) depending on the button pressed
        float horizontalInput = Input.GetAxis("Horizontal");
        player.SetFloat("velocityX", horizontalInput);
        float verticalInput = Input.GetAxis("Vertical");

        /*
        //Time.deltaTime = real time or per second
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime) = move given speed(_speed) to the given direction(horizontalInput) per second(Time.deltaTime).
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);

        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        //OPTIMISED (one line for both axis)
                //transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        */

        //FURTHER OPTIMISED (using a local variable)
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
    }


    void PlayerBoundries()
    {
        //PLAYER RESTRICTION
        //if player positions in y axis is greater than 0 then 
        // y =  position = 0
        //else if player positions in y axis is greater than -3.8 then 
        // y =  position = 0

        //OPTIMISED TO RESTRICT THE Y AXIS OF PLAYER USING mathf.clamp
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f), 0);

        //PLAYER WRAPPING (once off the screen appear on the other side of the screen)

        //if player on the x is > 11 then
        //x pos = -11
        //else if player on the x axis is less than -11
        //x pos =  11

        if (transform.position.x >= 11.2f)
        {
            transform.position = new Vector3(-11.2f, transform.position.y, transform.position.y);
        }
        else if (transform.position.x <= -11.2f)
        {
            transform.position = new Vector3(11.2f, transform.position.y, transform.position.z);
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shield.SetActive(false);
            _isShieldActive = false;
            return;
        }
        else
        {
            _lives -= 1;
            UIManager ui = UIManager.FindObjectOfType<UIManager>();
            ui.UpdateLives(_lives);
            
            print("lives = " + _lives);
            if(_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
    }

    public void AddScore(int _addedScore) {


        _score += _addedScore;
        _uiManager.ViewScore(_score);
    }


}
