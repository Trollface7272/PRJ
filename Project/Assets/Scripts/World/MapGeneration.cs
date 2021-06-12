using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using Entity.Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace World {
    public class MapGeneration : MonoBehaviour {
        public static MapGeneration Instance { get; private set; }
        [SerializeField] public Tilemap map;
        

        public int width;
        public int height;
        public float scale;
        [Range(0,100)]public float smoothness;
        
        private int _seed;
        private ItemListObject _itemList;
        private PlayerController _playerController;
        
        private void Start() {
            Instance = GetComponent<MapGeneration>();
            _seed = Random.Range(-1000000, 1000000);
            _playerController = PlayerController.Instance;
            _itemList = _playerController.player.itemList;
            GenerateMap();
        }

        private void Update() {
            return;
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                map.ClearAllTiles();
            } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                _seed = Random.Range(-1000000, 1000000);
                GenerateMap();
            }
        }

        private void GenerateMap() {
            GenerateDirt(); // Generate Dirt and stone footprint
            foreach (var ore in _itemList.ores) { // loop through all ores
                // Generate ore
                Generate(ore.block.tile, ore.spawnChance, ore.seedOffset, Vector2Int.RoundToInt(ore.spawn * height));
            }
            _playerController.TeleportToSpawn();
        }

        private void Generate(TileBase block, float chance, int offset, Vector2Int spawnYRange) {
            for (var i = 0; i < width; i++) { //Loop through entire width of the map
                for (var j = spawnYRange.x; j < spawnYRange.y; j++) { //Loop from generation start y to generation end y
                    var x = (float) (i + _seed + offset) / height * scale; //Generate X perlin coord
                    var y = (float) (j + _seed + offset) / height * scale; //Generate Y perlin coord
                    if (chance >= 0 && Mathf.PerlinNoise(x,y) > chance) continue; //Chance check
                    map.SetTile(new Vector3Int(i, height - j, 0), block);//Set block to ore
                }
            }
        }

        private void GenerateDirt() {
            Random.InitState(_seed);
            var stone = _itemList.FindTileByName("Stone").tile;//Get Stone Tile
            var dirt = _itemList.FindTileByName("Dirt").tile;  //Get Dirt Tile
            var grass = _itemList.FindTileByName("Grass").tile;//Get Grass Tile
            for (var x = 0; x < width; x++) { //Loop through entire width of the map
                var yMax = Mathf.RoundToInt (height + 25 * Mathf.PerlinNoise(x / smoothness, _seed)); //Topmost point for current X
                var minStoneSpawnDistance = yMax - 15; //Minimal Y for stone to spawn
                var maxStoneSpawnDistance = yMax - 12; //Maximal Y for stone to spawn
                var totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance); //Y for stone to spawn
                if (width / 2 == x) { //If we are in the middle of the map set player spawn position
                    var pos = map.CellToWorld(new Vector3Int(x, yMax+25, 0));
                    _playerController.SetSpawn(pos.x, pos.y);
                }
                
                for (var y = Mathf.Min(totalStoneSpawnDistance, 0); y < yMax; y++) {
                    map.SetTile(new Vector3Int(x, y, 0),
                        y < totalStoneSpawnDistance ? stone : dirt); 
                }
                
                map.SetTile(new Vector3Int(x, yMax, 0),
                    totalStoneSpawnDistance == yMax ? stone : grass);
            }
        }
    }
}
