using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyBullet bulletPrefab;
    [SerializeField] Transform target;
    [SerializeField] float trackingSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float shootInterval;
    public int hitPoints = 5;

    private Rigidbody2D _rigidBody;
    EnemySpawner instance;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<Player>().transform;
        instance = FindObjectOfType<EnemySpawner>();
    }

    private void Start()
    {
        StartCoroutine(EnemyShoot());
    }

    private void FixedUpdate()
    {
        EnemyMovement();
        EnemyDead();
    }

    private IEnumerator EnemyShoot()
    {
        while (true)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            //bullet.Project(transform.up);
            bullet.Project(target.transform.position);
            yield return new WaitForSeconds(5.0f);
        }
    }

    private void EnemyMovement()
    {
        if (target)
        {
            float step = trackingSpeed * Time.fixedDeltaTime;
            var toward = Vector3.MoveTowards(_rigidBody.position, target.position, step);
            _rigidBody.AddForce(target.transform.position - _rigidBody.transform.position);
            _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, maxSpeed);
            Debug.DrawLine(_rigidBody.position, toward);
        }
    }

    private void EnemyDead()
    {
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
            var gameManager = FindObjectOfType<GameManager>();
            gameManager.OnKillEnemy(this);

            instance.SpawnEnemy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = 0;
            collision.gameObject.SetActive(false);

            FindObjectOfType<GameManager>().PlayerDead();
        }

        if (collision.gameObject.tag == "Bullet")
        {
            hitPoints--;
        }
    }
}
