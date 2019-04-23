﻿using UnityEngine;

namespace CompleteProject
{
    public class EnemyHealth : MonoBehaviour
    {
        public float startingHealth = 5;            // The amount of health the enemy starts the game with.
        public float currentHealth;                   // The current health the enemy has.
        
		public float buff = 0;

		//public AudioClip hurtClip;
		//public AudioClip deathClip;                 // The sound to play when the enemy dies.

        AudioSource enemyAudio;                     // Reference to the audio source.
        ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
        CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
        bool isDead;                                // Whether the enemy is dead.
        bool isSinking;                             // Whether the enemy has started sinking through the floor.

		public GameObject explosion;
		public GameObject playerExplosion;
		public int scoreValue;
		private Done_GameController gameController;

		public Collider other;

        void Awake ()
        {
            // Setting up the references.
            enemyAudio = GetComponent <AudioSource> ();
            hitParticles = GetComponentInChildren <ParticleSystem> ();
            capsuleCollider = GetComponent <CapsuleCollider> ();

            // Setting the current health when the enemy first spawns.
            currentHealth = startingHealth;
        }

		void Start ()
		{
			GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}


        void Update ()
        {

        }


        public void TakeDamage (float amount)
        {
			// If the enemy is dead...
            if(isDead)
			{
                // ... no need to take damage so exit the function.
                return;
			}

            // Reduce the current health by the amount of damage sustained.
            currentHealth -= amount + buff;
            
            // Set the position of the particle system to where the hit was sustained.
            //hitParticles.transform.position = hitPoint;

            // And play the particles.
            hitParticles.Play();

            // If the current health is less than or equal to zero...
			if(currentHealth <= 0 && !isDead)
            {
                // ... the enemy is dead.
                Death ();
            }
        }


        void Death ()
        {
			enemyAudio.Play();
			Instantiate(playerExplosion, transform.position, transform.rotation);

			// The enemy is dead.
            isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
			capsuleCollider.isTrigger = true;

			gameController.AddScore(scoreValue);
			Destroy (other.gameObject);
			Destroy (gameObject);
        }

		void OnTriggerEnter (Collider other)
		{
			if(other.gameObject.layer ==LayerMask.NameToLayer("Projectile") && other.tag != "Enemy")
			{
				// Play the hurt sound effect.
				enemyAudio.Play();

				Instantiate(playerExplosion, transform.position, transform.rotation);
			}

			if (other.tag == "Player")
			{
				Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
				gameController.GameOver();
			}
			else
			{
				return;
			}

			//gameController.AddScore(scoreValue);

			//Death();
		}

		public void Buff(float d)
		{
			buff += d;
		}
			
    }
}