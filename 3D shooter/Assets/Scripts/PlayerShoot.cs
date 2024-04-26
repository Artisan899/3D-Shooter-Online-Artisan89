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
        currentAmmo = weapon.mxAmmo;  //Количество патронов у игрока
        UpdateAmmoText();
    }

    [Command]
    void CmdPlayerShoot(GameObject player, float damage)
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(damage); //Получение урона игроком от игрока
        }
    }

    void Shoot()
    {

        if (isReloading || currentAmmo <= 0) //При перезарядки не стреляем
        {
            return;
        }

        if (currentAmmo <= 0) // //При перезарядки не стреляем
        {
            return;
        }

        if (cam == null) //Постоновка камеры, для избежания её потери и ошибок
            return;

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))  //Механика выстрела игрока
        {
            if (_hit.collider.tag == "Player")
            {
                CmdPlayerShoot(_hit.collider.gameObject, weapon.damage); //Игрок в которого попали получает урон
            }

            if (_hit.collider.tag == "Zombie")
            {
                ZombieTakeDamage zombieScript = _hit.collider.gameObject.GetComponent<ZombieTakeDamage>();
                if (zombieScript != null)
                {
                    zombieScript.TakeDamage(weapon.damage);  //NPC получает урон
                }
            }
        }

        currentAmmo--;  //После выстрела кол-во патронов -1
        UpdateAmmoText(); // Функция Визуальное обновление
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo.ToString(); //Визуальное обновление
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Выстрел осуществляется с помощью ЛКМ
        {
            _anim.SetBool("FireWeapon", true); //Анимация выстрела
            Shoot(); //Выстрел
            StartCoroutine(ResetFireWeaponAnimation()); //Переазярдка анимации выстрела
        }
        if (Input.GetKeyDown(KeyCode.R)) //Перезарядка на соот клавишу
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator ResetFireWeaponAnimation()
    {
        yield return new WaitForSeconds(0.2f); // длительность анимации выстрела
        _anim.SetBool("FireWeapon", false);
    }



    IEnumerator Reload()
    {
        if (isReloading || currentAmmo == weapon.mxAmmo) // Проверка на перезарядку и полное магазином
        {
            yield break;
        }

        _anim.SetBool("Reloadd", true); // Старт анимации перезарядки
        isReloading = true;
        yield return new WaitForSeconds(weapon.reloading); // Время перезарядки из weapon
        currentAmmo = weapon.mxAmmo; // Обновление патронов на макс
        _anim.SetBool("Reloadd", false); // Конец анимации перезарядки
        isReloading = false;
        UpdateAmmoText();
    }

}