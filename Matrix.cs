public class Matrix
{
    private double[,] values;

    public Matrix(double[,] values)
    {
        this.values = values;
    }

    public double this[int i, int j]
    {
        get { return values[i, j]; }
    }

    public int Rows
    {
        get { return values.GetLength(0); }
    }
    public int Columns
    {
        get { return values.GetLength(1); }
    }

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                result += values[i, j].ToString() + "\t";
            }
            result += "\n";
        }
        return result;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Matrix))
            return false;

        Matrix other = (Matrix)obj;

        if (this.Rows != other.Rows || this.Columns != other.Columns)
            return false;

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (this[i, j] != other[i, j])
                    return false;
            }
        }
        return true;
    }
}