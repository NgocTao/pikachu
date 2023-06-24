using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int rows = 7;
    public int cols = 12;
    public string savePath = "Assets/Game/Resources/Data/Levels/level.json";

    private void Start()
    {
        // Sử dụng hàm để tạo một cấp độ có kích thước rows x cols
        int[,] level = GenerateLevel(rows, cols);

        // Lưu cấp độ vào tệp tin JSON
        SaveLevelToJson(level, savePath);
    }

    private int[,] GenerateLevel(int rows, int cols)
    {
        // Tạo một mảng 2 chiều rỗng
        int[,] grid = new int[rows, cols];

        // Tạo danh sách các cặp ô giống nhau ngẫu nhiên
        List<int> pairs = new List<int>();
        for (int i = 1; i <= (rows * cols) / 2; i++)
        {
            pairs.Add(i);
            pairs.Add(i);
        }
        ShuffleList(pairs);

        // Đặt các cặp ô vào lưới
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 0)
                {
                    grid[i, j] = pairs[index];
                    index++;
                }
            }
        }

        return grid;
    }

    private void ShuffleList<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void SaveLevelToJson(int[,] level, string filePath)
    {
        // Chuyển đổi mảng 2 chiều thành một danh sách các danh sách con
        List<List<int>> gridList = new List<List<int>>();
        int rows = level.GetLength(0);
        int cols = level.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            List<int> rowList = new List<int>();
            for (int j = 0; j < cols; j++)
            {
                rowList.Add(level[i, j]);
            }
            gridList.Add(rowList);
        }

        // Chuyển danh sách thành chuỗi JSON
        string json = JsonConvert.SerializeObject(gridList, Formatting.Indented);

        // Lưu chuỗi JSON vào tệp tin
        File.WriteAllText(filePath, json);

        Debug.Log("Level saved to: " + filePath);
    }
}
