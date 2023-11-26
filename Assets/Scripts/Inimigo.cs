using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Inimigo : MonoBehaviour
{
    public float totalHealth;
    public float currentHealth;
    public bool isAlive;
    public Image fillHealth;

    [Header("Estados de Sprites")]
    public Ship navioInfo;

    [Header("Configuração de Patrulha")]
    public float speedMovement;
    public int indexPontoRota;
    public int indexRota;
    public GameObject rota;
    Transform[] pontosRota;
    public List<Transform> pontosRotaLista;
    GamePlayManager gamePlayManager;

    [Header("Configuração para seguir player")]
    public float speed;
    public float stoppingDistance;
    public float retreaDistance;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject projectile;
    public Transform pointShoot;
    public Transform target;
    private NavMeshAgent agent;

    [Header("Configuração para atacar player")]
    public float followPlayerRange;
    public float attackRange;
    public bool inRangeFollow;
    public bool inRangeAttack;
    

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        gamePlayManager = FindObjectOfType<GamePlayManager>();
        target = FindObjectOfType<PlayerController>().transform;

        currentHealth = totalHealth;
        fillHealth.fillAmount = currentHealth / totalHealth;
        isAlive = true;

        indexRota = Random.Range(0, gamePlayManager.routes.Count);
        rota = gamePlayManager.routes[indexRota];
        rota.GetComponent<RotaController>().ocupada = true;
        gamePlayManager.routes.Remove(rota);

        pontosRota = rota.GetComponentsInChildren<Transform>();
        pontosRotaLista = new List<Transform>(pontosRota);
        pontosRotaLista.RemoveAt(0);
        transform.position = pontosRotaLista[0].position;

        timeBtwShots = startTimeBtwShots;
    }

    void Update(){
        if(!gamePlayManager.gameOver){
            PatrulhaTeste();
        }
    }

    void PatrulhaTeste(){

        if(pontosRotaLista.Count != 0 && isAlive && target != null){

            //Definição de estados do navio
            if (Vector2.Distance(transform.position, target.position) <= followPlayerRange && Vector2.Distance(transform.position, target.position) > attackRange){
                inRangeFollow = true;
            }
            else{
                inRangeFollow = false;

                if(Vector2.Distance(transform.position, target.position) <= attackRange){
                    inRangeAttack = true;
                }else{
                    inRangeAttack = false;
                }
            }

            if(!inRangeFollow && !inRangeAttack){ //Patrulhando
                agent.enabled = false;

                transform.position = Vector2.MoveTowards(transform.position, pontosRotaLista[indexPontoRota].position, speedMovement * Time.deltaTime);
                transform.right = pontosRotaLista[indexPontoRota].position - transform.position;

                Vector2 direction = pontosRotaLista[indexPontoRota].position - transform.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
                
                if(Vector2.Distance(transform.position, pontosRotaLista[indexPontoRota].position) == 0){
                    if(indexPontoRota == (pontosRotaLista.Count -1)){
                        indexPontoRota = 0;
                    }else{
                        indexPontoRota++;
                    }
                }

            }else if(inRangeFollow){ //Perseguindo player
                agent.enabled = true;
                agent.SetDestination(target.transform.position);

            }else if(inRangeAttack){ //Atacando Player
                agent.enabled = false;

                Vector2 direction = target.position - transform.position;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

                if(navioInfo.shipType == ShipType.Shooter){
                    Attack_Shooter();
                }
            }
        }
    }

    public void TakeDamageEnemy(float damage){
        currentHealth -= damage;
        fillHealth.fillAmount = currentHealth / totalHealth;
        SetStateEnemy(currentHealth);  

        if(currentHealth <= 0){
            gamePlayManager.GerarExplosao(transform);
            Destroy(gameObject, 0.35f);
            isAlive = false;
            gamePlayManager.SumPoint();
            gamePlayManager.routes.Add(rota);
            rota.GetComponent<RotaController>().ocupada = false;

            if(gamePlayManager.routes.Count == 0){
                InvokeRepeating("SpawnEnemy", 1f, PlayerPrefs.GetInt("tempoSpawn", 15));
            }
        }
    }

    void SetStateEnemy(float health){
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

    void OnCollisionEnter2D(Collision2D collider){
        if(navioInfo.shipType == ShipType.Chaser){
            if(collider.transform.CompareTag("Player") && isAlive){
                Debug.Log("Attack Chaser!");
                Attack_Chaser(collider.transform);
                collider.gameObject.GetComponent<PlayerController>().TakeDamagePlayer(navioInfo.shipDamage);

            }
        }
    }

    void Attack_Chaser(Transform transformAlvo){
        gamePlayManager.GerarExplosao(transformAlvo);
        gamePlayManager.GerarExplosao(transform);
        TakeDamageEnemy(100);
    }

    void Attack_Shooter(){
        if(timeBtwShots <= 0){
            GameObject bullet = Instantiate(projectile, pointShoot.position, pointShoot.rotation);
            bullet.GetComponent<Bullet>().damageBullet = navioInfo.shipDamage;
            timeBtwShots = startTimeBtwShots;
        }else{
            timeBtwShots -= Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followPlayerRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    
}
