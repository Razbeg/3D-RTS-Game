using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public float checkRate = 1.0f;
    
    private ResourceSource[] _resources;
    private Player _player;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _resources = FindObjectsOfType<ResourceSource>();
        InvokeRepeating("Check", 0.0f, checkRate);
    }

    public ResourceSource GetClosestResource(Vector3 pos)
    {
        ResourceSource[] closest = new ResourceSource[3];
        float[] closestDist = new float[3];

        foreach (ResourceSource resource in _resources)
        {
            if (resource == null)
            {
                continue;
            }

            float dist = Vector3.Distance(pos, resource.transform.position);

            for (int x = 0; x < closest.Length; x++)
            {
                if (closest[x] == null)
                {
                    closest[x] = resource;
                    closestDist[x] = dist;
                    break;
                }
                else if (dist < closestDist[x])
                {
                    closest[x] = resource;
                    closestDist[x] = dist;
                    break;
                }
            }
        }

        return closest[Random.Range(0, closest.Length)];
    }

    private void Check()
    {
        if (_player.food >= _player.unitCost)
        {
            _player.CreateNewUnit();
        }
    }
}
