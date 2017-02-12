using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ManipulateTile : MonoBehaviour
{
    [SerializeField]
    private Tilemap _wallTileMap, _groundTileMap;
    [SerializeField]
    private TilemapRenderer _wallTileMapRenderer, _groundTileMapRenderer;

    [SerializeField] private TileBase _groundTileBase;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyUp(KeyCode.N))
	    {
	        Debug.Log("New Tile!");
            _groundTileMap.SetTile(new Vector3Int(Random.Range(-10, 10), Random.Range(-5, 5), 0), _groundTileBase);
        }
	}
}
