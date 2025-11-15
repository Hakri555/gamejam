using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLogic : MonoBehaviour
{
    public float RotationSmoothingCoef = 0.1f;
    [SerializeField] GrabberLogic grabberLogic;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        if (!grabberLogic.isShooting && mouseWorldPos.x > 2 && mouseWorldPos.x < 8.8f && mouseWorldPos.y < 5 && mouseWorldPos.y > -3)
        {
            Vector2 direction = (mouseWorldPos - transform.position).normalized;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, RotationSmoothingCoef);

            transform.rotation = Quaternion.Euler(0, 0, currentAngle - 90);
        }
        else if (!grabberLogic.isShooting && (mouseWorldPos.x < 2 || mouseWorldPos.x > 8.8f || mouseWorldPos.y > 5 || mouseWorldPos.y < -3))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), RotationSmoothingCoef*Time.deltaTime);
        }
    }


}
