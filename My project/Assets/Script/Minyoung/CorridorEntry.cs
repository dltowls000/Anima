using UnityEngine;

[CreateAssetMenu(fileName = "NewAnima", menuName = "Corridor/Anima")]
public class CorridorEntry : ScriptableObject 
{
    public string animaName;
    public Sprite colorImage;
    //public Sprite sillhouetteImage;  //흑백사진 사용용도
    public string description;

}
