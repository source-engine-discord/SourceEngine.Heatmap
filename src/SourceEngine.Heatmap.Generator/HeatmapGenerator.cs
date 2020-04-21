using Newtonsoft.Json;
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
        private static HeatmapTypeDataGatherer heatmapTypeDataGatherer = new HeatmapTypeDataGatherer();
        private static HeatmapLogicCenter heatmapLogicCenter = new HeatmapLogicCenter();

        private static HeatmapTypeNames heatmapTypeNames = new HeatmapTypeNames();
        private static List<string> validHeatmapTypeNames = new List<string>();

        private static string inputDataDirectory;
        private static string inputDataFilepathsFile;
        private static string overviewFilesDirectory;
        private static string heatmapJsonDirectory;
        private static string outputHeatmapDirectory;

        private static void helpText()
        {
            Console.WriteLine("                             ========= HELP ==========\n\n" +
                        "Command line parameters:\n\n" +
                        "-inputdatadirectory            [path]                              The folder location of the input data json files (parsed demo data)\n" +
                        "-inputdatafilepathsfile        [path]                              The file location of the text file containing a list of filepaths that contain input json data (parsed demo data)\n" +
                        "-overviewfilesdirectory        [path]                              The folder location of the overview files (required if generating BombplantLocations or HostageRescueLocations heatmaps)\n" +
                        "-heatmapjsondirectory          [path]                              The folder location of the json for the previously created heatmap files\n" +
                        "-outputheatmapdirectory        [path]                              The folder location to output the generated heatmaps\n" +
                        "-heatmapstogenerate            [names (space seperated)]           A list of heatmap key names to generate\n" +
                        "\n"
            );
        }

        private static void helpTextOverviewRequired()
        {
            Console.WriteLine("-overviewfilesdirectory required if generating BombplantLocations or HostageRescueLocations heatmaps.");
        }

        private static void helpTextInvalidHeatmapNameProvided(string invalidHeatmapName)
        {
            Console.WriteLine(string.Concat("Invalid heatmap name provided: ", invalidHeatmapName));
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Running Heatmap Generator");

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
                else if (arg.ToLower() == "-overviewfilesdirectory")
                {
                    if (i < args.Count())
                    {
                        overviewFilesDirectory = args[i + 1];
                        CreateDirectoryIfDoesntExist(Directory.GetParent(overviewFilesDirectory));
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

            // stores all possible valid heatmap names
            validHeatmapTypeNames = typeof(HeatmapTypeNames)
                                        .GetFields()
                                        .Select(field => field.GetValue(heatmapTypeNames))
                                        .Cast<string>()
                                        .ToList();

            // validity checks for possible issues
            if ((string.IsNullOrWhiteSpace(inputDataDirectory) && string.IsNullOrWhiteSpace(inputDataFilepathsFile)) ||
                string.IsNullOrWhiteSpace(heatmapJsonDirectory) || string.IsNullOrWhiteSpace(outputHeatmapDirectory) ||
                heatmapsToGenerate.Count() == 0
            )
            {
                helpText();
                return;
            }
            else if (string.IsNullOrWhiteSpace(overviewFilesDirectory) &&
                heatmapsToGenerate.Any(x => x == "all" ||
                                            x == HeatmapTypeNames.BombPlantLocations.ToString().ToLower() ||
                                            x == HeatmapTypeNames.HostageRescueLocations.ToString().ToLower())
            )
            {
                helpTextOverviewRequired();
                helpText();
                return;
            }
            else if (heatmapsToGenerate.Any(x => x != "all" && !validHeatmapTypeNames.Contains(x)))
            {
                helpTextInvalidHeatmapNameProvided(heatmapsToGenerate.Where(x => !validHeatmapTypeNames.Contains(x)).First());
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
            var filepathsFromDirectory = new List<string>();
            var filepathsFromTxtFile = new List<string>();

            if (!string.IsNullOrWhiteSpace(inputDataDirectory))
            {
                filepathsFromDirectory = Directory.GetFiles(inputDataDirectory).ToList();
            }
            if (!string.IsNullOrWhiteSpace(inputDataFilepathsFile))
            {
                filepathsFromTxtFile = GetFilepathsFromInputDataFile();
            }

            var allStatsList = new List<AllStats>();

            var allStatsMatchIdsDone = new List<string>();
            ParseJson(allStatsList, allStatsMatchIdsDone, filepathsFromDirectory);
            ParseJson(allStatsList, allStatsMatchIdsDone, filepathsFromTxtFile); // prioritise json from filepathsFromDirectory

            var firstAllStats = allStatsList.FirstOrDefault();
            var heatmapDataFilename = string.Concat(heatmapJsonDirectory, firstAllStats.mapInfo.MapName, Filenames.HeatmapDataFilenameEnding);
            CreateFileIfDoesntExist(heatmapDataFilename);
            var heatmapData = ReadJsonFile<MapHeatmapData>(typeof(MapHeatmapData), heatmapDataFilename);

            if (heatmapData == null)
            {
                heatmapData = new MapHeatmapData() { AllStatsList = new List<AllStats>() };
            }

            // add newly parsed demo data into heatmap data json file
            foreach (var allStats in allStatsList)
            {
                heatmapData.AllStatsList.RemoveAll(x => x.mapInfo.DemoName == allStats.mapInfo.DemoName); // replace matches that appear in the previously created heatmap files with the newly parsed information in allStatsList
                heatmapData.AllStatsList.Add(allStats);
                OverwriteJsonFile(heatmapData, heatmapDataFilename);
            }

            if (heatmapData.AllStatsList.Count() > 0)
            {
                if (heatmapsToGenerate.Any(x => x.ToLower() == "all"))
                {
                    heatmapsToGenerate = validHeatmapTypeNames;
                }

                // remove unnecessary defuse specific or hostage specific heatmaps for the map
                if (allStatsList.FirstOrDefault().mapInfo.GameMode.ToLower() == "defuse" || allStatsList.FirstOrDefault().rescueZoneStats.All(x => x.XPositionMin == null))
                {
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsBeforeHostageTaken.ToString());
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsAfterHostageTaken.ToString());
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsBeforeHostageTaken.ToString());
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsAfterHostageTaken.ToString());
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.HostageRescueLocations.ToString());
                }
                else if (allStatsList.FirstOrDefault().mapInfo.GameMode.ToLower() == "hostage" || allStatsList.FirstOrDefault().bombsiteStats.All(x => x.XPositionMin == null))
                {
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsBeforeBombplant.ToString());
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsAfterBombplant.ToString());
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsBeforeBombplant.ToString());
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsAfterBombplant.ToString());
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.BombPlantLocations.ToString());
                }

                // always put playerpositionsbyteam heatmap last as it takes much longer than the others
                if (heatmapsToGenerate.Any(x => x == HeatmapTypeNames.PlayerPositionsByTeam.ToString()))
                {
                    heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.PlayerPositionsByTeam.ToString());
                    heatmapsToGenerate.Add(HeatmapTypeNames.PlayerPositionsByTeam.ToString());
                }

                CreateHeatmaps(heatmapsToGenerate, heatmapData.AllStatsList);
            }
            else
            {
                Console.WriteLine("No AllStats instances (parsed demo data) found.");
            }
        }

        private static void ParseJson(List<AllStats> allStatsList, List<string> allStatsMatchIdsDone, List<string> filepaths)
        {
            foreach (var filepath in filepaths)
            {
                var json = ReadJsonFile<AllStats>(typeof(AllStats), filepath);

                if (!allStatsMatchIdsDone.Contains(json.mapInfo.DemoName))
                {
                    allStatsMatchIdsDone.Add(json.mapInfo.DemoName);
                    allStatsList.Add(json);

                    Console.WriteLine("Finished reading allStats for: " + filepath);
                }
                else
                {
                    Console.WriteLine("AllStats already found, skipping: " + filepath);
                }
            }
        }

        private static List<string> GetFilepathsFromInputDataFile()
        {
            if (string.IsNullOrWhiteSpace(inputDataFilepathsFile))
            {
                return new List<string>();
            }

            return File.ReadAllLines(inputDataFilepathsFile).ToList();
        }

        private static OverviewInfo ReadOverviewTxtFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                Console.WriteLine(string.Concat("Overview .txt file not found, exiting: ", filepath));
                return null;
            }

            string[] lines = File.ReadAllLines(filepath);

            // remove inline comments
            for (int i = 0; i < lines.Count(); i++)
            {
                for (int j = 0; j < lines[i].Count(); j++)
                {
                    if (lines[i][j] == '/' && j + 1 < lines[i].Count() && lines[i][j + 1] == '/')
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
            OverviewInfo overviewInfo = GetOverviewInfo(allStatsList);

            if (overviewInfo != null)
            {
                foreach (var heatmapType in heatmapsToGenerate)
                {
                    Console.WriteLine(string.Concat("Creating heatmap: ", heatmapType));

                    Image bmp = new Bitmap(1024, 1024);

                    using (var graphics = Graphics.FromImage(bmp))
                    {
                        graphics.SmoothingMode =
                            System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

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

                            // save the images (if wingman, only one of the pointsData will contain data)
                            if (pointsDataASite.DataForPoint1X != null || pointsDataASite.DataForPoint1Y != null || pointsDataASite.DataForPoint2X != null || pointsDataASite.DataForPoint2Y != null)
                            {
                                if (allStatsList.FirstOrDefault().mapInfo.GameMode.ToLower() == "wingman")
                                {
                                    SaveImagePngObjective(overviewInfo, allStatsList, bmp, pointsDataASite, aSiteOutputFilepath, aSiteOutputOverviewFilepath);
                                }
                                else
                                {
                                    Console.WriteLine("No data for pointsDataASite even though gamemode is not wingman");
                                }
                            }

                            if (pointsDataBSite.DataForPoint1X != null || pointsDataBSite.DataForPoint1Y != null || pointsDataBSite.DataForPoint2X != null || pointsDataBSite.DataForPoint2Y != null)
                            {
                                if (allStatsList.FirstOrDefault().mapInfo.GameMode.ToLower() == "wingman")
                                {
                                    SaveImagePngObjective(overviewInfo, allStatsList, bmp, pointsDataBSite, bSiteOutputFilepath, bSiteOutputOverviewFilepath);
                                }
                                else
                                {
                                    Console.WriteLine("No data for pointsDataBSite even though gamemode is not wingman");
                                }
                            }

                            /*outputFilepath += ".png";
                            SaveImagePng(bmp, outputFilepath);*/
                        }
                        else if (heatmapType == HeatmapTypeNames.HostageRescueLocations)
                        {
                            var rescueZoneOutputFilepath = outputFilepath + "_rescue_zone.png";
                            var rescueZoneOutputOverviewFilepath = outputFilepath + "_rescue_zone_overview.png";

                            var rescueZone = allStatsList.FirstOrDefault().rescueZoneStats.FirstOrDefault();

                            PointsData pointsDataRescueZone = new PointsData()
                            {
                                DataForPoint1X = rescueZone.XPositionMin,
                                DataForPoint1Y = rescueZone.YPositionMin,
                                DataForPoint2X = rescueZone.XPositionMax,
                                DataForPoint2Y = rescueZone.YPositionMax,
                            };

                            SaveImagePngObjective(overviewInfo, allStatsList, bmp, pointsDataRescueZone, rescueZoneOutputFilepath, rescueZoneOutputOverviewFilepath);

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

        private static void SaveImagePngObjective(OverviewInfo overviewInfo, List<AllStats> allStatsList, Image img, PointsData pointsData, string filepathObjective, string filepathObjectiveOverview)
        {
            var overviewFilepath = string.Concat(overviewFilesDirectory, allStatsList.FirstOrDefault().mapInfo.MapName, "_radar.png");

            if (File.Exists(overviewFilepath))
            {
                Rectangle cropObjective = new Rectangle();

                try
                {
                    Bitmap overviewImage = new Bitmap(overviewFilepath);
                    //overviewImage.SetResolution(96, 96);

                    var marginMultiplier = 10;
                    cropObjective = heatmapLogicCenter.CreateRectangleObjectiveSquarePadding(overviewInfo, pointsData, overviewImage, marginMultiplier);

                    Image bmpCropObjective = ((Bitmap)img).Clone(cropObjective, img.PixelFormat);
                    Image bmpCropObjectiveOverview = overviewImage.Clone(cropObjective, img.PixelFormat);

                    //bmpCropASite.SetResolution(1024, 1024);
                    //bmpCropBSite.SetResolution(1024, 1024);

                    SaveImagePng(bmpCropObjective, filepathObjective);
                    SaveImagePng(bmpCropObjectiveOverview, filepathObjectiveOverview);
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an issue copping and saving images in SaveImagePngObjective().");
                    Console.WriteLine(string.Concat("cropObjective: ", cropObjective));
                }
            }
            else
            {
                Console.WriteLine(string.Concat("Overview .png file not found, exiting: ", overviewFilepath));
            }
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
            return ReadOverviewTxtFile(string.Concat(overviewFilesDirectory, allStatsList.FirstOrDefault().mapInfo.MapName, ".txt"));
        }
    }
}
