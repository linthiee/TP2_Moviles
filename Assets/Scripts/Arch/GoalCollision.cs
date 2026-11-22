using UnityEngine;

public class GoalCollision : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Win!");
        if (collision.gameObject.CompareTag("Player"))
            EventBus.Raise<OnPlayerWin>();
    }
}