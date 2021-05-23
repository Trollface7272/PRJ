using UnityEngine;
using UnityEngine.Tilemaps;

namespace World {
    public class MapUtils : MonoBehaviour {
        public static MapUtils Instance { get; private set; }
        private Tilemap _tilemap;
        public int width,height;
        private void Start() {
            Instance = GetComponent<MapUtils>();
            _tilemap = GetComponentInChildren<Tilemap>();
            var mg = MapGeneration.Instance;
            _tilemap = mg.map;
            width = mg.width;
            height = mg.height;
        }
        
    }
}
