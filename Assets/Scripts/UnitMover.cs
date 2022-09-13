using UnityEngine;

public class UnitMover : MonoBehaviour
{
    public static Vector3[] GetUnitGroupDestinations(Vector3 moveToPos, int numUnits, float unitGap)
    {
        Vector3[] destinations = new Vector3[numUnits];

        int rows = Mathf.RoundToInt(Mathf.Sqrt(numUnits));
        int cols = Mathf.CeilToInt((float)numUnits / (float)rows);

        int curRow = 0;
        int curCol = 0;

        float width = ((float) rows - 1) * unitGap;
        float length = ((float) cols - 1) * unitGap;

        for (int x = 0; x < numUnits; x++)
        {
            destinations[x] = moveToPos + new Vector3(curRow, 0, curCol) * unitGap - new Vector3(length / 2, 0, width / 2);
            curCol++;

            if (curCol == rows)
            {
                curCol = 0;
                curRow++;
            }
        }
        
        return destinations;
    }

    public static Vector3[] GetUnitGroupDestinationsAroundResource(Vector3 resourcePos, int unitsNum)
    {
        Vector3[] destinations = new Vector3[unitsNum];
        float unitsDistanceGap = 360.0f / (float)unitsNum;

        for (int x = 0; x < unitsNum; x++)
        {
            float angle = unitsDistanceGap * x;
            Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            destinations[x] = resourcePos + dir;
        }

        return destinations;
    }

    public static Vector3 GetUnitDestinationAroundResource(Vector3 resourcePos)
    {
        float angle = Random.Range(0, 360);
        Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));

        return resourcePos + dir;
    }
}
