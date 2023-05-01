using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public Transform fireKnife;
    public GameObject knife;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the knife
        GameObject newKnife = Instantiate(knife, fireKnife.position, fireKnife.rotation);

        // Get the mouse position in world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        // Calculate the direction and normalize
        Vector2 direction = (new Vector2(mouseWorldPosition.x, mouseWorldPosition.y) - (Vector2)fireKnife.position).normalized;

        // Set the knife's velocity
        newKnife.GetComponent<Rigidbody2D>().velocity = direction;

        // Calculate the rotation angle based on the velocity
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the knife's rotation
        newKnife.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
