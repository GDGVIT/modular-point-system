using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    ObjectPooler pool;
    InputSystem_Actions input;

    public List<GunIScriptableObject> gunTypes;
    GunIScriptableObject activeGun;

    bool isReloading = false;
    int currAmmo = 0;

    float canFire, fireInput = 0;

    [SerializeField] Vector3 reloadRotation;
    [SerializeField] Transform gunParent;
    AudioSource audioSource;

    [Header("Objects")]
    Transform shellSpawn;
    List<ParticleSystem> muzzleFlash = new List<ParticleSystem>();
    List<GameObject> flashLight = new List<GameObject>();
    public List<Transform> muzzle = new List<Transform>();

    private void Start()
    {
        pool = ObjectPooler.instance;
        input = new InputSystem_Actions();
        audioSource = GetComponent<AudioSource>();

        //gunTypes[Random.Range(0, gunTypes.Count)].active = true; //To get a random gun type

        foreach (GunIScriptableObject item in gunTypes)
        {
            if (item.active)
            {
                activeGun = item;
                //item.active = false; //To save the last active gun
                break;
            }
        }

        if (activeGun == null) activeGun = gunTypes[Random.Range(0, gunTypes.Count)];

        Transform gunInstance = Instantiate(activeGun.gun, gunParent).transform;
        Debug.Log(gunInstance.name);

        for (int i = 0; i < gunInstance.childCount; i++)
        {
            Transform item = gunInstance.transform.GetChild(i);

            if (item.CompareTag("muzzle") && item != null)
            {
                muzzle.Add(item);
                flashLight.Add(item.Find("Flash").gameObject);
                muzzleFlash.Add(item.Find("Muzzle Flash").GetComponent<ParticleSystem>());
            }

            if (item.gameObject.name == "Mag") shellSpawn = item;
        }

        currAmmo = activeGun.ammo;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Attack.performed += ctx => Attack();
    }

    void Attack()
    {
        if (isReloading) return;

        if (currAmmo <= 0)
        {
            StartCoroutine(reloading());
            return;
        }

        if (canFire <= Time.time)
        {
            if (activeGun.getStat(StatTypes.attackCooldown) > 0) canFire = Time.time + (1 / (activeGun.getStat(StatTypes.attackCooldown) / 60f));
            else canFire = Time.time;

            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        if (pool != null)
        {
            if (!activeGun.infiniteAmmo) currAmmo--;

            for (int i = 0; i < muzzle.Count; i++)
            {
                if (muzzle[i].gameObject.activeInHierarchy)
                {
                    GameObject bullet = pool.GetObject(1);
                    bullet.transform.SetPositionAndRotation(muzzle[i].position, muzzle[i].rotation);
                    bullet.GetComponent<Rigidbody>().AddForce(muzzle[i].forward * activeGun.bulletSpeed, ForceMode.Impulse);
                    muzzleFlash[i].Play();
                    flashLight[i].SetActive(true);
                    StartCoroutine(bullet.GetComponent<bullet>().Initialise(activeGun.range));
                }
            }

            GameObject shell = pool.GetObject(3);
            shell.transform.SetPositionAndRotation(shellSpawn.position, shellSpawn.rotation);

            audioSource.PlayOneShot(activeGun.audioClip);

            yield return new WaitForSeconds(.2f);

            foreach (GameObject item in flashLight) item.SetActive(false);
        }
    }

    IEnumerator reloading()
    {
        Debug.Log("Reloading");
        isReloading = true;

        yield return new WaitForSeconds(activeGun.reloadTime);
        isReloading = false;

        currAmmo = activeGun.ammo;
    }
}
