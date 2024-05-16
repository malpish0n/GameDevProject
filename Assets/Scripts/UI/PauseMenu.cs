using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _UI;
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private Camera _pauseCamera;


    private void Start()
    {
        _pauseCamera.enabled = false;
    }
    private void Update()
    {
        EnablePauseMenu();
    }

    private void LateUpdate()
    {

    }

    public bool EnablePauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameCamera.enabled = false;
            _pauseCamera.enabled = true;
            _UI.SetActive(false);
            _pauseMenu.SetActive(true);

            return true;
        }

        return false;
    }

    public void Test()
    {

    }
}
