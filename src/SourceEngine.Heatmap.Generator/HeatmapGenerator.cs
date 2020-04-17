﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private static string inputDataFilepathsFile;
        private static string heatmapJsonDirectory;
        private static string outputHeatmapDirectory;

        private static void helpText()
        {
            Debug.WriteLine("                             ========= HELP ==========\n\n" +
                        "Command line parameters:\n\n" +
                        "-inputdatadirectory            [path]                              The folder location of the input data json files (parsed demo data)\n" +
                        "-inputdatafilepathsfile        [path]                              The file location of the text file containing a list of filepaths that contain input json data (parsed demo data)\n" +
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
                else if (arg.ToLower() == "-inputdatafilepathsfile")
                {
                    if (i < args.Count())
                    {
                        inputDataFilepathsFile = args[i + 1];
                        CreateFileIfDoesntExist(args[i + 1]);
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

            if ((string.IsNullOrWhiteSpace(inputDataDirectory) && string.IsNullOrWhiteSpace(inputDataFilepathsFile)) ||
                string.IsNullOrWhiteSpace(heatmapJsonDirectory) || string.IsNullOrWhiteSpace(outputHeatmapDirectory) ||
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
            var filepathsFromDirectory = Directory.GetFiles(inputDataDirectory).ToList();
            var filepathsFromTxtFile = GetFilepathsFromInputDataFile();
            var allFilepathsForParsedDemos = filepathsFromDirectory.Concat(filepathsFromTxtFile);

            var allStatsList = new List<AllStats>();

            foreach (var filepath in allFilepathsForParsedDemos)
            {
                allStatsList.Add(ReadJsonFile<AllStats>(typeof(AllStats), filepath));

                Console.WriteLine("Finished reading allStats for: " + filepath);
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
                if (heatmapsToGenerate.Any(x => x.ToLower() == "all"))
                {
                    heatmapsToGenerate = new List<string>();

                    var instance = new HeatmapTypeNames();
                    heatmapsToGenerate = typeof(HeatmapTypeNames)
                                             .GetFields()
                                             .Select(field => field.GetValue(instance))
                                             .Cast<string>()
                                             .ToList();

                }

                CreateHeatmaps(heatmapsToGenerate, heatmapData.AllStatsList);
            }
            else
            {
                Console.WriteLine("No AllStats instances (parsed demo data) found.");
            }
        }

        private static List<string> GetFilepathsFromInputDataFile()
        {
            return File.ReadAllLines(inputDataFilepathsFile).ToList();
        }

        private static OverviewInfo ReadOverviewTxtFile(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);

            // remove inline comments
            for (int i = 0; i < lines.Count(); i++)
            {
                for (int j = 0; j < lines[i].Count(); j++)
                {
                    if (lines[i][j] == '/' && j + 1 < lines[i].Count() && lines[i][j+1] == '/')
                    {
                        lines[i] = lines[i].Substring(0, j);
                    }
                }
            }

            // remove blank entries
            lines = lines.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            // remove map name line
            lines[0] = string.Empty;
            lines = lines.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            // remove tabs and .ToLower() everything
            for (int i = 0; i < lines.Count(); i++)
            {
                lines[i] = Regex.Replace(lines[i], @"\t", "").ToLower();
            }

            // remove blank entries
            lines = lines.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            // add colons
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines.Count() > i + 1)
                {
                    var quotesCount = 0;

                    for (int j = 0; j < lines[i].Count(); j++)
                    {
                        if (lines[i][j] == '\"')
                        {
                            quotesCount++;

                            if (quotesCount == 2)
                            {
                                lines[i] = lines[i].Substring(0, j + 1) + ":" + lines[i].Substring(j + 1);

                                if (lines[i + 1] != "{" && lines[i + 1] != "}")
                                {
                                    lines[i] += ",";
                                }

                                break;
                            }
                        }
                        else if ((lines[i] == "{" && lines[i + 1] == "}") ||
                                (lines[i] == "}" && lines.Count() > i + 1 && !string.IsNullOrWhiteSpace(lines[i + 1]))
                        )
                        {
                            lines[i] += ",";
                        }
                    }
                }
            }

            // make it into one json string
            var overviewJson = string.Join(string.Empty, lines);

            JObject jObject = JObject.Parse(overviewJson);
            OverviewInfo overviewInfo = jObject.ToObject<OverviewInfo>();

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

        private static void CreateHeatmaps(List<string> heatmapsToGenerate, List<AllStats> allStatsList)
        {
            //imageProcessorExtender.BlurImage(radarLocation, @"C:\Users\jimmy\Desktop\heatmapstuff\bar1.png"); //blur the image

            OverviewInfo overviewInfo = GetOverviewInfo(allStatsList);

            foreach (var heatmapType in heatmapsToGenerate)
            {
                Image bmp = new Bitmap(1024, 1024);

                using (var graphics = Graphics.FromImage(bmp))
                {
                    string outputFilepath = GenerateHeatmapDataByType(heatmapType, overviewInfo, allStatsList, graphics);

                    graphics.Save();

                    if (heatmapType == HeatmapTypeNames.BombPlantLocations)
                    {
                        var aSiteOutputFilepath = outputFilepath + "_asite.png";
                        var aSiteOutputOverviewFilepath = outputFilepath + "_asite_overview.png";
                        var bSiteOutputFilepath = outputFilepath + "_bsite.png";
                        var bSiteOutputOverviewFilepath = outputFilepath + "_bsite_overview.png";

                        var bombsiteA = allStatsList.FirstOrDefault().bombsiteStats.Where(x => x.Bombsite == 'A').FirstOrDefault();
                        var bombsiteB = allStatsList.FirstOrDefault().bombsiteStats.Where(x => x.Bombsite == 'B').FirstOrDefault();

                        PointsData pointsDataASite = new PointsData()
                        {
                            DataForPoint1X = bombsiteA.XPositionMin,
                            DataForPoint1Y = bombsiteA.YPositionMin,
                            DataForPoint2X = bombsiteA.XPositionMax,
                            DataForPoint2Y = bombsiteA.YPositionMax,
                        };
                        PointsData pointsDataBSite = new PointsData()
                        {
                            DataForPoint1X = bombsiteB.XPositionMin,
                            DataForPoint1Y = bombsiteB.YPositionMin,
                            DataForPoint2X = bombsiteB.XPositionMax,
                            DataForPoint2Y = bombsiteB.YPositionMax,
                        };

                        SaveImagePngObjective(overviewInfo, bmp, pointsDataASite, aSiteOutputFilepath, aSiteOutputOverviewFilepath);
                        SaveImagePngObjective(overviewInfo, bmp, pointsDataBSite, bSiteOutputFilepath, bSiteOutputOverviewFilepath);

                        /*outputFilepath += ".png";
                        SaveImagePng(bmp, outputFilepath);*/
                    }
                    else
                    {
                        outputFilepath += ".png";
                        SaveImagePng(bmp, outputFilepath);
                    }

                    bmp = new Bitmap(1024, 1024);
                }

                DisposeImage(bmp);
            }
        }

        private static string GenerateHeatmapDataByType(string heatmapType, OverviewInfo overviewInfo, List<AllStats> allStatsList, Graphics graphics)
        {
            string outputFilepath = string.Concat(outputHeatmapDirectory, allStatsList.FirstOrDefault().mapInfo.MapName, "_", heatmapType.ToLower());

            heatmapTypeDataGatherer.GenerateByHeatmapType(heatmapType.ToLower(), overviewInfo, allStatsList, graphics);

            return outputFilepath;
        }

        private static void SaveImagePng(Image img, string filepath)
        {
            img.Save(filepath, ImageFormat.Png);
        }

        private static void SaveImagePngObjective(OverviewInfo overviewInfo, Image img, PointsData pointsData, string filepathObjective, string filepathObjectiveOverview)
        {
            Bitmap overviewImage = new Bitmap(@"C:\Users\jimmy\Desktop\heatmapstuff\de_mutiny_radar.png"); ///////////////////////////////////////////////
            //overviewImage.SetResolution(96, 96);

            var xPoint1 = (Convert.ToSingle(pointsData.DataForPoint1X) - overviewInfo.OffsetX) / overviewInfo.Scale;
            var yPoint1 = (Convert.ToSingle(pointsData.DataForPoint1Y) - overviewInfo.OffsetY) / overviewInfo.Scale;
            var xPoint2 = (Convert.ToSingle(pointsData.DataForPoint2X) - overviewInfo.OffsetX) / overviewInfo.Scale;
            var yPoint2 = (Convert.ToSingle(pointsData.DataForPoint2Y) - overviewInfo.OffsetY) / overviewInfo.Scale;

            var xPointLeft = xPoint1 <= xPoint2 ? Math.Abs(xPoint1) : Math.Abs(xPoint2);
            var yPointTop = yPoint1 <= yPoint2 ? Math.Abs(yPoint2) : Math.Abs(yPoint1);
            var xPointRight = xPoint1 <= xPoint2 ? Math.Abs(xPoint2) : Math.Abs(xPoint1);
            var yPointBottom = yPoint1 <= yPoint2 ? Math.Abs(yPoint1) : Math.Abs(yPoint2);

            var marginX = (xPointRight - xPointLeft) / 5;
            var marginY = (yPointTop - yPointBottom) / 5;
            Rectangle cropObjective = Rectangle.FromLTRB((int)(xPointLeft - marginX), (int)(yPointTop + marginY), (int)(xPointRight + marginX), (int)(yPointBottom - marginY));

            Image bmpCropObjective = ((Bitmap)img).Clone(cropObjective, img.PixelFormat);
            Image bmpCropObjectiveOverview = overviewImage.Clone(cropObjective, img.PixelFormat);

            //bmpCropASite.SetResolution(1024, 1024);
            //bmpCropBSite.SetResolution(1024, 1024);

            SaveImagePng(bmpCropObjective, filepathObjective);
            SaveImagePng(bmpCropObjectiveOverview, filepathObjectiveOverview);
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
