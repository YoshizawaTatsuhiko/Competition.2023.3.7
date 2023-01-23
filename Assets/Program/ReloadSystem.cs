using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSystem : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DebugReload();
        }
    }

    private void DebugReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
