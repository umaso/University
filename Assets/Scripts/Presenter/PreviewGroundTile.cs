using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace UnityEngine.Tilemaps
{
	[Serializable]
	public class PreviewGroundTile : Tile
	{
		[SerializeField]
		public Sprite[] m_Sprites;

		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			for (int yd = -1; yd <= 1; yd++)
				for (int xd = -1; xd <= 1; xd++)
				{
					Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
					if (TileValue(tileMap, position))
						tileMap.RefreshTile(position);
				}
		}

		public override bool GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			UpdateTile(location, tileMap, ref tileData);
			return true;
		}

		private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;

            // 相邻tile的掩码位
            //             ?
            //         0   |   1
            //     ?    -- t --   ?
            //         3   |   2
            //             ?

			int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
			mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;

			int index = GetIndex((byte)mask);
			if (index >= 0 && index < m_Sprites.Length && TileValue(tileMap, location))
			{
				tileData.sprite = m_Sprites[index];
				tileData.transform = GetTransform((byte)mask);
				tileData.flags = (int) (TileFlags.LockTransform | TileFlags.LockColor);
			}
		}

		private bool TileValue(ITilemap tileMap, Vector3Int position)
		{
			TileBase tile = tileMap.GetTile(position);
			return (tile != null && tile == this);
		}

		private int GetIndex(byte mask)
		{
			switch (mask)
			{
                //单独
				case 0:
                    return 0;
                //仅左上或右上：单列下端点
				case 1: case 2:
                    return 1;
                //仅左下或右下：单列上端点
				case 4: case 8:
                    return 3;
                //左上和右上：方块下角
				case 3:
                    return 9;
                //左上右下或左下右上：单列中间
				case 5: case 10:
                    return 2;
                //右上右下或左上左下，方块左角
				case 6: case 9:
                    return 4;
                //左下右下：方块上角
				case 12:
                    return 6;
                //左上右上右下或左上右上左下：方块左下边
				case 7: case 11:
                    return 8;
                //左上左下右下或右上左下右下：方块左上边
				case 13: case 14:
                    return 5;
                //周围全有：中心
				case 15:
                    return 7;
			}
			Debug.Log(mask);
			return -1;
		}

		private Matrix4x4 GetTransform(byte mask)
		{
			switch (mask)
			{
				case 1:
				case 5:
				case 11:
				case 4:
				case 9:
				case 13:
					return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
			}
			return Matrix4x4.identity;
		}

#if UNITY_EDITOR
		[MenuItem("Assets/Create/PreviewGroundTile")]
		public static void CreateIsometricPipelineTile()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save PreviewGroundTile", "New PreviewGroundTile", "asset", "Save PreviewGroundTile", "Assets");

			if (path == "")
				return;

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PreviewGroundTile>(), path);
		}

		public override Sprite GetPreview()
		{
			if (m_Sprites != null && m_Sprites.Length > 0)
			{
				return m_Sprites[0];
			}
			return null;
		}
#endif
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(PreviewGroundTile))]
	public class PreviewGroundTileEditor : Editor
	{
		private PreviewGroundTile tile { get { return (target as PreviewGroundTile); } }

		public void OnEnable()
		{
			if (tile.m_Sprites == null || tile.m_Sprites.Length != 10)
				tile.m_Sprites = new Sprite[10];
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Place sprites shown based on the number of tiles bordering it.");
			EditorGUILayout.Space();
			
			EditorGUI.BeginChangeCheck();
			tile.m_Sprites[0] = (Sprite) EditorGUILayout.ObjectField("唯一", tile.m_Sprites[0], typeof(Sprite), false, null);
			tile.m_Sprites[1] = (Sprite) EditorGUILayout.ObjectField("单列左下", tile.m_Sprites[1], typeof(Sprite), false, null);
			tile.m_Sprites[2] = (Sprite) EditorGUILayout.ObjectField("单列中间", tile.m_Sprites[2], typeof(Sprite), false, null);
			tile.m_Sprites[3] = (Sprite) EditorGUILayout.ObjectField("单列右上", tile.m_Sprites[3], typeof(Sprite), false, null);
			tile.m_Sprites[4] = (Sprite) EditorGUILayout.ObjectField("方块左角", tile.m_Sprites[4], typeof(Sprite), false, null);
			tile.m_Sprites[5] = (Sprite) EditorGUILayout.ObjectField("方块左上边", tile.m_Sprites[5], typeof(Sprite), false, null);
			tile.m_Sprites[6] = (Sprite) EditorGUILayout.ObjectField("方块上角", tile.m_Sprites[6], typeof(Sprite), false, null);
			tile.m_Sprites[7] = (Sprite) EditorGUILayout.ObjectField("四周都有", tile.m_Sprites[7], typeof(Sprite), false, null);
			tile.m_Sprites[8] = (Sprite) EditorGUILayout.ObjectField("方块左下边", tile.m_Sprites[8], typeof(Sprite), false, null);
			tile.m_Sprites[9] = (Sprite) EditorGUILayout.ObjectField("方块下角", tile.m_Sprites[9], typeof(Sprite), false, null);

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(tile);
		}
	}
#endif
}
