using Newtonsoft.Json;
using SourceEngine.Demo.Heatmaps.Compatibility;
using SourceEngine.Demo.Stats.Models;
using SourceEngine.Heatmap.Generator;
using SourceEngine.Heatmap.Generator.Constants;
using SourceEngine.Heatmap.Generator.Enums;
using SourceEngine.Heatmap.Generator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SourceEngine.Demo.Heatmaps
{
    public class HeatmapGenerator
    {
        private ImageProcessorExtender imageProcessorExtender = new ImageProcessorExtender();
        private static HeatmapTypeDataGatherer heatmapTypeDataGatherer = new HeatmapTypeDataGatherer();

        private static string inputDataDirectory;
        private static string heatmapJsonDirectory;
        private static string outputHeatmapDirectory;

        private static void helpText()
        {
            Debug.WriteLine("                             ========= HELP ==========\n\n" +
                        "Command line parameters:\n\n" +
                        "-inputdatadirectory            [path]                              The folder location of the input data json files (parsed demo data)\n" +
                        "-heatmapjsondirectory          [path]                              The folder location of the json for the previously created heatmap files\n" +
                        "-outputheatmapdirectory        [path]                              The folder location to output the generated heatmaps\n" +
                        "-heatmapstogenerate            [names (space seperated)]           A list of heatmap key names to generate\n" +
                        "\n"
                );
        }

        private static void Main(string[] args)
        {
            List<string> heatmapsToGenerate = new List<string>();
            
            if (args.Count() == 0)
            {
                helpText();
                return;
            }

            for (int i = 0; i < args.Count(); i++)
            {
                var arg = args[i].ToLower();

                if (arg.ToLower() == "-inputdatadirectory")
                {
                    if (i < args.Count())
                    {
                        inputDataDirectory = args[i + 1];
                        CreateDirectoryIfDoesntExist(Directory.GetParent(inputDataDirectory));
                    }

                    i++;
                }
                else if (arg.ToLower() == "-heatmapjsondirectory")
                {
                    if (i < args.Count())
                    {
                        heatmapJsonDirectory = args[i + 1];
                        CreateDirectoryIfDoesntExist(Directory.GetParent(heatmapJsonDirectory));
                    }

                    i++;
                }
                else if (arg.ToLower() == "-outputheatmapdirectory")
                {
                    if (i < args.Count())
                    {
                        outputHeatmapDirectory = args[i + 1];
                        CreateDirectoryIfDoesntExist(Directory.GetParent(outputHeatmapDirectory));
                    }

                    i++;
                }
                else if (arg.ToLower() == "-heatmapstogenerate")
                {
                    bool searching = true;
                    while (i < args.Count() - 1 && searching)
                    {
                        i++;

                        if (args[i][0] == '-')
                        {
                            searching = false;
                        }
                        else
                        {
                            heatmapsToGenerate.Add(args[i].ToLower());
                        }
                    }
                }
                else
                {
                    helpText();
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(inputDataDirectory) ||
                string.IsNullOrWhiteSpace(heatmapJsonDirectory) ||
                string.IsNullOrWhiteSpace(outputHeatmapDirectory) ||
                heatmapsToGenerate.Count() == 0
            )
            {
                helpText();
                return;
            }

            RunHeatmapGenerator(heatmapsToGenerate);

            Console.WriteLine("Finished generating heatmaps.");
        }

        private static void CreateDirectoryIfDoesntExist(DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(directoryInfo.FullName);
            }
        }

        private static void CreateFileIfDoesntExist(string filepath)
        {
            if (!File.Exists(filepath))
            {
                File.Create(filepath).Close();
            }
        }

        private static void RunHeatmapGenerator(List<string> heatmapsToGenerate)
        {
            var parsedDemoFiles = Directory.GetFiles(inputDataDirectory);
            var allStatsList = new List<AllStats>();
            foreach (var filepath in parsedDemoFiles)
            {
                allStatsList.Add(ReadJsonFile<AllStats>(typeof(AllStats), filepath));
            }

            var firstAllStats = allStatsList.First();
            var heatmapDataFilename = string.Concat(heatmapJsonDirectory, firstAllStats.mapInfo.MapName, Filenames.HeatmapDataFilenameEnding);
            CreateFileIfDoesntExist(heatmapDataFilename);
            var heatmapData = ReadJsonFile<MapHeatmapData>(typeof(MapHeatmapData), heatmapDataFilename);

            if (heatmapData == null)
            {
                heatmapData = new MapHeatmapData() { AllStatsList = new List<AllStats>() };
            }

            //add newly parsed demo data into heatmap data json file
            foreach (var allStats in allStatsList)
            {
                heatmapData.AllStatsList.RemoveAll(x => x.mapInfo.DemoName == allStats.mapInfo.DemoName); // if the parsed demo stats are not already in the heatmapData list, add them
                heatmapData.AllStatsList.Add(allStats);
                OverwriteJsonFile(heatmapData, heatmapDataFilename);
            }

            if (heatmapData.AllStatsList.Count() > 0)
            {
                Create(heatmapsToGenerate, heatmapData.AllStatsList);
            }
            else
            {
                Console.WriteLine("No AllStats instances (parsed demo data) found.");
            }
        }

        private static OverviewInfo ReadOverviewTxtFile(string filepath)
        {
            var overviewInfo = new OverviewInfo();

            var regexStatement = @"\-?\d+.+\d";

            string[] lines = File.ReadAllLines(filepath);
            foreach (var line in lines)
            {
                if (line.ToLower().Contains("scale"))
                {
                    overviewInfo.Scale = float.Parse(Regex.Match(line, regexStatement).Value);
                }
                else if (line.ToLower().Contains("pos_x"))
                {
                    overviewInfo.OffsetX = float.Parse(Regex.Match(line, regexStatement).Value);
                }
                else if (line.ToLower().Contains("pos_y"))
                {
                    overviewInfo.OffsetY = float.Parse(Regex.Match(line, regexStatement).Value);
                }
            }

            return overviewInfo;
        }

        private static void OverwriteJsonFile(object fileContents, string filepath)
        {
            File.WriteAllText(filepath, string.Empty);
            File.WriteAllText(filepath, JsonConvert.SerializeObject(fileContents, Formatting.Indented));
        }

        private static T ReadJsonFile<T>(Type type, string filename)
        {
            return (T)JsonConvert.DeserializeObject(File.ReadAllText(filename), type);
        }

        private static List<T> ReadJsonFiles<T>(Type type, string[] filenames)
        {
            List<object> jsonContentsList = new List<object>();

            foreach (var filename in filenames)
            {
                var json = ReadJsonFile<T>(type, filename);
                jsonContentsList.Add(json);
            }

            return (List<T>)(object)jsonContentsList;
        }

        private static void Create(List<string> heatmapsToGenerate, List<AllStats> allStatsList)
        {
            //imageProcessorExtender.BlurImage(radarLocation, @"C:\Users\jimmy\Desktop\heatmapstuff\bar1.png"); //blur the image

            OverviewInfo overviewInfo = GetOverviewInfo(allStatsList);

            foreach (var heatmapType in heatmapsToGenerate)
            {
                Image bmp = new Bitmap(1024, 1024);
                var bmpOriginalWidth = bmp.Width;
                var bmpOriginalHeight = bmp.Height;

                using (var graphics = Graphics.FromImage(bmp))
                {
                    string outputFilepath = GenerateHeatmapDataByType(heatmapType, overviewInfo, allStatsList, graphics);

                    graphics.Save();

                    SaveImagePng(bmp, outputFilepath);

                    bmp = new Bitmap(1024, 1024);
                }

                DisposeImage(bmp);
            }
        }

        private static string GenerateHeatmapDataByType(string heatmapType, OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            string outputFilepath = string.Concat(outputHeatmapDirectory, allStatsList.FirstOrDefault().mapInfo.MapName, "_", heatmapType.ToLower(), ".png");

            heatmapTypeDataGatherer.GenerateByHeatmapType(heatmapType.ToLower(), overviewInfo, allStatsList, graphics);

            return outputFilepath;
        }

        private static void SaveImagePng(Image img, string filepath)
        {
            img.Save(filepath, ImageFormat.Png);
        }

        private static void DeleteFileIfExists(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        private static void DisposeImage(Image img)
        {
            img.Dispose();
        }

        private static void CopyFile(string filepathOriginal, string filepathCopy)
        {
            File.Copy(filepathOriginal, filepathCopy);
        }

        private static OverviewInfo GetOverviewInfo(List<AllStats> allStatsList)
        {
            return ReadOverviewTxtFile(string.Concat(@"C:\Users\jimmy\Desktop\heatmapstuff\", allStatsList.FirstOrDefault().mapInfo.MapName, ".txt"));
        }
    }
}
