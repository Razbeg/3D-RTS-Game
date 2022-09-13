using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Food
}

public class ResourceSource : MonoBehaviour
{
    public ResourceType type;
    public int quantity;

    public UnityEvent onQuantityChange;

    public void GatherResource(int amount, Player gatheringPlayer)
    {
        quantity -= amount;

        int amountToGive = amount;

        if (quantity < 0)
        {
            amountToGive = amount + quantity;
        }
        
        gatheringPlayer.GainResource(type, amountToGive);

        if (quantity <= 0)
        {
            Destroy(gameObject);
        }

        if (onQuantityChange != null)
        {
            onQuantityChange.Invoke();
        }
    }
}
