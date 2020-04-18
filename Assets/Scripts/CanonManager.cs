using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonManager : MonoBehaviour
{
    public GameObject shotPrefab;
    public Transform firePoint;


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.name == "LettersField")
            {                
                if (Input.GetMouseButton(0))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                    Vector3 aimDirection = (mousePosition - transform.position).normalized;
                    float angle = (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg) + 90;
                    transform.eulerAngles = new Vector3(0, 0, angle);
                }if (Input.GetMouseButtonUp(0))
                {
                    Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
                }
            }
        }

    }

}
