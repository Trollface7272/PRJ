using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour {
    [SerializeField] public Tilemap map;
    
    [SerializeField] public Tile grass;
    [SerializeField] public Tile dirt;
    [SerializeField] public Tile stone;
    [SerializeField] public Tile coal;
    [SerializeField] public Tile iron;
    [SerializeField] public Tile diamond;
    
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public int currentY;
    [SerializeField] public int stoneStartY;

    private Camera _camera;

    private static MapController _instance;

    public static MapController Instance {
        get {
            if (_instance == null) _instance = FindObjectOfType<MapController>();
            return _instance;
        }
    }


    private void Start() {
        Generate();
        _camera = Camera.main;
    }

    public void EditBlock(Tile tile) {
        var pos = map.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
        //if (!HasNeighbour(pos.x, pos.y)) return;
        map.SetTile(pos, tile);
    }

    public Tile TileAtCursor() {
        return (Tile) map.GetTile(map.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition)));
    }


    private void Generate() {
        currentY = height;
        for (var x = 0; x < width; x++) {
            var minY = currentY - 1;
            var maxY = currentY + 2;
            currentY = Random.Range(minY, maxY);
            if (currentY < 0) currentY = 0;
            stoneStartY = Random.Range(12, 20);
            var stoneY = currentY - stoneStartY;
            for (var y = -height; y < stoneY-height; y++) {
                var randomAmount = Random.Range(1, 500);
                if(randomAmount <= 25 && randomAmount > 1) {
                    var oreChance = Random.Range(0, 100);
                    map.SetTile(Vector3Int.FloorToInt(new Vector3(x, y)), oreChance < 50 ? coal : iron);
                } else {
                    map.SetTile(Vector3Int.FloorToInt(new Vector3(x, y)), randomAmount == 1 ? diamond : stone);
                }
            }
 
            for (var y = stoneY-height; y < currentY-height; y++) {
                map.SetTile(Vector3Int.FloorToInt(new Vector3(x, y)), dirt);
            }
            
            map.SetTile(Vector3Int.FloorToInt(new Vector3(x, currentY-height)), grass);
        }
    }

    /*private bool HasNeighbour(int x, int y) {
        Debug.Log(x + " - " + y);
        if (x - 1 > 0 && tileArray[x - 1, y].sprite) 
            return true;
        if (y - 1 > 0 && tileArray[x, y - 1]) 
            return true;
        if (x + 1 < tileArray.GetLength(0) && tileArray[x + 1, y])
            return true;
        if (y + 1 < tileArray.GetLength(1) && tileArray[x, y + 1])
            return true;
        return false;
    }*/
}