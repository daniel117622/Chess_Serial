using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WhiteTile;
    public GameObject BlackTile;

    public static GameObject[] GameTiles = new GameObject[64];
    
    void Start()
    {
        char[] chPos = {'a','b','c','d','e','f','g','h'}; 
        

        for (int i = 0 ; i <= 63 ; i++)
        {
            string tileName = chPos[i%8].ToString() + ((int)Mathf.Round(i/8) + 1).ToString();
            var color = (int)Mathf.Round(i/8) + ((i % 8) + 1);
            if (color % 2 != 0)
            {
                GameObject tile = Instantiate(BlackTile,gameObject.GetComponent<Transform>(),false);
                tile.name = tileName;
                tile.GetComponent<Transform>().position = new Vector3(i%8,Mathf.Round(i/8),0);
                tile.AddComponent(typeof(TileBehaviour));
                tile.GetComponent<TileBehaviour>().tileId = (short)i;
                tile.AddComponent(typeof(BoxCollider));
                GameTiles[i] = tile;
            }
            else
            {
                GameObject tile = Instantiate(WhiteTile,gameObject.GetComponent<Transform>(),false);
                tile.name = tileName;
                tile.GetComponent<Transform>().position = new Vector3(i%8,Mathf.Round(i/8),0);
                tile.AddComponent(typeof(TileBehaviour));
                tile.GetComponent<TileBehaviour>().tileId = (short)i;
                tile.AddComponent(typeof(BoxCollider));
                GameTiles[i] = tile;                
            }

        }
    }

    public static void SetAllInactive()
    {
        foreach (var tile in GenerateBoard.GameTiles)
        {
            tile.GetComponent<TileBehaviour>().isActive = false;
        }
    }
}
