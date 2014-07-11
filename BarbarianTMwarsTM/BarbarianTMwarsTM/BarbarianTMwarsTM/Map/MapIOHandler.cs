using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarbarianTMwarsTM.MapClasses
{
    abstract class MapIOHandler
    {
        void SaveGame(Map saveMap, string saveName)
        {
            //create the save file
            System.IO.StreamWriter save = new System.IO.StreamWriter("/save/" + saveName +".sav");

            

            //give the savefile a header of the type
            // <MAP>15×10 (width×length)

            save.Write("<MAP> ");
            save.Write(saveMap.tileSet.Tiles.GetLength(0)+ "×"+ saveMap.tileSet.Tiles.GetLength(1));

            //for each Tile on the map to be saved, write a character, one line of text per row
                // ~ = sea
                // ^ = mountain
                // . = plain
                // ? = forest

            for (int i = 0; i < saveMap.tileSet.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < saveMap.tileSet.Tiles.GetLength(1); j++)
                {
                    if (saveMap.tileSet.Tiles[i, j].tileType == TileType.Sea)
                    {
                        save.Write("~ ");
                    }
                    if (saveMap.tileSet.Tiles[i, j].tileType == TileType.Mountain)
                    {
                        save.Write("^ ");
                    }
                    if (saveMap.tileSet.Tiles[i, j].tileType == TileType.Plains)
                    {
                        save.Write(". ");
                    }
                    if (saveMap.tileSet.Tiles[i, j].tileType == TileType.Forest)
                    {
                        save.Write("Y ");
                    }
                    if (saveMap.tileSet.Tiles[i, j].tileType == TileType.Road)
                    {
                        save.Write("_ ");
                    }
                }
                save.WriteLine();
            }
            save.Close();
        }

        void LoadGame(string saveName)
        {
            System.IO.StreamWriter save = new System.IO.StreamWriter("/save/" + saveName + ".sav");
            
            //temp, later weer verder, void moet naar "Map" voor return type
        }
    }
}
