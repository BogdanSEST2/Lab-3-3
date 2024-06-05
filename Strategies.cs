using System;

class Strategies {
  public static void Main (string[] args) {

  }
}
public static class Strategies
{
    public static IRootFindingStrategy ChooseStrategy(double[] coefficients)
    {
        if (coefficients.Length == 2)
        {
            return new LinearEquationStrategy();
        }
        else if (coefficients.Length == 3)
        {
            return new QuadraticEquationStrategy();
        }
      else
      {
          return null;
      }
  }
}
public class LinearEquationStrategy : IRootFindingStrategy
{
    public Complex[] Solve(double[] coefficients)
    {
        if (coefficients.Length != 2)
            throw new ArgumentException("The coefficients array must contain 2 elements for a linear equation.");

        double a = coefficients[0];
        double b = coefficients[1];

        if (a == 0)
        {
            if (b == 0)
                throw new InvalidOperationException("Infinite number of roots.");
            else
                throw new InvalidOperationException("No roots.");
        }

        return new Complex[] { new Complex(-b / a) };
    }
}

public class QuadraticEquationStrategy : IRootFindingStrategy
{
    public Complex[] Solve(double[] coefficients)
    {
        if (coefficients.Length != 3)
            throw new ArgumentException("The coefficients array must contain 3 elements for a quadratic equation.");

        double a = coefficients[0];
        double b = coefficients[1];
        double c = coefficients[2];

        double discriminant = b * b - 4 * a * c;

        if (discriminant > 0)
        {
            double sqrtDiscriminant = Math.Sqrt(discriminant);
            return new Complex[] { new Complex((-b + sqrtDiscriminant) / (2 * a)), new Complex((-b - sqrtDiscriminant) / (2 * a)) };
        }
        else if (discriminant == 0)
        {
            return new Complex[] { new Complex(-b / (2 * a)) };
        }
        else
        {
            throw new InvalidOperationException("No roots.");
        }
    }
}