using Entity.ObjectLists;
using Entity.Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace World {
    public class MapGeneration : MonoBehaviour {
        public static MapGeneration Instance { get; private set; }
        [SerializeField] public Tilemap map;
        [SerializeField] private Tiles tiles;

        public int width;
        public int height;
        public float scale;
        [Range(0,100)]public float smoothness;

        [SerializeField] [Range(0, 1f)] private float diamondSpawnChance;
        [SerializeField] [Range(0, 1f)] private float goldSpawnChance;
        [SerializeField] [Range(0, 1f)] private float ironSpawnChance;
        [SerializeField] [Range(0, 1f)] private float coalSpawnChance;

        
        private int _seed;
        private const int DiamondOffset = 250000;
        private const int GoldOffset    = -250000;
        private const int IronOffset    = 125000;
        private const int CoalOffset    = -125000;
        

        private void Start() {
            Instance = GetComponent<MapGeneration>();
            _seed = Random.Range(-1000000, 1000000);
            GenerateMap();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                map.ClearAllTiles();
            } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                _seed = Random.Range(-1000000, 1000000);
                GenerateMap();
            }
        }

        private void GenerateMap() {
            Generate(tiles.items[2].tile, -1, 0, new Vector2Int(0, height));
            GenerateDirt();
            Generate(tiles.items[3].tile, coalSpawnChance, CoalOffset, new Vector2Int(height / 10, height - height / 10));
            Generate(tiles.items[4].tile, ironSpawnChance, IronOffset, new Vector2Int(height / 10, height - height / 10));
            Generate(tiles.items[5].tile, goldSpawnChance, GoldOffset, new Vector2Int(height - height / 2, height - height / 10));
            Generate(tiles.items[6].tile, diamondSpawnChance, DiamondOffset, new Vector2Int(height - height / 10, height));
            
        }

        private void Generate(TileBase block, float chance, int offset, Vector2Int spawnYRange) {
            for (var i = 0; i < width; i++) {
                for (var j = spawnYRange.x; j < spawnYRange.y; j++) {
                    var x = (float) (i + _seed + offset) / height * scale;
                    var y = (float) (j + _seed + offset) / height * scale;
                    if (chance >= 0 && Mathf.PerlinNoise(x,y) > chance) continue;
                    map.SetTile(new Vector3Int(i, j, 0), block);
                }
            }
        }

        private void GenerateDirt() {
            for (var x = 0; x < width; x++) {
                var xMax = Mathf.RoundToInt (width + 25 * Mathf.PerlinNoise(x / smoothness, _seed));
                var minStoneSpawnDistance = xMax - 15;
                var maxStoneSpawnDistance = xMax - 12;
                var totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);
                if (width / 2 == x) {
                    var pos = map.CellToWorld(new Vector3Int(x, xMax+5, 0));
                    PlayerController.Instance.SetSpawn(pos.x, pos.y);
                    PlayerController.Instance.TeleportToSpawn();
                }
                for (var y = Mathf.Min(totalStoneSpawnDistance, 0); y < xMax; y++) {
                    map.SetTile(new Vector3Int(x, y, 0),
                        y < totalStoneSpawnDistance ? tiles.items[2].tile : tiles.items[1].tile);
                }

                map.SetTile(new Vector3Int(x, xMax, 0),
                    totalStoneSpawnDistance == xMax ? tiles.items[2].tile : tiles.items[0].tile);
            }
        }
    }
}
