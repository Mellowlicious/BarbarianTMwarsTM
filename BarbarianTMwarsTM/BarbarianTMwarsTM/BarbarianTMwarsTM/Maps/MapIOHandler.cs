using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BarbarianTMwarsTM.Units;
using BarbarianTMwarsTM.Maps;
using BarbarianTMwarsTM;

namespace BarbarianTMwarsTM.Maps
{
    abstract class MapIOHandler
    {
        public static void SaveGame(Map saveMap, string saveName)
        {
            //create the directory if it does not exist
            if (!System.IO.Directory.Exists("save")) System.IO.Directory.CreateDirectory("save");

            //create the save file
            System.IO.StreamWriter save = new System.IO.StreamWriter("save\\" + saveName +".sav");

            

            //give the savefile a header of the type
            // <MAP> 15×10 (width×length)

            save.Write("<MAP> ");
            save.WriteLine(saveMap.tileSet.Tiles.GetLength(0)+ "×"+ saveMap.tileSet.Tiles.GetLength(1));

            //for each Tile on the map to be saved, write a character, one line of text per row
                // ~ = sea
                // ^ = mountain
                // . = plain
                // ? = forest
                // _ = road

            for (int j = 0; j < saveMap.tileSet.Tiles.GetLength(1); j++)
            {
                for (int i = 0; i < saveMap.tileSet.Tiles.GetLength(0); i++)
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

            save.WriteLine("<BUILDINGS> ");
            // Write all the buildings to the save, on seperate lines of the form [<Player> <Type> (X,Y)]
            // CODE GOES HERE LOL
            save.WriteLine("</BUILDINGS> ");

            save.WriteLine("<UNITS> ");
            // Write all the units to the save, on seperate lines of the form [<Player> <Faction> <Type> (X,Y)]
            foreach (List<Unit> unitList in saveMap.listOfUnits)
            {
                foreach (Unit unit in unitList)
                save.WriteLine(unit.ControllingPlayer + " " +/* FACTION + " " + */ unit.unitType + " " + unit.Position.X + " " + unit.Position.Y);
            }
            
            save.WriteLine("</UNITS> ");

            save.Close();
        }

        public static void LoadGame(Map map, string saveName) //Moet Map map returnen
        {
            System.IO.StreamReader save = new System.IO.StreamReader("save\\" + saveName + ".sav");

            //Read the first line, and attempt to make a TileSet with width and length specified
            string mapSize = save.ReadLine();
            if (mapSize.Split()[1] == null) /* een error throwen en shit*/Console.WriteLine("File Error");
            int tilesetWidth = Convert.ToInt32(mapSize.Split()[1].Split('×')[0]);
            int tilesetLength = Convert.ToInt32(mapSize.Split()[1].Split('×')[1]);
            map.tileSet.Tiles = new Tile[tilesetWidth, tilesetLength];

            //Fill the tileset according to the characters in the save
                // ~ = sea
                // ^ = mountain
                // . = plain
                // ? = forest
                // _ = road

            for (int j = 0; j < map.tileSet.Tiles.GetLength(1); j++)
            {
                
                string[] tileRow = save.ReadLine().Split();

                for (int i = 0; i < map.tileSet.Tiles.GetLength(0); i++)
                {
                    Console.Write(tileRow[i]);
                    if (tileRow[i] == "~") map.tileSet.Tiles[i, j] = new Tile(TileType.Sea, SpriteType.Sea);
                    if (tileRow[i] == "^") map.tileSet.Tiles[i, j] = new Tile(TileType.Mountain, SpriteType.Mountain);
                    if (tileRow[i] == ".") map.tileSet.Tiles[i, j] = new Tile(TileType.Plains, SpriteType.Plains);
                    if (tileRow[i] == "?") map.tileSet.Tiles[i, j] = new Tile(TileType.Forest, SpriteType.Forest);
                    if (tileRow[i] == "_") map.tileSet.Tiles[i, j] = new Tile(TileType.Road, SpriteType.Road);
                }
                Console.WriteLine();
            }

            //Maak de array unitpositions aan in map
            map.unitPositions = new Unit[map.tileSet.Tiles.GetLength(0), map.tileSet.Tiles.GetLength(1)];
            for (int j = 0; j < map.unitPositions.GetLength(1); j++)
            {
                for (int i = 0; i < map.unitPositions.GetLength(0); i++)
                {
                    map.unitPositions[i, j] = null;
                }
            }

            
            if (save.ReadLine().Equals("<BUILDINGS>"))
                Console.WriteLine("File Error Type 2");
            else Console.WriteLine("<BUILDINGS>");
            save.ReadLine(); // temp totdat er ook echt buildings bestaan

            Console.WriteLine(save.ReadLine()); // de regel <units> er uit filteren


            // Alle units er in zetten naar hun data zoals gesaved in SaveGame
            string[] unitData = save.ReadLine().Split();
            while (!unitData[0].Equals("</UNITS>"))
            {
                map.listOfUnits[0].Add(new Unit(map, UnitTypeEnum.Militia, map.unitPlaceholder, new Point(Convert.ToInt32(unitData[2]), Convert.ToInt32(unitData[3])), Convert.ToInt32(unitData[0]), false));
                unitData = save.ReadLine().Split();
            }

            Console.WriteLine("Succesfully loaded file");
            save.Close();

        }
    }
}
