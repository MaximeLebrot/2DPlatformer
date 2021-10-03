using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Camera camera;
    public GameObject bullet;
    public LayerMask mask;
    public float fireRate = 1f;
    //What direction to shoot
    float aimAngle;
    Vector3 aimDirection;
    bool hitCheck;

    public bool canShoot;

    float rateOfFirePointer;
    void Update()
    {
        Raycast();

        if (Input.GetAxisRaw("Fire1") != 0 && Time.time > rateOfFirePointer && !hitCheck && canShoot)
        {
            rateOfFirePointer = Time.time + fireRate;

            GameObject bulletObject = Instantiate(bullet, transform.position + aimDirection, Quaternion.Euler(0,0,aimAngle * Mathf.Rad2Deg));

            Rigidbody2D bulletClone = bulletObject.GetComponent<Rigidbody2D>();
            bulletClone.velocity = aimDirection * 15f;

        }
    }
    

    void Raycast()
    {

        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;

        if (Input.GetAxisRaw("Vertical2") != 0 || Input.GetAxisRaw("Horizontal2") != 0)
        {
            float x = Input.GetAxis("Horizontal2");
            float y = Input.GetAxis("Vertical2");
            aimAngle = Mathf.Atan2(y, x);
        }
        else
        {
            aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        }

        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right/2;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, .5f, mask);
        Debug.DrawRay(transform.position, aimDirection);

        if (hit)
        {
            hitCheck = true;
           
        }
        else
        {
            hitCheck = false;
        }
    }
}

