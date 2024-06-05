using System;

class IRootFindingStrategy {
  public static void Main (string[] args) {

  }
}
public interface IRootFindingStrategy
{
    Complex[] Solve(double[] coefficients);
}
