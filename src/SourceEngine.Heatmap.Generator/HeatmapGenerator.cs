﻿using Newtonsoft.Json;
using SourceEngine.Demo.Heatmaps.Compatibility;
using SourceEngine.Demo.Stats.Models;
using SourceEngine.Heatmap.Generator;
using SourceEngine.Heatmap.Generator.Enums;
using SourceEngine.Heatmap.Generator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SourceEngine.Demo.Heatmaps
{
    public class HeatmapGenerator
    {
        private ImageProcessorExtender imageProcessorExtender = new ImageProcessorExtender();
        private static HeatmapDataGatherer heatmapDataGatherer = new HeatmapDataGatherer();

        private static string inputDataFilepath;
        private static string heatmapJsonFilepath;
        private static string outputHeatmapFilepath;

        private static void helpText()
        {
            Debug.WriteLine("                             ========= HELP ==========\n\n" +
                        "Command line parameters:\n\n" +
                        "-inputdatafilepath             [path]                              The folder location of the input data json files (parsed demo data)\n" +
                        "-heatmapjsonfilepath           [path]                              The folder location of the json for the previously created heatmap files\n" +
                        "-outputheatmapfilepath         [path]                              The folder location to output the generated heatmaps\n" +
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

                if (arg.ToLower() == "-inputdatafilepath")
                {
                    if (i < args.Count())
                        inputDataFilepath = args[i + 1];

                    i++;
                }
                else if (arg.ToLower() == "-heatmapjsonfilepath")
                {
                    if (i < args.Count())
                        heatmapJsonFilepath = args[i + 1];

                    i++;
                }
                else if (arg.ToLower() == "-outputheatmapfilepath")
                {
                    if (i < args.Count())
                        outputHeatmapFilepath = args[i + 1];

                    i++;
                }
                else if (arg.ToLower() == "-heatmapstogenerate")
                {
                    bool searching = true;
                    while (i < args.Count() - 1 && searching)
                    {
                        i++;

                        if (args[i][0] == '-')
                            searching = false;
                        else
                            heatmapsToGenerate.Add(args[i].ToLower());
                    }
                }
                else
                {
                    helpText();
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(inputDataFilepath) ||
                string.IsNullOrWhiteSpace(heatmapJsonFilepath) ||
                string.IsNullOrWhiteSpace(outputHeatmapFilepath) ||
                heatmapsToGenerate.Count() == 0
            )
            {
                helpText();
                return;
            }

            RunHeatmapGenerator(heatmapsToGenerate);
        }

        private static void RunHeatmapGenerator(List<string> heatmapsToGenerate)
        {
            var allStatsList = ReadJsonFiles();

            foreach (var allStats in allStatsList)
            {
                Create(heatmapsToGenerate, allStats);
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

        private static List<AllStats> ReadJsonFiles()
        {
            var allFilenames = Directory.GetFiles(inputDataFilepath);

            List<AllStats> allStatsList = new List<AllStats>();

            foreach (var filename in allFilenames)
            {
                allStatsList.Add(JsonConvert.DeserializeObject<AllStats>(File.ReadAllText(filename)));
            }

            return allStatsList;
        }

        private static void Create(List<string> heatmapsToGenerate, AllStats allStats)
        {
            //imageProcessorExtender.BlurImage(radarLocation, @"C:\Users\jimmy\Desktop\heatmapstuff\bar1.png"); //blur the image

            OverviewInfo overviewInfo = GetOverviewInfo(allStats);

            foreach (var heatmapType in heatmapsToGenerate)
            {
                Image bmp = new Bitmap(1024, 1024);
                var bmpOriginalWidth = bmp.Width;
                var bmpOriginalHeight = bmp.Height;

                using (var graphics = Graphics.FromImage(bmp))
                {
                    string outputFilepath = GenerateHeatmapByType(heatmapType, overviewInfo, allStats, graphics);

                    graphics.Save();

                    SaveImagePng(bmp, outputFilepath);

                    bmp = new Bitmap(1024, 1024);
                }

                DisposeImage(bmp);
            }
        }

        private static string GenerateHeatmapByType(string heatmapType, OverviewInfo overviewInfo, AllStats allStats, Graphics graphics) //needs to append to old data rather than replace the image with one demo's data, needs to append onto json file (the same file it should read for this (containing previously used data for the image))
        {
            string outputFilepath = string.Empty;

            switch (heatmapType)
            {
                case "tkills":
                    heatmapDataGatherer.GenerateKillsHeatmap(overviewInfo, allStats, graphics, Sides.Terrorists);
                    outputFilepath = string.Concat(outputHeatmapFilepath, allStats.mapInfo.MapName, "_tKills.png");
                    break;
                case "ctkills":
                    heatmapDataGatherer.GenerateKillsHeatmap(overviewInfo, allStats, graphics, Sides.CounterTerrorists);
                    outputFilepath = string.Concat(outputHeatmapFilepath, allStats.mapInfo.MapName, "_ctKills.png");
                    break;
            }

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

        private static OverviewInfo GetOverviewInfo(AllStats allStats)
        {
            return ReadOverviewTxtFile(string.Concat(@"C:\Users\jimmy\Desktop\heatmapstuff\", allStats.mapInfo.MapName, ".txt"));
        }
    }
}