using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float thrustSpeed = 5.0f;
    [SerializeField] float turnSpeed = 1.0f;
    [SerializeField] ParticleSystem fuelFX;

    private bool _trusting;
    private float _turnDirection;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerMovement();
        PlayerShoot();
    }

    private void PlayerShoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void PlayerMovement()
    {
        _trusting = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
        fuelFX.Play();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1.0f;
            

        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnDirection = -1.0f;
        }
        else
        {
            _turnDirection = 0;
            fuelFX.Stop();
        }
    }

    private void FixedUpdate()
    {
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        if (_trusting)
        {
            _rigidBody.AddForce(transform.up * thrustSpeed);
        }

        if (_turnDirection != 0)
        {
            _rigidBody.AddTorque(_turnDirection * turnSpeed);
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Project(transform.up);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = 0;
            gameObject.SetActive(false);

            FindObjectOfType<GameManager>().PlayerDead();

        }

        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Te pego un balaso el UFO");
        }
    }
}
