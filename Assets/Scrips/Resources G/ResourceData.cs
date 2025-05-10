using UnityEngine;

[CreateAssetMenu(fileName= "NewResource", menuName = "Movil/newResourseM", order = 1)]

public class ResourceData : ScriptableObject
{
    public string resourceID;
    public string resourceName;
    public Sprite icon;
    public int defaultAmount;
}