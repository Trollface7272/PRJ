using System.Collections.Generic;
using Inventory;
using Inventory.Items;
using Entity.Player;
using SavesSystem;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace World {
    public class MapGeneration : MonoBehaviour {
        public static MapGeneration Instance { get; private set; }
        [SerializeField] public Tilemap map;
        [SerializeField] public Tilemap backgroundMap;
        

        public int width;
        public int height;
        public float scale;
        public int minTreeDistance = 6;
        public int maxTreeDistance = 12;
        [Range(0,100)]public float smoothness;
        
        private int _seed;
        private ItemListObject _itemList;
        private PlayerController _playerController;

        public List<BlockData> Blocks;
        public List<BlockData> Background;
        
        private void Start() {
            Instance = GetComponent<MapGeneration>();
            _playerController = PlayerController.Instance;
            _itemList = _playerController.player.itemList;
            if (SaveController.LoadOnStart) return;
            Blocks = new List<BlockData>();
            Background = new List<BlockData>();
            _seed = Random.Range(-1000000, 1000000);
            GenerateMap();
            MapController.Instance.Blocks = Blocks;
            MapController.Instance.Background = Background;
        }

        /*private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                map.ClearAllTiles();
            } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                _seed = Random.Range(-1000000, 1000000);
                GenerateMap();
            }
        }*/

        private void GenerateMap() {
            GenerateDirt(); // Generate Dirt and stone footprint
            foreach (var ore in _itemList.ores) { // loop through all ores
                // Generate ore
                Generate(ore.block, ore.spawnChance, ore.seedOffset, Vector2Int.RoundToInt(ore.spawn * height));
            }
            _playerController.TeleportToSpawn();
        }

        private void Generate(BlockObject block, float chance, int offset, Vector2Int spawnYRange) {
            for (var i = 0; i < width; i++) { //Loop through entire width of the map
                for (var j = spawnYRange.x; j < spawnYRange.y; j++) { //Loop from generation start y to generation end y
                    var x = (float) (i + _seed + offset) / height * scale; //Generate X perlin coord
                    var y = (float) (j + _seed + offset) / height * scale; //Generate Y perlin coord
                    if (chance >= 0 && Mathf.PerlinNoise(x,y) > chance) continue; //Chance check
                    SetTile(new Vector3Int(i, height - j, 0), block);//Set block to ore
                }
            }
        }

        private void GenerateDirt() {
            Random.InitState(_seed);
            var stone = _itemList.FindTileByName("Stone");//Get Stone Tile
            var dirt = _itemList.FindTileByName("Dirt");  //Get Dirt Tile
            var grass = _itemList.FindTileByName("Grass");//Get Grass Tile
            var treeDist = 0;
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
                    SetTile(new Vector3Int(x, y, 0),
                        y < totalStoneSpawnDistance ? stone : dirt);
                }
                
                SetTile(new Vector3Int(x, yMax, 0),
                    totalStoneSpawnDistance == yMax ? stone : grass);

                if ((Random.Range(0, 20) >= 18 && (treeDist > minTreeDistance)) || treeDist > maxTreeDistance) {
                    treeDist = 0;
                    GenerateTree(new Vector3Int(x, yMax + 1, 0));
                } else treeDist++;
            }
        }
        
        private void GenerateTree(Vector3Int basePos) {
            var treeHeight = Random.Range(8, 16);
            var leavesSpawnHeight = treeHeight - Random.Range(5, 6);
            var wood = _itemList.FindTileByName("Wood");
            var leaves = _itemList.FindTileByName("Leaves");
            for (var i = 0; i < treeHeight; i++) {
                if (i > leavesSpawnHeight) {
                    for (var j = 0; j < 3; j++) {
                        SetBackgroundTile(new Vector3Int(basePos.x + j + 1, basePos.y, basePos.z), leaves);
                        SetBackgroundTile(new Vector3Int(basePos.x - j - 1, basePos.y, basePos.z), leaves);
                    }
                }
                SetBackgroundTile(basePos, wood);
                basePos.y++;
            }
            SetBackgroundTile(basePos, leaves);
            basePos.x += 1;
            SetBackgroundTile(basePos, leaves);
            basePos.x -= 2;
            SetBackgroundTile(basePos, leaves);
        }
        
        private void SetTile(Vector3Int pos, BlockObject block) {
            Blocks.Add(new BlockData(pos, block));
            map.SetTile(pos, block.tile);
        }
        
        private void SetBackgroundTile(Vector3Int pos, BlockObject block) {
            if (backgroundMap.GetTile(pos) != null || map.GetTile(pos) != null) return;
            Background.Add(new BlockData(pos, block));
            backgroundMap.SetTile(pos, block.tile);
        }
        
    }

    public class BlockData {
        public Vector3Int Pos;
        public BlockObject Block;

        public BlockData(Vector3Int p, BlockObject b) {
            Pos = p;
            Block = b;
        }
    }
}
