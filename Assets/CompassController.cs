using System.Collections;
using UnityEngine;

// 日本語対応
public class CompassController : MonoBehaviour
{
    [SerializeField] private GameObject _arrowObj = null;

    private Vector3 _goalPos = Vector3.zero;
    private Coroutine _currentCoroutine = null;

    private void Start()
    {
        if (_arrowObj == null) Debug.LogWarning("Arrow is not found");
        else _arrowObj.SetActive(false);
        
        _goalPos = GameObject.FindGameObjectWithTag("GoalPoint").gameObject.transform.position;
        ShowDestination(10f);
    }

    public void ShowDestination(float duration)
    {
        if (_currentCoroutine != null) return;

        _currentCoroutine = StartCoroutine(PointToGoal(duration));
    }

    private IEnumerator PointToGoal(float duration)
    {
        _arrowObj.SetActive(true);

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            _arrowObj.transform.LookAt(_goalPos);
            yield return null;
        }
        _arrowObj.SetActive(false);
    }
}
