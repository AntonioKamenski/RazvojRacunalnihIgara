using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPSCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;

    void Start()
    {
        
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Pew!");
            Shoot();
        }
    }
    private void Shoot(){
        ProcessHit();
        PlayMuzzleFlash();
    }
    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }
    private void ProcessHit()
    {
        RaycastHit hit;
        if(Physics.Raycast(FPSCamera.transform.position,FPSCamera.transform.forward, out hit, range))
        {
            CreateHitEffect(hit);
            //Debug.Log("I hit:" + hit.transform.name);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if(target == null) return;
            target.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }
    private void CreateHitEffect(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }
}
