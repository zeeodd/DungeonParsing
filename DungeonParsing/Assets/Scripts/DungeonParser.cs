using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonParser : MonoBehaviour
{
    #region Variables
    // Text Data Variables
    public TextAsset data;
    string[] linesFromfile;

    // Parsing Variables
    private int dungeonWidth;
    private int dungeonHeight;
    private int totalEmpty;
    List<Vector2> visitedTiles = new List<Vector2>();

    bool orthoBool = false;
    bool diagBool = false;
    bool traverseBool = true;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        linesFromfile = data.text.Split("\n"[0]);
        dungeonWidth = linesFromfile[0].Length;
        dungeonHeight = linesFromfile.Length;

        // Find the number of completely empty tiles
        for (int i = 0; i < dungeonHeight; i++)
        {
            for (int j = 0; j < dungeonWidth; j++)
            {
                if (linesFromfile[i][j] == ' ')
                {
                    totalEmpty++;
                }
            }
        }
    }

    void Start()
    {
        orthoBool = ParseDungeon(false);
        diagBool = ParseDungeon(true);

        Debug.Log("This map can be traversed orthogonally: " + orthoBool);
        Debug.Log("This map can be traversed orthogonally & diagonally: " + diagBool);
        
        if (orthoBool || diagBool)
        {
            traverseBool = false;
        }

        Debug.Log("This map can't be traversed at all: " + traverseBool);
    }
    #endregion

    private bool ParseDungeon(bool diagToggle)
    {
        // Instantiate a list of visited tiles and a stack of unexplored ones
        // A stack is used here becuase we want to keep looking at the closest tile (DFS)
        List<Vector2> visitedTiles = new List<Vector2>();
        Stack<Vector2> unexploredTiles = new Stack<Vector2>();

        // Get a random blank tile and add it to the unexplored tiles stack
        unexploredTiles.Push(GetRandomBlankTile());

        // Keep running until all tiles have been explored
        while (unexploredTiles.Count > 0)
        {
            Vector2 tile = unexploredTiles.Pop();

            if (visitedTiles.Contains(tile))
            {
                continue;
            }

            visitedTiles.Add(tile);

            foreach (Vector2 neighbor in GetNeighbors(tile, diagToggle))
            {
                if (!visitedTiles.Contains(neighbor))
                {
                    unexploredTiles.Push(neighbor);
                }
            }
        }

        return totalEmpty == visitedTiles.Count;
    }

    private Vector2 GetRandomBlankTile()
    {
        bool isBlank = false;
        Vector2 blankTile = Vector2.zero;

        while (!isBlank)
        {
            Vector2 randomTile = new Vector2(Random.Range(0, dungeonWidth - 1), Random.Range(0, dungeonHeight - 1));
            if (linesFromfile[(int)randomTile.y][(int)randomTile.x] == ' ')
            {
                blankTile = randomTile;
                isBlank = true;
            }
        }

        return blankTile;
    }

    private bool CheckIfBlankTile(Vector2 tile)
    {
        //Debug.Log(tile);

        bool returnbool = false;
        if (linesFromfile[(int)tile.y][(int)tile.x] == ' ')
        {
            returnbool = true;
        }

        return returnbool;
    }

    private List<Vector2> GetNeighbors(Vector2 tile, bool diagToggle)
    {
        List<Vector2> neighbors = new List<Vector2>();
        bool top = true;
        bool left = true;
        bool right = true;
        bool bot = true;

        // Check if ortho neighbors exist or not. Return false if one doesn't exist.
        if (tile.y - 1 < 0)
        {
            top = false;
        }
        if (tile.y + 1 > dungeonHeight - 1)
        {
            bot = false;
        }
        if (tile.x - 1 < 0)
        {
            left = false;
        }
        if (tile.x + 1 > dungeonWidth - 1)
        {
            right = false;
        }

        if (top == true)
        {
            Vector2 topTile = new Vector2(tile.x, tile.y - 1);
            if (CheckIfBlankTile(topTile))
            {
                neighbors.Add(topTile);
            }
        }

        if (bot == true)
        {
            Vector2 botTile = new Vector2(tile.x, tile.y + 1);
            if (CheckIfBlankTile(botTile))
            {
                neighbors.Add(botTile);
            }
        }

        if (left == true)
        {
            Vector2 leftTile = new Vector2(tile.x - 1, tile.y);
            if (CheckIfBlankTile(leftTile))
            {
                neighbors.Add(leftTile);
            }
        }

        if (right == true)
        {
            Vector2 rightTile = new Vector2(tile.x + 1, tile.y);
            if (CheckIfBlankTile(rightTile))
            {
                neighbors.Add(rightTile);
            }
        }

        if (diagToggle)
        {
            bool topleft = true;
            bool topright = true;
            bool botleft = true;
            bool botright = true;

            // Check if diag neighbors exist or not. Return false if one doesn't exist.
            if (tile.y - 1 < 0 || tile.x - 1 < 0)
            {
                topleft = false;
            }
            if (tile.y - 1 < 0 || tile.x + 1  > dungeonWidth - 1)
            {
                topright = false;
            }
            if (tile.x - 1 < 0 || tile.y + 1 > dungeonHeight - 1)
            {
                botleft = false;
            }
            if (tile.x + 1 > dungeonWidth - 1 || tile.y + 1 > dungeonHeight - 1)
            {
                botright = false;
            }

            if (topleft == true)
            {
                Vector2 tlTile = new Vector2(tile.x - 1, tile.y - 1);
                if (CheckIfBlankTile(tlTile))
                {
                    neighbors.Add(tlTile);
                }
            }

            if (topright == true)
            {
                Vector2 trTile = new Vector2(tile.x + 1, tile.y - 1);
                if (CheckIfBlankTile(trTile))
                {
                    neighbors.Add(trTile);
                }
            }

            if (botleft == true)
            {
                Vector2 blTile = new Vector2(tile.x - 1, tile.y + 1);
                if (CheckIfBlankTile(blTile))
                {
                    neighbors.Add(blTile);
                }
            }

            if (botright == true)
            {
                Vector2 brTile = new Vector2(tile.x + 1, tile.y + 1);
                if (CheckIfBlankTile(brTile))
                {
                    neighbors.Add(brTile);
                }
            }
        }

        /*
        Debug.Log("TILE: " + tile);
        foreach(Vector2 n in neighbors)
        {
            Debug.Log("NEIGHBOR: " + n);
        }
        */

        return neighbors;
    }
}
