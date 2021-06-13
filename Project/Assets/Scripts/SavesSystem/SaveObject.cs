using System.Collections.Generic;
using Entity.Player;
using Hud;
using Inventory;
using Inventory.Items;
using UnityEngine;
using World;

namespace SavesSystem {
    [System.Serializable]
    public class SaveObject {
        public int[][] Inventory;
        public int[][] Armor;
        public int[][] Vanity;
        public int[] cursor;
        public byte activeSlot;
        public float[] spawnPoint;
        
        
        public int[][] Map;
        public int[][] Background;
        //public Vector3Int pos;
        //public BlockObject block;
        
        
        
        public SaveObject() {
            var player = PlayerController.Instance.player;
            var spawn = PlayerController.Instance.SpawnPoint;
            Inventory = InventoryObjectToData(player.inventory);
            Armor = InventoryObjectToData(player.armor);
            Vanity = InventoryObjectToData(player.vanity);
            cursor = SlotToData(player.cursor);
            activeSlot = player.activeSlot;
            spawnPoint = new[] {spawn.x, spawn.y, spawn.z};
            Map = BlocksToData(MapController.Instance.Blocks);
            Background = BlocksToData(MapController.Instance.Background);
        }

        private int[][] InventoryObjectToData(InventoryObject inv) {
            var res = new int[inv.length][];
            for (var i = 0; i < inv.length; i++) 
                res[i] = SlotToData(inv.items[i]);
            return res;
        }

        private int[] SlotToData(InventorySlot slot) {
            return !slot.item ? new[] {-1, -1} : new[] {slot.item.id, slot.count};
        }

        private int[][] BlocksToData(List<BlockData> blocks) {
            var res = new int[blocks.Count][];
            for (var i = 0; i < blocks.Count; i++) {
                res[i] = BlockToData(blocks[i]);
            }
            return res;
        }

        private int[] BlockToData(BlockData block) {
            return new []{block.Pos.x, block.Pos.y, block.Pos.z, block.Block == null ? -1 : block.Block.id};
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Load() {
            var player = PlayerController.Instance.player;
            var inv = DataToInventoryObject(Inventory);
            var arm = DataToInventoryObject(Armor);
            var van = DataToInventoryObject(Vanity);
            player.cursor = DataToSlot(cursor);
            player.activeSlot = activeSlot;
            PlayerController.Instance.SpawnPoint = new Vector3(spawnPoint[0], spawnPoint[1], spawnPoint[2]);
            var blocks = DataToBlocks(Map);
            var background = DataToBlocks(Background);
            var mCont = MapController.Instance;
            mCont.Blocks = blocks;
            mCont.Background = background;

            for (var i = 0; i < inv.Count; i++) player.inventory.items[i] = inv[i];
            for (var i = 0; i < arm.Count; i++) player.armor.items[i] = arm[i];
            for (var i = 0; i < van.Count; i++) player.vanity.items[i] = van[i];
            for (var i = 0; i < blocks.Count; i++) mCont.SetTile(blocks[i].Pos, blocks[i].Block);
            for (var i = 0; i < background.Count; i++) mCont.SetBackground(background[i].Pos, background[i].Block);
            HudControler.Instance.UpdateHud();
            PlayerController.Instance.TeleportToSpawn();
        }


        private List<InventorySlot> DataToInventoryObject(int[][] data) {
            var res = new List<InventorySlot>();
            for (var i = 0; i < data.Length; i++) {
                res.Add(DataToSlot(data[i]));
            }
            return res;
        }
        
        private InventorySlot DataToSlot(int[] data) {
            var id = data[0];
            if (id < 1) return new InventorySlot(null, 0);
            id--;
            return new InventorySlot(PlayerController.Instance.player.itemList.items[id], data[1]);;
        }

        private List<BlockData> DataToBlocks(int[][] data) {
            var res = new List<BlockData>();
            for (var i = 0; i < data.Length; i++) {
                res.Add(DataToBlock(data[i]));
            }

            return res;
        }

        private BlockData DataToBlock(int[] data) {
            BlockObject block;
            if (data[3] == -1) block = null;
            else block = PlayerController.Instance.player.itemList.items[data[3] - 1] as BlockObject;
            return new BlockData(new Vector3Int(data[0], data[1], data[2]), block);
        }
    }
}
