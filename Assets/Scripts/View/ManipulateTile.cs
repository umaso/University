using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Zephyr.University.View
{
    public class ManipulateTile : MonoBehaviour
    {
        [SerializeField] private Tilemap _wallTileMap, _groundTileMap, _furnitureTileMap;
        [SerializeField] private TilemapRenderer _wallTileMapRenderer, _groundTileMapRenderer;

        [SerializeField] private TileBase _groundTileBase;

        private Tile _tile;

        // Use this for initialization
        private void Start()
        {
            _tile = _furnitureTileMap.GetTile<Tile>(Vector3Int.zero);

        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.N))
            {
                Debug.Log("New Tile!");
                _groundTileMap.SetTile(new Vector3Int(Random.Range(-10, 10), Random.Range(-5, 5), 0), _groundTileBase);
            }

            //if (Input.GetKey(KeyCode.W))
            //{
            //    _tile.gameObject.transform.Translate(Vector3.up * Time.deltaTime);
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    _tile.gameObject.transform.Translate(Vector3.down * Time.deltaTime);
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    _tile.gameObject.transform.Translate(Vector3.left * Time.deltaTime);
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    _tile.gameObject.transform.Translate(Vector3.right * Time.deltaTime);
            //}
        }
    }
}