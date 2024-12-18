using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode2024.Day17;

public class Day17 : IAocDay
{
    private static string[] _input;
    private static BigInteger _registerA;
    private static BigInteger _registerB;
    private static BigInteger _registerC;
    private static int[] _program;
    private static int _instructionPointer;
    private static string _output = "";
    private static string _programString;

    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "/Day17/data-aoc-day17.txt");
        _registerA = BigInteger.Parse(Regex.Matches(_input[0], @"(?<=Register A:\s)\d+")[0].Value);
        _registerB = BigInteger.Parse(Regex.Matches(_input[1], @"(?<=Register B:\s)\d+")[0].Value);
        _registerC = BigInteger.Parse(Regex.Matches(_input[2], @"(?<=Register C:\s)\d+")[0].Value);
        _program = Regex.Matches(_input[4], @"(?<=Program:\s)((\d+,)*\d+)")[0].Value.Split(',').Select(int.Parse)
            .ToArray();
        _programString = string.Join(",", _program);
        Console.WriteLine($"Register A: {_registerA}");
        Console.WriteLine($"Register B: {_registerB}");
        Console.WriteLine($"Register C: {_registerC}");
        Console.WriteLine($"Program: {string.Join(",", _program)}");
    }

    public static void RunPart1()
    {
        Initialize();
        Execute();
        Console.WriteLine($"Register A: {_registerA}");
        Console.WriteLine($"Register B: {_registerB}");
        Console.WriteLine($"Register C: {_registerC}");
        Console.WriteLine($"Output: {_output}");
    }
    
    public static void RunPart2()
    {
        Initialize();
        
        Console.WriteLine($"Register A: {_registerA}");
        Console.WriteLine($"Register B: {_registerB}");
        Console.WriteLine($"Register C: {_registerC}");
        Console.WriteLine($"Output: {_output}");
    }

    private static void Execute()
    {
        _output = "";
        while (_instructionPointer < _program.Length)
        {
            var opcode = _program[_instructionPointer];
            var literalOperand = _program[_instructionPointer + 1];
            var comboOperand = convertToComboOperand(literalOperand);
            var jump = false;
            switch (opcode)
            {
                case 0:
                    adv(comboOperand);
                    break;
                case 1:
                    bxl(literalOperand);
                    break;
                case 2:
                    bst(comboOperand);
                    break;
                case 3:
                    jump = jnz(literalOperand);
                    break;
                case 4:
                    bxc();
                    break;
                case 5:
                    if (_output == "")
                        _output = outOpcode(comboOperand).ToString();
                    else
                        _output = _output + "," + outOpcode(comboOperand).ToString();
                    break;
                case 6:
                    bdv(comboOperand);
                    break;
                case 7:
                    cdv(comboOperand);
                    break;
            }

            if (!jump)
            {
                _instructionPointer += 2;
            }
        }
    }
    
    private static void Execute2()
    {
        _output = "";
        while (_instructionPointer < _program.Length)
        {
            var opcode = _program[_instructionPointer];
            var literalOperand = _program[_instructionPointer + 1];
            var comboOperand = convertToComboOperand(literalOperand);
            var jump = false;
            switch (opcode)
            {
                case 0:
                    adv(comboOperand);
                    break;
                case 1:
                    bxl(literalOperand);
                    break;
                case 2:
                    bst(comboOperand);
                    break;
                case 3:
                    jump = jnz(literalOperand);
                    break;
                case 4:
                    bxc();
                    break;
                case 5:
                    if (_output == "")
                        _output = outOpcode(comboOperand).ToString();
                    else
                        _output = _output + "," + outOpcode(comboOperand).ToString();
                    var pattern = "^" + Regex.Escape(_output);
                    // if (Regex.Matches(_programString, pattern).Count == 0)
                    // {
                    //     // Console.WriteLine("Discontinue");
                    //     // Console.WriteLine($"Program: {_programString}");
                    //     // Console.WriteLine($"Output: {_output}");
                    //     return;
                    // }
                    if(_output.Split(',').Length >8)
                    {
                        // Console.WriteLine("Continue");
                        // Console.WriteLine($"Program: {_programString}");
                        Console.WriteLine($"Output: {_output}");
                        Console.WriteLine($"Register A: {_registerA}");
                        Console.WriteLine($"Register B: {_registerB}");
                        Console.WriteLine($"Register C: {_registerC}");
                    }
                    break;
                case 6:
                    bdv(comboOperand);
                    break;
                case 7:
                    cdv(comboOperand);
                    break;
            }
            PrintCursor();

            if (!jump)
            {
                _instructionPointer += 2;
            }
        }
    }

    private static BigInteger convertToComboOperand(int literalOperand)
    {
        switch (literalOperand)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return literalOperand;
            case 4:
                return _registerA;
            case 5:
                return _registerB;
            case 6:
                return _registerC;
            case 7:
                Console.WriteLine("Invalid program");
                return -1;
        }

        return -1;
    }

    private static void adv(BigInteger comboOperand)
    {
        _registerA = (BigInteger)Math.Truncate((double)_registerA / Math.Pow(2, (double)comboOperand));
    }

    private static void bxl(int literalOperand)
    {
        _registerB = _registerB ^ literalOperand;
    }

    private static void bst(BigInteger comboOperand)
    {
        _registerB = comboOperand % 8;
    }

    private static bool jnz(int literalOperand)
    {
        if (_registerA == 0)
        {
            return false;
        }
        PrintCursor();
        _instructionPointer = literalOperand;
        return true;
    }

    private static void PrintCursor()
    {
        Console.WriteLine($"_output: {_output}");
        Console.Write(_programString);
        Console.Write($"  Register A: {_registerA}  ");
        Console.Write($"  Register B: {_registerB}  ");
        Console.Write($"  Register C: {_registerC}  ");
        Console.Write($"  Register B modulo 8: {_registerB % 8}");
        Console.WriteLine();
        for (int i = 0; i < _program.Length; i++)
        {
            if (i == _instructionPointer)
            {
                Console.Write("\u2191");
            }
            else
            {
                Console.Write("  ");
            }
        }
        Console.WriteLine();
    }

    private static void bxc()
    {
        _registerB = _registerB ^ _registerC;
    }

    private static BigInteger outOpcode(BigInteger comboOperand)
    {
        return comboOperand % 8;
    }

    private static void bdv(BigInteger comboOperand)
    {
        _registerB = (BigInteger)Math.Truncate((double)_registerA / Math.Pow(2, (double)comboOperand));
    }

    private static void cdv(BigInteger comboOperand)
    {
        _registerC = (BigInteger)Math.Truncate((double)_registerA / Math.Pow(2, (double)comboOperand));
    }

    public void BenchmarkPart1()
    {
        throw new NotImplementedException();
    }

    public void BenchmarkPart2()
    {
        throw new NotImplementedException();
    }
}