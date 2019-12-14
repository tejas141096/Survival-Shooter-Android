﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerShooting : MonoBehaviour
    {
        public int damagePerShot = 20;
        public float timeBetweenBullets = 0.15f;
        public float range = 100f;
        float timer;
        Ray shootRay = new Ray();
        RaycastHit shootHit;
        int shootableMask;
        ParticleSystem gunParticles;
        LineRenderer gunLine;
        AudioSource gunAudio;
        Light gunLight;
		public Light faceLight;
        float effectsDisplayTime = 0.2f;


        void Awake ()
        {
            shootableMask = LayerMask.GetMask ("Shootable");
            gunParticles = GetComponent<ParticleSystem> ();
            gunLine = GetComponent <LineRenderer> ();
            gunAudio = GetComponent<AudioSource> ();
            gunLight = GetComponent<Light> ();
        }


        void Update ()
        {
            timer += Time.deltaTime;

            #if !MOBILE_INPUT
			    if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
                {
                    Shoot ();
                }
            #else
			    if ((CrossPlatformInputManager.GetAxisRaw("Horizontal2") != 0 || CrossPlatformInputManager.GetAxisRaw("Vertical2") != 0) && timer >= timeBetweenBullets)
                {
                    Shoot();
                }
            #endif
            if(timer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects ();
            }
        }


        public void DisableEffects ()
        {
            gunLine.enabled = false;
			faceLight.enabled = false;
            gunLight.enabled = false;
        }


        void Shoot ()
        {
            timer = 0f;
            gunAudio.Play ();
            gunLight.enabled = true;
			faceLight.enabled = true;
            gunParticles.Stop ();
            gunParticles.Play ();
            gunLine.enabled = true;
            gunLine.SetPosition (0, transform.position);
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;
            if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage (damagePerShot, shootHit.point);
                }
                gunLine.SetPosition (1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
            }
        }
    }
}