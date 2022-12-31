using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonMovement : MonoBehaviour
{
    private float xAxis, yAxis;
    [SerializeField] private Transform camFollowPos;
    [SerializeField] private float mouseSense;
    
    [HideInInspector] public CinemachineVirtualCamera vCam;
    public float adsFov = 40;
    public float currentFov;
    public float hipFov;
    public float fovSmoothSpeed = 10;

    private InGameUI gameUI;
    
    private void Awake()
    {
        // Locks the cursor
        Cursor.lockState = CursorLockMode.Locked;

        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        hipFov = vCam.m_Lens.FieldOfView;
    }
    
    private void Start()
    {
        gameUI = InGameUI.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // If either win or lose, then return
        if (gameUI.IsWin() || gameUI.IsGameOver()) return;
        
        xAxis += (Input.GetAxisRaw("Mouse X") * mouseSense);
        yAxis -= (Input.GetAxisRaw("Mouse Y") * mouseSense);
        yAxis = Mathf.Clamp(yAxis, -80, 80);
        
        
        
       
        
        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, xAxis, transform.localEulerAngles.z);
    }
}
