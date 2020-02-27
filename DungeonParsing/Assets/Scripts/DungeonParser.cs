using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonParser : MonoBehaviour
{
    // Text Data Variables
    public TextAsset data;
    string[] linesFromfile;

    // Parsing Variables
    private int dungeonWidth;
    private int dungeonHeight;
    List<int> visitedRowSpaces = new List<int>();
    List<int> visitedColSpaces = new List<int>();
    List<Vector2> visited = new List<Vector2>();

    Dictionary<Vector2, List<Vector2>> spaces = new Dictionary<Vector2, List<Vector2>>();

    private void Awake()
    {
        linesFromfile = data.text.Split("\n"[0]);
        dungeonWidth = linesFromfile[0].Length;
        dungeonHeight = linesFromfile.Length;

        for (int i = 0; i < dungeonWidth; i++)
        {
            visitedColSpaces.Add(i);
        }

        for (int i = 0; i < dungeonHeight; i++)
        {
            visitedRowSpaces.Add(i);
        }
    }
    void Start()
    {
        Debug.Log(Parse());
    }

    private bool OrthogonalParse()
    {

        bool verdict = false;

        // x, i = current row
        // y, j = current col
        for (int i = 0; i < dungeonHeight; i++)
        {
            for (int j = 0; j < dungeonWidth; j++)
            {
                print(dungeonWidth);
                if (linesFromfile[i][j] == ' ')
                {
                    visitedColSpaces.Remove(j);
                    visitedRowSpaces.Remove(i);
                }
            }
        }

        if (visitedColSpaces.Count == 0 && visitedRowSpaces.Count == 0)
        {
            verdict = true;
        }

        return verdict;
    }

    private bool Parse()
    {
        bool verdict = true;

        // x, i = current row
        // y, j = current col
        for (int i = 0; i < dungeonHeight; i++)
        {
            for (int j = 0; j < dungeonWidth; j++)
            {
                if (linesFromfile[i][j] == ' ')
                {
                    bool top = false;
                    bool left = false;
                    bool right = false;
                    bool bot = false;
                    int numTrue = 0;

                    // Check if not null
                    if(i-1 < 0)
                    {
                        top = true;
                        numTrue++;
                    }
                    if (i + 1 > dungeonWidth)
                    {
                        bot = true;
                    }
                    if (j - 1 < 0)
                    {
                        left = true;
                    }
                    if (j + 1 > dungeonHeight)
                    {
                        right = true;
                    }

                    if (left == false && right == false && top == false && bot == false)
                    {
                        if (linesFromfile[i - 1][j] != ' ' && linesFromfile[i][j - 1] != ' ' && linesFromfile[i + 1][j] != ' ' && linesFromfile[i][j + 1] != ' ')
                        {
                            verdict = false;
                        }
                    } 
                    else
                    {
                        if (left == false && right == true)
                        {
                            if (linesFromfile[i][j - 1] == ' ')
                            {
                                verdict = false;
                            }
                        }
                        if (right == false && left == true)
                        {
                            if (linesFromfile[i][j + 1] == ' ')
                            {
                                verdict = false;
                            }
                        }
                        if (top == false && bot == true)
                        {
                            if (linesFromfile[i - 1][j] == ' ')
                            {
                                verdict = false;
                            }
                        }
                        if (bot == false && top == true)
                        {
                            if (linesFromfile[i + 1][j] == ' ')
                            {
                                verdict = false;
                            }
                        }
                    }
                }
            }
        }

        return verdict;
    }

}
