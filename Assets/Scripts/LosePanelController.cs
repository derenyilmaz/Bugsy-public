using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LosePanelController : MonoBehaviour
{
    private float _elapsedTime;

    private void OnEnable()
    {
        Debug.Log("LosePanelController OnEnable");
        _elapsedTime = 0;
    }

    private void Update()
    {
        if (gameObject.activeSelf) _elapsedTime += Time.deltaTime;
        
        if (_elapsedTime < 0.5f) return;
        
        if (Keyboard.current.anyKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.rightButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
    
}
