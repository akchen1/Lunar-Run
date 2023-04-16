using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

//[ExecuteInEditMode]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public bool freezeTime;

    [SerializeField] private InputActionReference moveInputAction;
    [SerializeField] private CinemachineFreeLook CinemachineFreeLook;

    private Movement MovementController;

    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        moveInputAction.action.performed += OnPlayerInputMoveStart;
        moveInputAction.action.canceled += OnPlayerInputMoveEnd;
    }

    private void OnDisable()
    {
        moveInputAction.action.canceled -= OnPlayerInputMoveEnd;
        moveInputAction.action.performed -= OnPlayerInputMoveStart;
    }


    private void Start()
    {
        MovementController = GetComponent<Movement>();
        //Time.timeScale = 0;
        freezeTime = true;
        CinemachineFreeLook.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("SENS", 0.5f) * 600f;
    }


    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing) return;
        if (freezeTime) return;
        PlayerMove();
    }

    private void OnPlayerInputMoveStart(InputAction.CallbackContext obj)
    {
        //Time.timeScale = 1;
        freezeTime = false;
    }

    private void OnPlayerInputMoveEnd(InputAction.CallbackContext obj)
    {
        //Time.timeScale = 0;
        freezeTime = true;
    }
  
    private void PlayerMove()
    {
        Vector2 direction = moveInputAction.action.ReadValue<Vector2>();
        MovementController.Move(direction.normalized);
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("Win");
        } else if (collision.gameObject.tag == "Meteor")
        {
            Debug.Log("Lose");
            GameManager.Instance.EndGame("meteor");
        } else if (collision.gameObject.tag == "Oxygen")
        {
            GameManager.Instance.ObtainedOxygen(collision.gameObject);
        } else if (collision.gameObject.tag == "Cheese")
        {
            GameManager.Instance.ObtainedCheese(collision.gameObject);
        }
    }
}
