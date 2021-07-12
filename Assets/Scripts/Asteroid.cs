using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    public float size = 1;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float asteroidSpeed = 50f;
    public float asteroidLifeTime = 30;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private Vector2 _updateDirection;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        transform.localScale = Vector3.one * size;

        _rigidBody.mass = size;

        //if (gameObject.activeInHierarchy)
        //{
        //    SetTrajectory(_updateDirection);
        //}
    }

    public void SetTrajectory(Vector2 direction)
    {

        _rigidBody.AddForce(direction * asteroidSpeed);
        //_updateDirection = direction;

        Destroy(gameObject, asteroidLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if ((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(gameObject);

            //gameObject.SetActive(false);
            //FindObjectOfType<ObjectPool>().poolSize++;
        }
    }

    private void CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        var half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized * asteroidSpeed);
    }
}
