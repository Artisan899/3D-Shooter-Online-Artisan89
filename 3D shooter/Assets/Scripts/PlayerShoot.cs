using Mirror;
using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerShoot : NetworkBehaviour
{
    public Weapon weapon;
    public Camera cam;
    public LayerMask mask;
    public TextMeshProUGUI ammoText;

    private Animator _anim;
    

    [SyncVar]
    private int currentAmmo;

    private bool isReloading = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        currentAmmo = weapon.mxAmmo;  //���������� �������� � ������
        UpdateAmmoText();
    }

    [Command]
    void CmdPlayerShoot(GameObject player, float damage)
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(damage); //��������� ����� ������� �� ������
        }
    }

    void Shoot()
    {

        if (isReloading || currentAmmo <= 0) //��� ����������� �� ��������
        {
            return;
        }

        if (currentAmmo <= 0) // //��� ����������� �� ��������
        {
            return;
        }

        if (cam == null) //���������� ������, ��� ��������� � ������ � ������
            return;

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))  //�������� �������� ������
        {
            if (_hit.collider.tag == "Player")
            {
                CmdPlayerShoot(_hit.collider.gameObject, weapon.damage); //����� � �������� ������ �������� ����
            }

            if (_hit.collider.tag == "Zombie")
            {
                ZombieTakeDamage zombieScript = _hit.collider.gameObject.GetComponent<ZombieTakeDamage>();
                if (zombieScript != null)
                {
                    zombieScript.TakeDamage(weapon.damage);  //NPC �������� ����
                }
            }
        }

        currentAmmo--;  //����� �������� ���-�� �������� -1
        UpdateAmmoText(); // ������� ���������� ����������
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo.ToString(); //���������� ����������
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //������� �������������� � ������� ���
        {
            _anim.SetBool("FireWeapon", true); //�������� ��������
            Shoot(); //�������
            StartCoroutine(ResetFireWeaponAnimation()); //����������� �������� ��������
        }
        if (Input.GetKeyDown(KeyCode.R)) //����������� �� ���� �������
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator ResetFireWeaponAnimation()
    {
        yield return new WaitForSeconds(0.2f); // ������������ �������� ��������
        _anim.SetBool("FireWeapon", false);
    }



    IEnumerator Reload()
    {
        if (isReloading || currentAmmo == weapon.mxAmmo) // �������� �� ����������� � ������ ���������
        {
            yield break;
        }

        _anim.SetBool("Reloadd", true); // ����� �������� �����������
        isReloading = true;
        yield return new WaitForSeconds(weapon.reloading); // ����� ����������� �� weapon
        currentAmmo = weapon.mxAmmo; // ���������� �������� �� ����
        _anim.SetBool("Reloadd", false); // ����� �������� �����������
        isReloading = false;
        UpdateAmmoText();
    }

}