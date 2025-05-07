using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; 
    private void Start() {
        Time.timeScale =1;
    }
    void Update()
    {
        if(gameObject.activeInHierarchy)
        {
            gameObject.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }
}
