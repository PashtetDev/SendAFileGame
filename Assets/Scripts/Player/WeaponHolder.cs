using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    private Weapon myWeapon;
    private SpriteRenderer weaponSprite;
    private float angleZ = 0;

    public void Initialization()
    {
        myWeapon = transform.GetChild(0).GetComponent<Weapon>();
        weaponSprite = myWeapon.GetComponent<SpriteRenderer>();
        myWeapon.Initialization();
    }

    public void Rotate(Vector3 targetVector)
    {
        Vector2 mousePosition = targetVector - transform.position;
        float angle = mousePosition.y / mousePosition.x;

        Vector3 eulerAngles;

        if (mousePosition.x > 0)
        {
            weaponSprite.flipY = false;
            eulerAngles = new Vector3(0, 0, Mathf.Atan(angle) * Mathf.Rad2Deg);
        }
        else
        {
            weaponSprite.flipY = true;
            eulerAngles = new Vector3(0, 0, Mathf.Atan(angle) * Mathf.Rad2Deg + 180);
        }

        angleZ = Mathf.LerpAngle((angleZ + 360) % 360, (eulerAngles.z + 360) % 360, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, angleZ);
    }

    public void WeaponShot()
    {
        myWeapon.Shot(transform.eulerAngles);
    }
}
