using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Character : BasicDamageable
{
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject losePanel;

    private bool _dead = false;
    private CharacterMovementInput movement;

    protected override void Start()
    {
        base.Start();
        movement = transform.GetComponent<CharacterMovementInput>();
    }

    protected override void Die()
    {
        if (_dead) return;
        
        AudioManager.Instance.PlaySound(deathSound);

        movement.StopMovement();

        foreach (var weapon in transform.GetComponentsInChildren<CharacterWeapon>())
        {
            weapon.Die();
        }
        _dead = true;
        StartCoroutine(WaitAndActivateLosePanel());
        // todo: character death Vfx
    }

    private IEnumerator WaitAndActivateLosePanel()
    {
        yield return new WaitForSeconds(0.5f);
        losePanel.SetActive(true);
    }
}
