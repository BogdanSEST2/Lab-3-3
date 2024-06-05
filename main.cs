using System;

namespace ComplexNumbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Complex complex1 = new Complex(3, 4);
            Complex complex2 = new Complex(5, -2);

            Console.WriteLine($"Complex number 1: {complex1}");
            Console.WriteLine($"Complex number 2: {complex2}");

            Complex sum = complex1 + complex2;
            Console.WriteLine($"Sum: {sum}");

            Complex difference = complex1 - complex2;
            Console.WriteLine($"Difference: {difference}");

            Complex product = complex1 * complex2;
            Console.WriteLine($"Product: {product}");

            Complex quotient = complex1 / complex2;
            Console.WriteLine($"Quotient: {quotient}");

            double realPart = complex1.Real;
            double imaginaryPart = complex1.Imaginary;
            Console.WriteLine($"Real part of complex1: {realPart}");
            Console.WriteLine($"Imaginary part of complex1: {imaginaryPart}");

            double length = complex1.Length;
            Console.WriteLine($"Length of complex1: {length}");

            Complex sqrt = Complex.Sqrt(25);
            Console.WriteLine($"Square root of 25: {sqrt}");
            
            try
            {
                double[] coefficients = ReadCoefficientsFromConsole();
                Equation equation = new Equation(coefficients);
                Complex[] roots = equation.FindRoots();
                PrintRoots(roots);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        static double[] ReadCoefficientsFromConsole()
        {
            double[] coefficients = new double[3];
            Console.WriteLine("Enter the coefficients of the quadratic equation ax^2 + bx + c = 0:");
            for (int i = 0; i < coefficients.Length; i++)
            {
                Console.Write($"Coefficient {i + 1}: ");
                if (!double.TryParse(Console.ReadLine(), out coefficients[i]))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    i--;
                }
            }
            return coefficients;
        }

        static void PrintRoots(Complex[] roots)
        {
            Console.WriteLine("Roots:");
            foreach (Complex root in roots)
            {
                Console.WriteLine(root);
            }
        }
    }

    public class Complex
    {
        public static readonly Complex Zero = new Complex(0, 0);
        public static readonly Complex One = new Complex(1, 0);
        public static readonly Complex ImaginaryOne = new Complex(0, 1);

        public double Real { get; }
        public double Imaginary { get; }

        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public Complex(double real) : this(real, 0) { }

        public Complex() : this(0, 0) { }

        public double Length => Math.Sqrt(Real * Real + Imaginary * Imaginary);

        public static Complex Sqrt(double x)
        {
            if (x < 0)
                return new Complex(0, Math.Sqrt(-x));
            else
                return new Complex(Math.Sqrt(x));
        }

        public static Complex operator +(Complex a, Complex b) =>
            new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);

        public static Complex operator -(Complex a, Complex b) =>
            new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);

        public static Complex operator *(Complex a, Complex b) =>
            new Complex(a.Real * b.Real - a.Imaginary * b.Imaginary,
                        a.Real * b.Imaginary + a.Imaginary * b.Real);

        public static Complex operator /(Complex a, Complex b)
        {
            double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
            if (denominator == 0)
                throw new DivideByZeroException("Division by zero.");

            double realPart = (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator;
            double imaginaryPart = (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator;

            return new Complex(realPart, imaginaryPart);
        }

        public static Complex operator +(Complex a) => a;

        public static Complex operator -(Complex a) => new Complex(-a.Real, -a.Imaginary);

        public override string ToString() => $"{Real} + {Imaginary}i";

        public override bool Equals(object obj)
        {
            if (!(obj is Complex))
                return false;

            Complex other = (Complex)obj;
            return Real == other.Real && Imaginary == other.Imaginary;
        }

        public override int GetHashCode() => Real.GetHashCode() ^ Imaginary.GetHashCode();
    }

    public class Equation
    {
        private readonly double[] coefficients;

        public Equation(double[] coefficients)
        {
            this.coefficients = coefficients;
        }

        public Complex[] FindRoots()
        {
            double a = coefficients[0];
            double b = coefficients[1];
            double c = coefficients[2];

            double discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                Complex root1 = new Complex(-b / (2 * a), Math.Sqrt(-discriminant) / (2 * a));
                Complex root2 = new Complex(-b / (2 * a), -Math.Sqrt(-discriminant) / (2 * a));
                return new Complex[] { root1, root2 };
            }
            else
            {
                Complex root1 = new Complex((-b + Math.Sqrt(discriminant)) / (2 * a));
                Complex root2 = new Complex((-b - Math.Sqrt(discriminant)) / (2 * a));
                return new Complex[] { root1, root2 };
            }
        }
    }
}
