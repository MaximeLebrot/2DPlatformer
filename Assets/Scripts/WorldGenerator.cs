using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{

    public int mapSizeY;
    public int mapSizeX;
    public Tilemap Foreground;
    public Tile[] tiles;

    public int minHeight = 2;

    // Start is called before the first frame update
    public void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        int startHeight = 5;

        for (int i = 0; i < mapSizeX; i++)
        {
            int randomWalk = Random.Range(-1, 2);

            if(startHeight >= minHeight)
            {     
                startHeight += randomWalk;
            }
            else if( randomWalk != -1)
            {
                startHeight += randomWalk;
            }          
            for (int n = 0; n < startHeight; n++)
            {
                Foreground.SetTile(new Vector3Int(i,n, 0), tiles[0]);
            }
        }
    }
}
