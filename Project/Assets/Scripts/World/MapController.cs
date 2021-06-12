using System;
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

        private void Start() {
            _camera = Camera.main;
        }


        public Tile TileAtCursor() {
            return (Tile) map.GetTile(map.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition)));
        }

        public bool EditBlock(Tile tile, bool force) {
            var pos = map.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
            if (!force && HasTile(pos, map)) return false;
            if (!IsConnected(pos)) return false;
            map.SetTile(pos, tile);
            return true;
        }
        
        public bool EditNonSolid(Tile tile, bool force) {
            var pos = nonSolid.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));
            if (!force && HasTile(pos, nonSolid)) return false;
            if (!HasGround(pos)) return false;
            nonSolid.SetTile(pos, tile);
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
    }
}