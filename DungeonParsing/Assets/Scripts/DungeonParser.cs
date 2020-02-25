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
        Debug.Log(OrthogonalParse());
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

}
