using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSystem : MonoBehaviour
{
    [SerializeField] private KeyCode _key = KeyCode.Return;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // R-Keyが入力される度に、同じシーンが読み込まれる。
        if (Input.GetKeyDown(_key))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
