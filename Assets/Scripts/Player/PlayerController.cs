using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory
{
    public int level;

    //Buffs
    public bool antivirus;
    public bool cryptoDurability;
    public bool optialFiber;
    public bool https;
    public bool archive;
    //Debuffs
    public bool publicNet;
    public bool exploit;
    public bool weakPassword;
    public bool damagedHDD;
    public bool oldVersion;

    public void Reset()
    {
        level = 0;
        antivirus = false;
        cryptoDurability = false;
        optialFiber = false;
        https = false;
        archive = false;
        publicNet = false;
        exploit = false;
        weakPassword = false;
        damagedHDD = false;
        oldVersion = false;
    }
}

public class PlayerController : MonoBehaviour
{
    public enum Item
    {
        antivirus,
        cryptoDurability,
        optialFiber,
        https,
        archive,
        publicNet,
        exploit,
        weakPassword,
        damagedHDD,
        oldVersion
    }

    [SerializeField]
    private GameObject deadParticle;
    private float health;
    [SerializeField]
    private float maxHealth;

    private float shield, maxShield;
    private float addSpeed;
    private float damageRatio;
    [SerializeField]
    private GameObject sprite;

    public Inventory myInventory;
    [SerializeField]
    private WeaponHolder myWeaponHolder;
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    public static PlayerController instance;
    [HideInInspector]
    public bool isLose;
    private Coroutine reload = null;
    private Coroutine fall = null;
    [HideInInspector]
    public bool activated;

    public void Initialization()
    {
        activated = false;
        SetInstance();
        maxShield = 10;
        myInventory = new Inventory();
        myInventory.Reset();
        health = maxHealth;
        shield = 0;
        addSpeed = 0;
        damageRatio = 0;
        isLose = false;
        Camera.main.transform.parent.GetComponent<CameraController>().Initialization();
        myWeaponHolder.Initialization();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        while (UIDrawer.instance == null)
            yield return null;
        UIDrawer.instance.UpdateUI(health / maxHealth);
        UIDrawer.instance.UpdateUIShield(shield / maxShield);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            Destroy(gameObject);
        else
        {
            activated = false;
            reload = null;
            fall = null;
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.eulerAngles = Vector3.zero;
            if (Camera.main.transform.parent != null)
                if (Camera.main.transform.parent.TryGetComponent(out CameraController cameraController))
                    cameraController.Initialization();
            rb = GetComponent<Rigidbody2D>();
            StartCoroutine(UpdateUI());
        }
    }

    private void SetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
        if (!isLose && activated)
        {
            if (Input.GetMouseButton(0) && SceneManager.GetActiveScene().name == "Game")
            {
                myWeaponHolder.WeaponShot();
            }
            myWeaponHolder.Rotate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (rb != null)
                rb.velocity = direction * speed * (1 + addSpeed);
        }
    }

    private void Death()
    {
        health = 0;
        isLose = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.velocity = Vector2.zero;
        myWeaponHolder.gameObject.SetActive(false);
        Instantiate(deadParticle, transform.position, Quaternion.identity).GetComponent<ParticleController>().Initialization();
        sprite.SetActive(false);
        StartCoroutine(LoadLoseScene());
        StartCoroutine(UpdateUI());
    }

    public void GetDamage(float damage)
    {
        if (!isLose)
        {
            if (shield != 0)
            {
                if (shield > damage * (1 + damageRatio))
                    shield -= damage * (1 + damageRatio);
                else
                    shield = 0;
                StartCoroutine(UpdateUI());
            }
            else
            {
                if (health > damage * (1 + damageRatio))
                {
                    if (myInventory.exploit && Random.Range(0, 100) == 0)
                    {
                        if (!isLose)
                            Death();
                    }
                    else
                        health -= damage * (1 + damageRatio);
                }
                else
                {
                    if (!isLose)
                        Death();
                }
            }
            if (myInventory.exploit && !isLose)
            {
                if (Random.Range(0, 25) == 0)
                    Death();
            }
            StartCoroutine(UpdateUI());
        }
    }

    public void FallInPortal()
    {
        if (fall == null)
            fall = StartCoroutine(Fall());
    }

    private IEnumerator LoadLoseScene()
    {
        yield return new WaitForSeconds(1f);
        instance = null;
        SceneManager.LoadScene("LoseScene");
    }

    private IEnumerator Fall()
    {
        float speed = 0;
        rb.velocity = Vector2.zero;
        rb = null;
        while (speed < 100)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, 5 * Time.deltaTime);
            speed += 100 * Time.deltaTime;
            transform.localScale = new Vector3(1 / (speed / 50 + 1), 1 / (speed / 50 + 1), 1);
            transform.eulerAngles = new Vector3(0, 0, speed * 18);
            yield return null;
        }
        CameraController.instance = null;
        myInventory.level++;
        if (myInventory.level < 10)
            SceneManager.LoadScene("Upgrade");
        else
        {
            if (myInventory.level == 10)
                SceneManager.LoadScene("Game");
            else
                ExitToMenu();
        }
    }

    private void ExitToMenu()
    {
        instance = null;
        SceneManager.LoadScene("WinnerScene");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") && reload == null)
        {
            GetDamage(3);
            reload = StartCoroutine(Reload(0.5f));
        }

    }

    private IEnumerator Reload(float duration)
    {
        yield return new WaitForSeconds(duration);
        reload = null;
    }

    public void AddItem(Item item)
    {
        switch (item)
        {
            case Item.antivirus:
                myInventory.antivirus = true;
                shield = maxShield;
                health = maxHealth;
                break;
            case Item.cryptoDurability:
                shield = maxShield;
                myInventory.cryptoDurability = true;
                break;
            case Item.optialFiber:
                addSpeed += 0.25f;
                myInventory.optialFiber = true;
                break;
            case Item.https:
                health += maxHealth * 0.5f;
                maxHealth *= 1.5f;
                myInventory.https = true;
                break;
            case Item.archive:
                health = maxHealth;
                myInventory.archive = true;
                break;

            case Item.publicNet:
                addSpeed -= 0.25f;
                myInventory.publicNet = true;
                break;
            case Item.exploit:
                shield = 0;
                StartCoroutine(UpdateUI());
                myInventory.exploit = true;
                break;
            case Item.weakPassword:
                damageRatio = 0.5f;
                myInventory.weakPassword = true;
                break;
            case Item.damagedHDD:
                health -= maxHealth * 0.5f;
                maxHealth /= 1.5f;
                myInventory.damagedHDD = true;
                break;
            case Item.oldVersion:
                myInventory.oldVersion = true;
                break;
            default:
                break;
        }
    }
}
