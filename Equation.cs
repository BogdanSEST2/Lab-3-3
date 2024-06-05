using System;

class Equation {
  public static void Main (string[] args) {
    
  }
}
public class Equation : IEquation
{
    private readonly double[] coefficients;
    private readonly IRootFindingStrategy strategy;

    public int Dimension => coefficients.Length;
    public double[] Coefficients => (double[])coefficients.Clone();

    public Equation(double[] coefficients)
    {
        coefficients = RemoveExtraCoefficients(coefficients);
        this.coefficients = coefficients;
        this.strategy = Strategies.ChooseStrategy(coefficients);
    }

    public Complex[] FindRoots()
    {
        if (strategy == null)
            throw new InvalidOperationException("Unknown equation type.");

        return strategy.Solve(coefficients);
    }

    private double[] RemoveExtraCoefficients(double[] coefficients)
    {
        int lastIndex = coefficients.Length - 1;
        while (lastIndex >= 0 && coefficients[lastIndex] == 0)
        {
            lastIndex--;
        }

        return lastIndex < coefficients.Length - 1 ? coefficients[..(lastIndex + 1)] : coefficients;
    }
}