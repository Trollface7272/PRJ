using System;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World {
    public class MapController : MonoBehaviour {
        private static MapController _instance;

        public static MapController Instance {
            get {
                if (!_instance) _instance = FindObjectOfType<MapController>();
                return _instance;
            }
        }

        [SerializeField] private Tilemap map;
        [SerializeField] private Tilemap nonSolid;
        private Camera _camera;

        public List<BlockData> Blocks;
        public List<BlockData> Background;

        private void Start() {
            _camera = Camera.main;
        }

        public int CursorHasTileOrBackground() {
            var pos = map.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
            if (map.GetTile(pos) != null) return 2;
            if (nonSolid.GetTile(pos) != null) return 1;
            return 0;
        }
        public Tile TileAtCursor() {
            var pos = map.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
            var tile = (Tile) map.GetTile(pos);
            if (tile != null) return tile;
            return (Tile) nonSolid.GetTile(pos);
        }

        public bool EditBlock(BlockObject block, bool force) {
            var pos = map.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
            if (!force && HasTile(pos, map)) return false;
            if (!force && !IsConnected(pos)) return false;
            Blocks ??= MapGeneration.Instance.Blocks;
            Blocks.Add(new BlockData(pos, block));
            map.SetTile(pos, block == null ? null : block.tile);
            return true;
        }
        
        public bool EditNonSolid(BlockObject block, bool force) {
            var pos = nonSolid.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
            if (!force && HasTile(pos, nonSolid)) return false;
            Background ??= MapGeneration.Instance.Background;
            Background.Add(new BlockData(pos, block));
            nonSolid.SetTile(pos, block == null ? null : block.tile);
            return true;
        }

        private bool HasGround(Vector3Int pos) {
            pos.y += 1;
            return HasTile(pos, nonSolid);
        }

        private bool IsConnected(Vector3Int pos) {
            if (HasTile(pos.x+1, pos.y, pos.z, map)) return true;
            if (HasTile(pos.x-1, pos.y, pos.z, map)) return true;
            if (HasTile(pos.x, pos.y+1, pos.z, map)) return true;
            if (HasTile(pos.x, pos.y-1, pos.z, map)) return true;
            return false;
        }

        private static bool HasTile(int x, int y, int z, Tilemap m) {
            return m.GetTile(new Vector3Int(x,y,z)) != null;
        }

        private static bool HasTile(Vector3Int pos, Tilemap m) {
            return m.GetTile(pos) != null;
        }
        
        public void SetTile(Vector3Int pos, BlockObject block) {
            map.SetTile(pos, block ? block.tile : null);
        }
        public void SetBackground(Vector3Int pos, BlockObject block) {
            nonSolid.SetTile(pos, block ? block.tile : null);
        }
    }
}