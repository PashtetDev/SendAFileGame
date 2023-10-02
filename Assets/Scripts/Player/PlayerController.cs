using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory
{
    public int level;

    //Buffs
    public bool antivirus;
    public bool cryptoDurability; //щит со своей прочностью
    public bool optialFiber; //увеличивает скорость бега и стрельбы
    public bool https; //увеличивает количество жизней
    public bool archive; //полностью восстанавливает щит

    //Debuffs
    public bool publicNet; //уменьшает скорость стрельбы и бега (перекрывается оптоволокном)
    public bool exploit; //шанс мгновенной смерти
    public bool weakPassword; //увеличивает получаемый урон
    public bool damagedHDD; //уменьшает общее количество жизней
    public bool oldVersion; //увеличивает число багов на уровнях

    public void Reset()
    {
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

    public bool All()
    {
       return antivirus && cryptoDurability && optialFiber && https && archive && publicNet && exploit && weakPassword && damagedHDD && oldVersion;
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
    [SerializeField]
    private float health;
    private float maxHealh;

    private float shield;
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
    public bool activated;

    public void Initialization()
    {
        activated = false;
        SetInstance();
        myInventory = new Inventory();
        myInventory.Reset();
        maxHealh = health;
        shield = 0;
        addSpeed = 0;
        damageRatio = 0;
        isLose = false;
        Camera.main.transform.parent.GetComponent<CameraController>().Initialization();
        myWeaponHolder.Initialization();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnLevelWasLoaded(int level)
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
        isLose = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.velocity = Vector2.zero;
        myWeaponHolder.gameObject.SetActive(false);
        Instantiate(deadParticle, transform.position, Quaternion.identity).GetComponent<ParticleController>().Initialization();
        sprite.SetActive(false);
    }

    public void GetDamage(float damage)
    {
        if (shield != 0)
        {
            if (shield > damage * (1 + damageRatio))
                shield -= damage * (1 + damageRatio);
            else
                shield = 0;
        }
        else
        {
            if (health > damage * (1 + damageRatio))
            {
                if (myInventory.exploit && Random.Range(0, 100) == 0)
                {
                    health = 0;
                    if (!isLose)
                        Death();
                }
                else
                    health -= damage * (1 + damageRatio);
            }
            else
            {
                health = 0;
                if (!isLose)
                    Death();
            }
        }
    }

    public void FallInPortal()
    {
        if (fall == null)
            fall = StartCoroutine(Fall());
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
        if (!myInventory.All())
            SceneManager.LoadScene("Upgrade");
        else
            SceneManager.LoadScene("Game"); //Потом тут будет босс
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
                health = maxHealh;
                break;
            case Item.cryptoDurability:
                shield = 10;
                myInventory.cryptoDurability = true;
                break;
            case Item.optialFiber:
                if (myInventory.publicNet)
                    addSpeed = 0.25f;
                myInventory.optialFiber = true;
                break;
            case Item.https:
                if (myInventory.damagedHDD)
                    maxHealh *= 1.5f;
                else
                    maxHealh /= 0.5f;
                myInventory.https = true;
                break;
            case Item.archive:
                if (myInventory.cryptoDurability)
                    shield = 10;
                myInventory.archive = true;
                break;

            case Item.publicNet:
                if (!myInventory.optialFiber)
                    addSpeed = -0.25f;
                myInventory.publicNet = true;
                break;
            case Item.exploit:
                myInventory.exploit = true;
                break;
            case Item.weakPassword:
                damageRatio = 0.1f;
                myInventory.weakPassword = true;
                break;
            case Item.damagedHDD:
                if (myInventory.https)
                    maxHealh *= 0.5f;
                else
                    maxHealh /= 1.5f;
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
