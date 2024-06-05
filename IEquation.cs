using System;

class IEquations {
  public static void Main (string[] args) {
    
  }
}
public interface IEquation
{
    int Dimension { get; }
    double[] Coefficients { get; }
    Complex[] FindRoots();
}
