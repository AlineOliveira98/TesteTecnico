using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public bool isAlive;
    [SerializeField] private float totalPlayerHealth;
    [SerializeField] private float currentPlayerHealth;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    

    [Header("Referencias do Player")]
    public Image fillHealth;
    public SpriteRenderer spriteShip;
    public GameObject bulletPrefab;
    public Transform pointerShootFront;
    public Transform pointerShootLatDir;
    public Transform pointerShootLatDir1;
    public Transform pointerShootLatDir2;
    public Transform pointerShootLatEsq;
    public Transform pointerShootLatEsq1;
    public Transform pointerShootLatEsq2;

    [Header("Estados de Sprites")]
    public Ship navioInfo;
    GamePlayManager gamePlayManager;

    void Start()
    {
        gamePlayManager = FindObjectOfType<GamePlayManager>();
        currentPlayerHealth = totalPlayerHealth;
        fillHealth.fillAmount = currentPlayerHealth;
        
    }

    void Update()
    {
        if(gamePlayManager.gameOver){
            return;
        }

        SetMovement();

        if(Input.GetKeyDown(KeyCode.Space)){ //Meio
            Shoot(pointerShootFront);
        }

        if(Input.GetKeyDown(KeyCode.Mouse2)){ //Meio
            Shoot(pointerShootFront);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)){ //Esquerda
            Shoot(pointerShootLatEsq);
            Shoot(pointerShootLatEsq1);
            Shoot(pointerShootLatEsq2);
        }

        if(Input.GetKeyDown(KeyCode.Mouse1)){ //Direita
            Shoot(pointerShootLatDir);
            Shoot(pointerShootLatDir1);
            Shoot(pointerShootLatDir2);
        }
    }

    void SetMovement(){
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        transform.Translate(movementDirection * speed * inputMagnitude * Time.deltaTime, Space.World);

        if (movementDirection != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void TakeDamagePlayer(float damage){
        currentPlayerHealth -= damage;
        fillHealth.fillAmount = currentPlayerHealth / totalPlayerHealth;
        SetStatePlayer(currentPlayerHealth);

        if(currentPlayerHealth <= 0){
            gamePlayManager.GerarExplosao(transform);
            Destroy(gameObject, 0.35f);
            isAlive = false;
            gamePlayManager.gameOver = true;
        }
    }

    void Shoot(Transform pointer){
        GameObject bullet = Instantiate(bulletPrefab, pointer.position, pointer.rotation);
        bullet.GetComponent<Bullet>().damageBullet = navioInfo.shipDamage;
    }

    void SetStatePlayer(float health){

        if(health < 100 && health >= 75){
            navioInfo.spriteRenderer.sprite = navioInfo.estado1;
            navioInfo.fire1.gameObject.SetActive(false);
            navioInfo.fire2.gameObject.SetActive(false);

        }else if(health < 75 && health >= 50){
            navioInfo.spriteRenderer.sprite = navioInfo.estado2;
            navioInfo.fire1.gameObject.SetActive(true);

        }else if(health < 50 && health >= 0){
            navioInfo.spriteRenderer.sprite = navioInfo.estado3;
            navioInfo.fire2.gameObject.SetActive(true);

        }else if(health <= 0){
            navioInfo.spriteRenderer.sprite = navioInfo.estado4;
            navioInfo.fire1.gameObject.SetActive(false);
            navioInfo.fire2.gameObject.SetActive(false);
        }
        
    }
}
