using UnityEngine;

public struct Cell
{
    public enum Type // her bir karonun alabileceği değerler.
    {
        Invalid,
        Empty,
        Mine,
        Number,
    }

    public Vector3Int position;  // Karorunun Tilemap üzerindeki konumu.
    public Type type;

    public int number;  // Eğer tip Number ise bu değer 1-8 arası bir sayı olur.

    public bool revealed; // Karonun açılıp açılmadığını tutar.
    public bool flagged; // Karonun işaretlenip işaretlenmediğini tutar
    public bool exploaded; // Mayın   patladı mı onu tutar


}
