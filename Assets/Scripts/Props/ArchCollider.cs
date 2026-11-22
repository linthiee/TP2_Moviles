using UnityEngine;

public class ArchCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private bool _hasBeenPassedYet;

    private void Start()
    {
        _hasBeenPassedYet = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hasBeenPassedYet)
        {
            _hasBeenPassedYet = true;
            EventBus.Raise<OnArchPassed>(transform.position.y);
            Debug.Log("Arch passed!");
        }
    }

    public void ResetTrigger()
    {
        _hasBeenPassedYet = false;
    }
}