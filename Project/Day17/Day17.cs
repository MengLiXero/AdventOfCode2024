using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AdventOfCode2024.Day17;

public class Day17 : IAocDay
{
    private static string[] _input;
    private static int _registerA;
    private static int _registerB;
    private static int _registerC;
    private static int[] _program;
    private static int _instructionPointer;
    private static string _output = "";

    private static void Initialize()
    {
        _input = File.ReadAllLines(Constants.baseDir + "/Day17/data-aoc-day17.txt");
        _registerA = int.Parse(Regex.Matches(_input[0], @"(?<=Register A:\s)\d+")[0].Value);
        _registerB = int.Parse(Regex.Matches(_input[1], @"(?<=Register B:\s)\d+")[0].Value);
        _registerC = int.Parse(Regex.Matches(_input[2], @"(?<=Register C:\s)\d+")[0].Value);
        _program = Regex.Matches(_input[4], @"(?<=Program:\s)((\d+,)*\d+)")[0].Value.Split(',').Select(int.Parse)
            .ToArray();
        Console.WriteLine($"Register A: {_registerA}");
        Console.WriteLine($"Register B: {_registerB}");
        Console.WriteLine($"Register C: {_registerC}");
        Console.WriteLine($"Program: {string.Join(", ", _program)}");
    }

    public static void RunPart1()
    {
        Initialize();
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
                    if(_output=="")
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
        
        Console.WriteLine($"Register A: {_registerA}");
        Console.WriteLine($"Register B: {_registerB}");
        Console.WriteLine($"Register C: {_registerC}");
        Console.WriteLine($"Output: {_output}");
    }

    private static int convertToComboOperand(int literalOperand)
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

    private static void adv(int comboOperand)
    {
        _registerA = (int)Math.Truncate(_registerA / Math.Pow(2, comboOperand));
    }

    private static void bxl(int literalOperand)
    {
        _registerB = _registerB ^ literalOperand;
    }

    private static void bst(int comboOperand)
    {
        _registerB = comboOperand % 8;
    }

    private static bool jnz(int literalOperand)
    {
        if (_registerA == 0)
        {
            return false;
        }
        _instructionPointer = literalOperand;
        return true;
    }

    private static void bxc()
    {
        _registerB = _registerB ^ _registerC;
    }

    private static int outOpcode(int comboOperand)
    {
        return comboOperand % 8;
    }

    private static void bdv(int comboOperand)
    {
        _registerB = (int)Math.Truncate(_registerA / Math.Pow(2, comboOperand));
    }

    private static void cdv(int comboOperand)
    {
        _registerC = (int)Math.Truncate(_registerA / Math.Pow(2, comboOperand));
    }

    public static void RunPart2()
    {
        throw new NotImplementedException();
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