﻿using System.IO;

namespace HomeWorkFunction4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[,] map;
        }

        static char[,] ReadMap(string mapName)
        {
            string[] newFile = File.ReadAllLines($"Maps/{mapName}");
        }
    }
}