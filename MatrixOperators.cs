public static class MatrixOperations
{
    public static Matrix Transpose(Matrix matrix)
    {
        int rows = matrix.Columns;
        int cols = matrix.Rows;
        double[,] result = new double[rows, cols];
        Parallel.For(0, rows, i =>
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = matrix[j, i];
            }
        });
        return new Matrix(result);
    }

    public static Matrix Multiply(Matrix a, double scalar)
    {
        int rows = a.Rows;
        int cols = a.Columns;
        double[,] result = new double[rows, cols];
        Parallel.For(0, rows, i =>
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = a[i, j] * scalar;
            }
        });
        return new Matrix(result);
    }

    public static Matrix Add(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
            throw new ArgumentException("Обе матрицы должны быть одинакого размера.");

        int rows = a.Rows;
        int cols = a.Columns;
        double[,] result = new double[rows, cols];
        Parallel.For(0, rows, i =>
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = a[i, j] + b[i, j];
            }
        });
        return new Matrix(result);
    }

    public static Matrix Subtract(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
            throw new ArgumentException("Обе матрицы должны быть одинакого размера.");

        int rows = a.Rows;
        int cols = a.Columns;
        double[,] result = new double[rows, cols];
        Parallel.For(0, rows, i =>
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = a[i, j] - b[i, j];
            }
        });
        return new Matrix(result);
    }

    public static Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows)
            throw new ArgumentException("Количество строк их 1-й матрицы и количество колонок со 2-й матрицы должны быть одинаковыми.");

        int rows = a.Rows;
        int cols = b.Columns;
        int common = a.Columns;
        double[,] result = new double[rows, cols];
        Parallel.For(0, rows, i =>
        {
            for (int j = 0; j < cols; j++)
            {
                double sum = 0;
                for (int k = 0; k < common; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                result[i, j] = sum;
            }
        });
        return new Matrix(result);
    }

    public static (Matrix, double) Inverse(Matrix matrix)
    {
        throw new NotImplementedException();
    }
}