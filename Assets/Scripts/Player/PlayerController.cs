using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private WeaponHolder myWeaponHolder;
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    public static PlayerController instance;
    [HideInInspector]
    public bool isLose;

    public void Initialization()
    {
        SetInstance();
        isLose = false;
        Camera.main.transform.parent.GetComponent<CameraController>().Initialization();
        myWeaponHolder.Initialization();
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
        if (!isLose)
        {
            if (Input.GetMouseButton(0))
            {
                myWeaponHolder.WeaponShot();
            }
            myWeaponHolder.Rotate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void Death()
    {
        Debug.Log("Auch");
        isLose = true;
    }

    public void GetDamage(int damage)
    {
        if (health > damage)
        {
            health = 0;
            if (!isLose)
                Death();
        }
        else
            health -= damage;
    }
}
