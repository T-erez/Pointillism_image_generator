#nullable enable
using System.Diagnostics;
using System.Drawing;
using System;
using System.IO;
using System.Windows.Forms;
using Pointillism_image_generator;
namespace Pointillism_image_generator;

/// <summary>
/// Compares serial and parallel pointillism image generators.
/// </summary>
public static class SerialAndParallelAlgorithmComparer
{
    private static string _sourceImagePath;
    private static string _logsFolderPath;
    private static int[] _patternsSize = {7, 9, 11, 23};
    static int _patternsCount = 30000;
    static int _testsCount = 10;
    
    static Image _image = Image.FromFile(_sourceImagePath);
    static string _imageName = Path.GetFileNameWithoutExtension(_sourceImagePath);
    private static StreamWriter _writer = null!;

    public static void Run()
    {
        Directory.CreateDirectory(_logsFolderPath);
        
        foreach (int patternSize in _patternsSize)
        {
            Directory.CreateDirectory(Path.Combine(_logsFolderPath, $"{_imageName}{patternSize}"));
            _writer = new(Path.Combine(_logsFolderPath,$"{_imageName}{patternSize}.txt"));
            
            _writer.WriteLine($"Testing serial and parallel algorithm. Number of patterns: {_patternsCount}. Pattern size: {patternSize}. Number of tests: {_testsCount}.");
            _writer.WriteLine();
            _writer.Flush();
            TestSerialAlgorithm(patternSize, true);
            _writer.Flush();
            TestParallelAlgorithm(patternSize, true);
            _writer.Flush();
            _writer.Dispose();
        }
    }

    // public static void TestThreadsCount()
    // {
    //     Directory.CreateDirectory(_logsFolderPath);
    //
    //     foreach (int patternSize in _patternsSize)
    //     {
    //         int[] threadsCount = {4}; //, 8, 20, 50, 100, 150
    //         foreach (var threadCount in threadsCount)
    //         {
    //             Directory.CreateDirectory(Path.Combine(_logsFolderPath, $"{_imageName}{patternSize}"));
    //             _writer = new(Path.Combine(_logsFolderPath, $"{_imageName}{patternSize}{threadCount}.txt"));
    //             TestParallelAlgorithm(patternSize, true);
    //             _writer.WriteLine();
    //             _writer.Flush();
    //             _writer.Dispose();
    //         }
    //     }
    // }

    private static void TestSerialAlgorithm(int patternSize, bool saveGeneratedImage = false)
    {
        TestAlgorithm(patternSize, false, saveGeneratedImage);
    }

    private static void TestParallelAlgorithm(int patternSize, bool saveGeneratedImage = false)
    {
        TestAlgorithm(patternSize, true, saveGeneratedImage);
    }


    /// <summary>
    /// Counts the runtime of generator initialization and image generation.
    /// </summary>
    private static void TestAlgorithm(int patternSize, bool parallel, bool saveGeneratedImage)
    {
        string serialParallel = parallel ? "parallel" : "serial";
        _writer.WriteLine($"Testing {serialParallel} algorithm...");

        TimeSpan timeInit = TimeSpan.Zero;
        TimeSpan timeGenerate = TimeSpan.Zero;
        Stopwatch stopwatch = new Stopwatch();
        for (int i = 1; i <= _testsCount; i++)
        {
            _writer.WriteLine($"TEST {i}/{_testsCount}.");

            #region Init

            stopwatch.Restart();
            PointillismImageGenerator generator;
            if (parallel)
                generator = new PointillismImageGeneratorParallel(_image, patternSize, Color.White);
            else
                generator = new PointillismImageGeneratorSerial(_image, patternSize, Color.White);
            stopwatch.Stop();
            timeInit += stopwatch.Elapsed;
            _writer.WriteLine(stopwatch.Elapsed);

            #endregion

            #region Generate

            stopwatch.Restart();
            var (_, generatedBitmaps) = generator.AddPatterns(_patternsCount.ToIntReference());            
            stopwatch.Stop();
            timeGenerate += stopwatch.Elapsed;
            _writer.WriteLine(stopwatch.Elapsed);

            #endregion

            generator.Dispose();
            _writer.Flush();
            if (saveGeneratedImage)
            {
                GeneratedBitmap generated = generatedBitmaps[^1];
                generated.Bitmap.Save(Path.Combine(_logsFolderPath, $"{_imageName}{patternSize}\\{serialParallel}{i}.jpg"));
            }
            generatedBitmaps[^1].Bitmap.Dispose();
        }

        _writer.WriteLine($"Time init: {timeInit}");
        _writer.WriteLine($"Time generate: {timeGenerate}");
        _writer.WriteLine($"Total time: {timeInit+timeGenerate}");
        _writer.WriteLine($"Average time init: {timeInit / _testsCount}");
        _writer.WriteLine($"Average time generate: {timeGenerate / _testsCount}");
        _writer.WriteLine($"Average total time: {(timeInit+timeGenerate)/_testsCount}");
        _writer.WriteLine();
    }
}
