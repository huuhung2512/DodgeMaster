using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCharacter : MonoBehaviour
{
    public int savedCharacter;
    public GameObject[] characterModels;
    [SerializeField] PlayerController playerController;

    void Start()
    {
        savedCharacter = PlayerPrefs.GetInt("SelectedCar", 0);
        foreach (GameObject car in characterModels)
        {
            car.SetActive(false);
        }
        characterModels[savedCharacter].SetActive(true);
        playerController.animator = characterModels[savedCharacter].GetComponent<Animator>();
    }
}
