﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Vaults {
	public enum VaultType {
		Entrance,
		Exit,
		Floating,
		Configured
	}
		
	public enum TileType {
		Empty,
		Wall,
		Lava,
		Spike
	}
		
	public class Vault : MonoBehaviour{
		public string name;
		public Coordinates size;
		public string csv;
		public VaultType type = VaultType.Floating;
		public List<TiledObject> objects = new List<TiledObject>();
		public int minDepth, maxDepth;

		public TileType[,] tiles { get; set;}
		public uint[,] tileInts;

		public void ParseCSV() {

			const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
			const uint FLIPPED_VERTICALLY_FLAG   = 0x40000000;
			const uint FLIPPED_DIAGONALLY_FLAG   = 0x20000000;

			tiles = new TileType[size.x, size.y];
			tileInts = new uint[size.x, size.y];
			int i = 0;
			string[] tileIds = csv.Split (',');
			List<uint> values = new List<uint>();
			foreach (string id in tileIds) {
				values.Add (uint.Parse (id, System.Globalization.NumberFormatInfo.InvariantInfo));
			}

			uint[] tileData = values.ToArray ();

			for (int y = 0; y < size.y; y++) {
				for (int x = 0; x < size.x; x++) {
					uint c = tileData [i];
					int tile = (int)(c & ~(0xE0000000)); // ignore flipping and rotating
					tileInts[x, y] = c;


					switch (tile) {
					case 5:
					case 6:
					case 7:
					case 8:
						tiles [x, y] = TileType.Wall;
						break;
					case 9:
						tiles [x, y] = TileType.Lava;
						break;
					case 11: 
						tiles [x, y] = TileType.Spike;
						break;
					default:
						tiles [x, y] = TileType.Empty;
						break;
					}
					i++;
				}
			}
		}

		public void ConvertVaultToTK2D() {
			TileType[,] newTiles =  new TileType[size.y, size.x];
			newTiles = Utils.Utils.TransposeMatrix (tiles);
			int newX = tiles.GetLength (0);
			int newY = tiles.GetLength (1);
			size.x = newX;
			size.y = newY;
			tiles = newTiles;
		}
	}
}