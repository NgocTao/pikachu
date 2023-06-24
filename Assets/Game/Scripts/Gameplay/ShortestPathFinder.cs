using System;
using System.Collections.Generic;

public class ShortestPathFinder
{
    private int[,] grid;
    private int numRows;
    private int numColumns;

    public void GenerateGrid(CellData[,] cellData)
    {
        numRows = cellData.GetLength(0);
        numColumns = cellData.GetLength(1);
        grid = new int[numRows, numColumns];
        for (var r = 0; r < numRows; r++)
        {
            for (var c = 0; c < numColumns; c++)
            {
                grid[r, c] = cellData[r, c] != null ? -1 : 1;
            }
        }
    }

    public List<int[]> FindShortestPath(int startRow, int startColumn, int targetRow, int targetColumn)
    {
        // Kiểm tra tọa độ điểm đầu và điểm cuối có hợp lệ hay không
        if (!IsValidCoordinate(startRow, startColumn) || !IsValidCoordinate(targetRow, targetColumn))
        {
            Console.WriteLine("Tọa độ không hợp lệ!");
            return null;
        }

        // Tạo một hàng đợi để lưu trữ các ô chờ xét
        Queue<int[]> queue = new Queue<int[]>();

        // Tạo một mảng visited để lưu trữ trạng thái đã ghé thăm của các ô
        bool[,] visited = new bool[numRows, numColumns];

        // Tạo một mảng prevRow và prevColumn để lưu trữ tọa độ ô trước đó trên đường đi ngắn nhất
        int[,] prevRow = new int[numRows, numColumns];
        int[,] prevColumn = new int[numRows, numColumns];

        // Khởi tạo prevRow và prevColumn với giá trị -1
        for (var r = 0; r < numRows; r++)
        {
            for (var c = 0; c < numColumns; c++)
            {
                prevRow[r, c] = -1;
                prevColumn[r, c] = -1;
            }
        }

        // Đánh dấu ô đầu tiên là đã ghé thăm và thêm vào hàng đợi
        visited[startRow, startColumn] = true;
        queue.Enqueue(new int[] { startRow, startColumn });

        // Duyệt qua các ô kề của ô hiện tại để tìm đường đi ngắn nhất
        while (queue.Count > 0)
        {
            int[] currentCell = queue.Dequeue();
            int currentRow = currentCell[0];
            int currentColumn = currentCell[1];

            // Kiểm tra xem đã tìm được ô đích chưa
            if (currentRow == targetRow && currentColumn == targetColumn)
            {
                break;
            }

            // Kiểm tra các ô kề của ô hiện tại
            // Kiểm tra ô phía trên
            if (IsValidCoordinate(currentRow - 1, currentColumn) && !visited[currentRow - 1, currentColumn] && grid[currentRow - 1, currentColumn] == 1)
            {
                visited[currentRow - 1, currentColumn] = true;
                prevRow[currentRow - 1, currentColumn] = currentRow;
                prevColumn[currentRow - 1, currentColumn] = currentColumn;
                queue.Enqueue(new int[] { currentRow - 1, currentColumn });
            }

            // Kiểm tra ô phía dưới
            if (IsValidCoordinate(currentRow + 1, currentColumn) && !visited[currentRow + 1, currentColumn] && grid[currentRow + 1, currentColumn] == 1)
            {
                visited[currentRow + 1, currentColumn] = true;
                prevRow[currentRow + 1, currentColumn] = currentRow;
                prevColumn[currentRow + 1, currentColumn] = currentColumn;
                queue.Enqueue(new int[] { currentRow + 1, currentColumn });
            }

            // Kiểm tra ô bên trái
            if (IsValidCoordinate(currentRow, currentColumn - 1) && !visited[currentRow, currentColumn - 1] && grid[currentRow, currentColumn - 1] == 1)
            {
                visited[currentRow, currentColumn - 1] = true;
                prevRow[currentRow, currentColumn - 1] = currentRow;
                prevColumn[currentRow, currentColumn - 1] = currentColumn;
                queue.Enqueue(new int[] { currentRow, currentColumn - 1 });
            }

            // Kiểm tra ô bên phải
            if (IsValidCoordinate(currentRow, currentColumn + 1) && !visited[currentRow, currentColumn + 1] && grid[currentRow, currentColumn + 1] == 1)
            {
                visited[currentRow, currentColumn + 1] = true;
                prevRow[currentRow, currentColumn + 1] = currentRow;
                prevColumn[currentRow, currentColumn + 1] = currentColumn;
                queue.Enqueue(new int[] { currentRow, currentColumn + 1 });
            }
        }

        // Tạo đường đi từ ô đích về ô đầu tiên
        List<int[]> shortestPath = new List<int[]>();
        int row = targetRow;
        int column = targetColumn;

        while (row != startRow || column != startColumn)
        {
            shortestPath.Add(new int[] { row, column });
            int prevRowValue = prevRow[row, column];
            int prevColumnValue = prevColumn[row, column];
            row = prevRowValue;
            column = prevColumnValue;
        }

        // Đảo ngược danh sách đường đi để có thứ tự từ ô đầu tiên đến ô đích
        shortestPath.Reverse();

        return shortestPath;
    }

    private bool IsValidCoordinate(int row, int column)
    {
        return row >= 0 && row < numRows && column >= 0 && column < numColumns;
    }
}