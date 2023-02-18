using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletModel bulletModel;
    public float speed;
    public float timeToDestroy;
    public float damageBullet;
    

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(bulletModel == BulletModel.Player){
            if(collider.transform.CompareTag("Inimigo")){
                collider.GetComponent<Inimigo>().TakeDamageEnemy(damageBullet);
                FindObjectOfType<GamePlayManager>().GerarExplosao(transform);
                Destroy(gameObject);
            }
        }else{
            if(collider.transform.CompareTag("Player")){
                collider.GetComponent<PlayerController>().TakeDamagePlayer(damageBullet);
                FindObjectOfType<GamePlayManager>().GerarExplosao(transform);
                Destroy(gameObject);
            }
        }
    }


}
