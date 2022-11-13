using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;
using System.Runtime.CompilerServices;

public class MapDataExtractor 
{
    // % = ctrl, # = shift, & alt
    [MenuItem("Tools/ExtractMap %&e")] 
    private static void ExtractMapData()
    {
        GameObject tilemap = GameObject.Find("Tilemap");

        if(tilemap == null)
        {
            Debug.LogError("There is no tilemap in hierarchy");
            return;
        }

        Tilemap tmCollision = tilemap.transform.Find("Collision").GetComponent<Tilemap>();
        Tilemap tmSafezone = tilemap.transform.Find("SafeZone").GetComponent<Tilemap>();

        tmCollision.CompressBounds();
        tmSafezone.CompressBounds();

        using (StreamWriter writer = File.CreateText($"Assets/Resources/Map/{tilemap.name}.txt"))
        {
            BoundsInt mapBounds = tmCollision.cellBounds;

            writer.WriteLine(mapBounds.xMin);
            writer.WriteLine(mapBounds.xMax);
            writer.WriteLine(mapBounds.yMin);
            writer.WriteLine(mapBounds.yMax);

            for (int y = mapBounds.yMax - 1; y >= mapBounds.yMin; y--)
            {
                for (int x = mapBounds.xMin; x < mapBounds.xMax; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    TileBase tile = tmCollision.GetTile(tilePos);
                    TileBase safetile = tmSafezone.GetTile(tilePos);

                    if (tile != null)
                    {
                        writer.Write("1");
                    }

                    else if(safetile != null)
                    {
                        writer.Write("2");
                    }

                    else
                    {
                        writer.Write("0");
                    }
                }

                writer.WriteLine("");
            }
        }
    }
    
}
