using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
    // Spell ideas:
    // Circle Defense: Fires magic missiles in a circle around the player [DONE!]
    // Acid Bolt: magic missile but green, low damage but afflicts damage over time [DONE!]
    // Warrior's Might: coat yourself in magic armor, reducing damage you take from physical attacks
    // Simulacrum: summons a little dude to help you out [LEAST PRIORITY]

    private Player player;
    private PlayerMana playerMana;
    private SpriteRenderer playerSprite;

    [SerializeField]
    private Transform aimer;
    [SerializeField]
    private Transform circleRotater;

    [SerializeField]
    private Color wraithFormColor;

    private bool onCooldown = false;

    private void Awake()
    {
        playerMana = GetComponent<PlayerMana>();
        player = GetComponent<Player>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player.PlayerEnabled)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !onCooldown && playerMana.RemoveMana(20f))
        {
            onCooldown = true;
            StartCoroutine(Fireball());
        }

        if (Input.GetKeyDown(KeyCode.X) && !onCooldown && playerMana.RemoveMana(35f))
        {
            onCooldown = true;
            StartCoroutine(AcidMissile());
        }

        if (Input.GetKeyDown(KeyCode.C) && !onCooldown && playerMana.RemoveMana(50f))
        {
            onCooldown = true;
            StartCoroutine(CircleDefense());
        }

        if (Input.GetKeyDown(KeyCode.V) && !onCooldown && playerMana.RemoveMana(40f))
        {
            onCooldown = true;
            StartCoroutine(WraithForm());
        }
    }

    private IEnumerator Fireball()
    {
        PlayerProjectile newMissile = PlayerProjectile.Create(PlayerProjectile.MissileType.FireMissile, 2);
        newMissile.transform.SetPositionAndRotation(aimer.position, aimer.rotation);

        yield return new WaitForSeconds(0.75f);
        onCooldown = false;
    }

    private IEnumerator CircleDefense()
    {
        for (int i = 0; i < 12; i++)
        {
            circleRotater.RotateAround(transform.position, Vector3.forward, 30f);
            PlayerProjectile newMissile = PlayerProjectile.Create(PlayerProjectile.MissileType.MagicMissile, 2);
            newMissile.transform.SetPositionAndRotation(circleRotater.position, circleRotater.rotation);
            yield return new WaitForSeconds(0.02f);
        }

        circleRotater.localPosition = new Vector2(0f, 0.5f);
        circleRotater.localRotation = Quaternion.Euler(0f, 0f, 90f);

        yield return new WaitForSeconds(2.5f);
        onCooldown = false;
    }

    private IEnumerator AcidMissile()
    {
        PlayerProjectile newMissile = PlayerProjectile.Create(PlayerProjectile.MissileType.AcidMissile, 1);
        newMissile.transform.SetPositionAndRotation(aimer.position, aimer.rotation);

        yield return new WaitForSeconds(1.5f);
        onCooldown = false;
    }

    private IEnumerator WraithForm()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true); // Disables collision between player and enemies
        player.SetPlayerSpeed(8f);
        playerSprite.color = wraithFormColor;

        yield return new WaitForSeconds(3f);

        Physics2D.IgnoreLayerCollision(7, 8, false); // Enables collision between player and enemies
        player.SetPlayerSpeed(5f);
        playerSprite.color = Color.white;

        yield return new WaitForSeconds(1.5f);

        onCooldown = false;
    }
}
