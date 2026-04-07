using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class Obstacle : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; set; }
    [field: SerializeField] public bool Passable { get; set; } = false;
    [field: SerializeField] public bool HigherThanPlayer { get; set; } = true;
    [field: SerializeField] public float SpeedModifier { get; set; } = 0; //only makes sense to be set when passable is true or if you wanna go fance you can make "Sticky" obstacles
    [field: SerializeField] public bool Movable { get; set; }
    [field: SerializeField] public bool Destroyable { get; set; }
    [field: SerializeField] public bool Explosive { get; set; }
    [field: SerializeField] public float ExplodeRange { get; set; }
    [field: SerializeField] public int ExplodeDamage { get; set; }
    [field: SerializeField] public bool Carnivore { get; set; }
    [field: SerializeField] public bool Herbivore { get; set; }
    [field: SerializeField] public int HP { get; set; } //only makes sense when its Destroyable or is Edible AKA Herbivore/Carnivore
    [field: SerializeField] public Enums.ObstacleSize size { get; set; }
    [field: SerializeField] public GameObject ObstaclePrefab { get; set; }
    [field: SerializeField] public GameObject ObstacleExplosionPrefab { get; set; }


    private float GetTargetUnitWidth()
    {
        return size switch
        {
            Enums.ObstacleSize.Small => 1f,
            Enums.ObstacleSize.Medium => 2f,
            Enums.ObstacleSize.Large => 4f,
            _ => 1f
        };
    }

    public Vector3 GetRequiredScale()
    {
        float spriteWidth = Sprite.bounds.size.x;
        float targetWidth = GetTargetUnitWidth();
        float scaleFactor = targetWidth / spriteWidth;
        return new Vector3(scaleFactor, scaleFactor, 1f);
    }
    public float GetWorldRadius()
    {
        float targetWidth = GetTargetUnitWidth();
        return (targetWidth / 2f) * 1.414f;
    }
    private void Reset()
    {
        string path = "Assets/Prefabs/Rooms/Obstacle.prefab";
        ObstaclePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        string explodePath  = "Assets/Prefabs/Rooms/ObstacleExplodeVFX.prefab";
        ObstacleExplosionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(explodePath);
    }
}
