using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] private MapCell mapCellPrefab;
    [SerializeField] private int requiredForwardCells = 5;
    [SerializeField] private int requiredBackwardCells = 1;
    private Car car;

    private List<MapCell> cells;
    public void Init(Car car)
    {
        this.car = car;
        cells = new List<MapCell>();
    }
    #region map generation
    public void GenerateMap(Vector3 origin)
    {
        transform.position = origin;
        Vector3 offset = new Vector3();
        for (int i = 0; i < requiredBackwardCells + requiredForwardCells + 1; i++)
        {
            cells.Add(GenerateForwardCell(origin, ref offset));
        }
    }

    private MapCell GenerateForwardCell(Vector3 origin, ref Vector3 offset)
    {
        MapCell generatedCell = Instantiate(mapCellPrefab, transform);
        generatedCell.transform.localPosition = origin + offset;
        offset.z += generatedCell.lenght;
        return generatedCell;
    }
    #endregion

    public void CustomUpdate(float deltaTime)
    {
        MapCell middleCell = getMiddleMapCell();
        float carAndMidleCellZDif = car.transform.position.z - middleCell.transform.position.z;
        if (carAndMidleCellZDif > middleCell.lenght)
        {
            //moving back element to forward
            MapCell lastBackCell = cells[0];
            MapCell firstCell = cells[cells.Count - 1];
            lastBackCell.transform.localPosition = firstCell.transform.localPosition + new Vector3(0,0,firstCell.lenght);
            cells.Remove(lastBackCell);
            cells.Add(lastBackCell);
        }
    }

    private MapCell getMiddleMapCell()
    {
        return cells[requiredBackwardCells];
    }
}
