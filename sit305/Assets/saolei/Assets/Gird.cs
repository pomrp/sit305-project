using UnityEngine;
using System.Collections;

public class Grid
{
    // The Grid itself
    public static int w = 5; // this is the width
    public static int h = 4; // this is the height
    public static Elemet[,] elements = new Elemet[w, h];

    public static void uncoverMines()
    {
        foreach (Elemet elem in elements)
            if (elem.mine)
                elem.loadTexture(0);
    }

    public static bool minAt(int x, int y) {
        if (x >= 0 && y >= 0 && x < w && y < h)
            return elements[x, y].mine;
        return false;
    }

    public static int adjacentMines(int x, int y) {
        int count = 0;
        if (minAt(x, y + 1)) ++count;
        if (minAt(x+1, y + 1)) ++count;
        if (minAt(x+1, y)) ++count;
        if (minAt(x+1, y - 1)) ++count;
        if (minAt(x, y - 1)) ++count;
        if (minAt(x-1, y - 1)) ++count;
        if (minAt(x-1, y)) ++count;
        if (minAt(x-1, y + 1)) ++count;
        return count;
    }

    public static void FFuncover(int x, int y, bool[,] visited)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            if (visited[x, y])
                return;

            elements[x, y].loadTexture(adjacentMines(x, y));

            if (adjacentMines(x, y) > 0)
                return;

            visited[x, y] = true;

            FFuncover(x - 1, y, visited);
            FFuncover(x + 1, y, visited);
            FFuncover(x, y - 1, visited);
            FFuncover(x, y + 1, visited);

        }
    }

    public static bool isFinished() {
        foreach (Elemet elem in elements)
            if (elem.isCovered() && !elem.mine)
                return false;
        return true;

    }
}