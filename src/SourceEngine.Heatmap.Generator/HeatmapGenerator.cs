using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SourceEngine.Demo.Stats.Models;
using SourceEngine.Heatmap.Generator;
using SourceEngine.Heatmap.Generator.Constants;
using SourceEngine.Heatmap.Generator.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace SourceEngine.Demo.Heatmaps
{
    public class HeatmapGenerator
    {
        private static HeatmapTypeDataGatherer heatmapTypeDataGatherer = new HeatmapTypeDataGatherer();
        private static HeatmapLogicCenter heatmapLogicCenter = new HeatmapLogicCenter();
        private static ConsoleMessageStyler consoleMessageStyler = new ConsoleMessageStyler();

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
            var errorMessage = "-overviewfilesdirectory required if generating BombplantLocations or HostageRescueLocations heatmaps.";
            consoleMessageStyler.PrintErrorMessage(errorMessage);
        }

        private static void helpTextInvalidHeatmapNameProvided(string invalidHeatmapName)
        {
            var errorMessage = string.Concat("Invalid heatmap name provided: ", invalidHeatmapName);
            consoleMessageStyler.PrintErrorMessage(errorMessage);
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

            var successMessage = "Finished generating heatmaps.";
            PrintSuccessMessage(successMessage);
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

        private static void PrintSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void RunHeatmapGenerator(List<string> heatmapsToGenerate)
        {
            var allStatsFilepathsFromDirectory = new List<string>();
            var allStatsFilepathsFromTxtFile = new List<string>();

            if (!string.IsNullOrWhiteSpace(inputDataDirectory))
            {
                allStatsFilepathsFromDirectory = Directory.GetFiles(inputDataDirectory).ToList();
            }
            if (!string.IsNullOrWhiteSpace(inputDataFilepathsFile))
            {
                allStatsFilepathsFromTxtFile = GetFilepathsFromInputDataFile();
            }

            var allOutputDataList = new List<AllOutputData>();

            var allStatsMatchIdsDone = new List<string>();
            ParseJson(allOutputDataList, allStatsMatchIdsDone, allStatsFilepathsFromDirectory);
            ParseJson(allOutputDataList, allStatsMatchIdsDone, allStatsFilepathsFromTxtFile); // prioritises json from filepathsFromDirectory, so only new matches' data will be added

            if (allOutputDataList.Count() > 0)
            {
                var firstAllStats = allOutputDataList.FirstOrDefault().AllStats;
                var heatmapDataFilename = string.Concat(heatmapJsonDirectory, firstAllStats.mapInfo.MapName, Filenames.HeatmapDataFilenameEnding);
                CreateFileIfDoesntExist(heatmapDataFilename);
                MapHeatmapData heatmapData = new MapHeatmapData();

                // read the current contents of heatmapData containing old parsed demo information, and write the new parsed demo information to it (overwriting any recurring ones)
                var fileAccessible = CheckFileIsNotLocked(heatmapDataFilename, false, true, false);
                if (fileAccessible)
                {
                    heatmapData = ReadJsonFile<MapHeatmapData>(typeof(MapHeatmapData), heatmapDataFilename); // read previously parsed json from the heatmapData.json file for this map

                    if (heatmapData == null)
                    {
                        heatmapData = new MapHeatmapData() { AllOutputDataList = new List<AllOutputData>() };
                    }

                    // add newly parsed demo data into heatmap data json file
                    foreach (var allOutputData in allOutputDataList)
                    {
                        heatmapData.AllOutputDataList.RemoveAll(x => x.AllStats.mapInfo.DemoName == allOutputData.AllStats.mapInfo.DemoName); // replace matches that appear in the previously created heatmap files with the newly parsed information in allStatsList
                        heatmapData.AllOutputDataList.Add(allOutputData);
                    }

                    OverwriteJsonFile(heatmapData, heatmapDataFilename); // output all parsed json data to the map's heatmapdata.json file
                }

                // create the heatmaps after checking for incompatible heatmaps requested
                if (heatmapData.AllOutputDataList?.Count() > 0)
                {
                    if (heatmapsToGenerate.Any(x => x.ToLower() == "all"))
                    {
                        heatmapsToGenerate = validHeatmapTypeNames;
                    }

                    // remove unnecessary defuse specific or hostage specific heatmaps for the map
                    if (allOutputDataList.FirstOrDefault().AllStats.mapInfo.GameMode.ToLower() == "defuse" || allOutputDataList.FirstOrDefault().AllStats.rescueZoneStats.All(x => x.XPositionMin == null))
                    {
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsBeforeHostageTaken.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsAfterHostageTaken.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsBeforeHostageTaken.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsAfterHostageTaken.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.HostageRescueLocations.ToString());
                    }
                    else if (allOutputDataList.FirstOrDefault().AllStats.mapInfo.GameMode.ToLower() == "hostage" || allOutputDataList.FirstOrDefault().AllStats.bombsiteStats.All(x => x.XPositionMin == null))
                    {
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsBeforeBombplant.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsAfterBombplantASite.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.TKillsAfterBombplantBSite.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsBeforeBombplant.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsAfterBombplantASite.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.CTKillsAfterBombplantBSite.ToString());
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.BombPlantLocations.ToString());
                    }

                    // always put playerpositionsbyteam heatmap last as it takes much longer than the others
                    if (heatmapsToGenerate.Any(x => x == HeatmapTypeNames.PlayerPositionsByTeam.ToString()))
                    {
                        heatmapsToGenerate.RemoveAll(x => x == HeatmapTypeNames.PlayerPositionsByTeam.ToString());
                        heatmapsToGenerate.Add(HeatmapTypeNames.PlayerPositionsByTeam.ToString());
                    }

                    CreateHeatmaps(heatmapsToGenerate, heatmapData.AllOutputDataList);
                }
                else
                {
                    var errorMessage = "No AllStats instances (parsed demo data) found.";
                    consoleMessageStyler.PrintErrorMessage(errorMessage);
                }
            }
            else
            {
                var errorMessage = "No files found.";
                consoleMessageStyler.PrintErrorMessage(errorMessage);
            }
        }

        private static void ParseJson(List<AllOutputData> allOutputDataList, List<string> allStatsMatchIdsDone, List<string> allStatsFilepaths)
        {
            foreach (var filepath in allStatsFilepaths.Where(x => !x.Contains("playerpositions")))
            {
                var playerPositionsStatsFilepath = string.Empty;

                try
                {
                    var allOutputData = new AllOutputData();

                    var allStats = ReadJsonFile<AllStats>(typeof(AllStats), filepath);

                    var splitFilepath = filepath.Split(".json");
                    playerPositionsStatsFilepath = string.Concat(splitFilepath[0], "_playerpositions.json");

                    PlayerPositionsStats playerPositionsStats = null;
                    if (File.Exists(playerPositionsStatsFilepath)) // if a PlayerPositionsStats json file has been provided
                    {
                        playerPositionsStats = ReadJsonFile<PlayerPositionsStats>(typeof(PlayerPositionsStats), playerPositionsStatsFilepath);
                    }

                    if (!allStatsMatchIdsDone.Contains(allStats.mapInfo.DemoName))
                    {
                        allStatsMatchIdsDone.Add(allStats.mapInfo.DemoName);

                        allOutputData.AllStats = allStats;
                        allOutputData.PlayerPositionsStats = playerPositionsStats;

                        allOutputDataList.Add(allOutputData);

                        Console.WriteLine("Finished reading allStats for: " + filepath);
                    }
                    else
                    {
                        Console.WriteLine("AllStats already found, skipping: " + filepath);
                    }
                }
                catch
                {
                    var errorMessage = string.Concat("Failed to parse json. AllStats filepath: ", filepath, "PlayerPositionsStats filepath: ", playerPositionsStatsFilepath);
                    consoleMessageStyler.PrintErrorMessage(errorMessage);
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
                var errorMessage = string.Concat("Overview .txt file not found, exiting: ", filepath);
                consoleMessageStyler.PrintErrorMessage(errorMessage);

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
            var fileAccessible = CheckFileIsNotLocked(filepath, false);

            if (fileAccessible)
            {
                File.WriteAllText(filepath, string.Empty);
                File.WriteAllText(filepath, JsonConvert.SerializeObject(fileContents, Formatting.Indented));
            }
        }

        private static bool CheckFileIsNotLocked(string filepath, bool isImageFile, bool checkRead = true, bool checkWrite = true, int maxRetries = 20, int waitTimeSeconds = 5)
        {
            CreateFileIfDoesntExist(filepath);

            var fileReadable = false;
            var fileWriteable = false;

            var retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    if (checkRead)
                    {
                        using (FileStream fs = File.OpenRead(filepath))
                        {
                            if (fs.CanRead)
                            {
                                fileReadable = true;
                            }
                        }
                    }
                    if (checkWrite)
                    {
                        using (FileStream fs = File.OpenWrite(filepath))
                        {
                            if (fs.CanWrite)
                            {
                                fileWriteable = true;
                            }
                        }
                    }

                    if ((!checkRead || fileReadable) && (!checkWrite || fileWriteable))
                    {
                        return true;
                    }
                }
                catch { }

                retries++;

                if (retries < maxRetries)
                {
                    var warningMessage = string.Concat("File has been locked ", retries, " time(s). Waiting ", waitTimeSeconds, " seconds before trying again. Filepath: ", filepath);
                    consoleMessageStyler.PrintWarningMessage(warningMessage);

                    Thread.Sleep(waitTimeSeconds * 1000);
                    continue;
                }
            }

            var errorMessage = string.Concat("SKIPPING! File has been locked ", maxRetries, " times ");
            if (isImageFile)
                errorMessage += string.Concat("(heatmap image will not be created/updated).");
            else
                errorMessage += string.Concat("(this may result in lost data in future if not rerun).");

            errorMessage += string.Concat(" Filepath: ", filepath);
            consoleMessageStyler.PrintErrorMessage(errorMessage);

            return false;
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

        private static void CreateHeatmaps(List<string> heatmapsToGenerate, List<AllOutputData> allOutputDataList)
        {
            Console.WriteLine(string.Concat("Creating heatmaps for map: ", allOutputDataList.FirstOrDefault().AllStats.mapInfo.MapName));

            OverviewInfo overviewInfo = GetOverviewInfo(allOutputDataList);

            if (overviewInfo != null)
            {
                foreach (var heatmapType in heatmapsToGenerate)
                {
                    Console.WriteLine(string.Concat("Creating heatmap: ", heatmapType));

                    Image bmp = new Bitmap(1024, 1024);

                    using (var graphics = Graphics.FromImage(bmp))
                    {
                        string outputFilepath = string.Concat(outputHeatmapDirectory, allOutputDataList.FirstOrDefault().AllStats.mapInfo.MapName, "_", heatmapType.ToLower());
                        
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;

                        GenerateHeatmapDataByType(heatmapType, overviewInfo, allOutputDataList, graphics);

                        graphics.Save();

                        if (heatmapType == HeatmapTypeNames.BombPlantLocations)
                        {
                            var aSiteOutputFilepath = outputFilepath + "-asite.png";
                            var aSiteOutputOverviewFilepath = outputFilepath + "-asite-overview.png";
                            var bSiteOutputFilepath = outputFilepath + "-bsite.png";
                            var bSiteOutputOverviewFilepath = outputFilepath + "-bsite-overview.png";

                            var bombsiteA = allOutputDataList.FirstOrDefault().AllStats.bombsiteStats.Where(x => x.Bombsite == 'A').FirstOrDefault();
                            var bombsiteB = allOutputDataList.FirstOrDefault().AllStats.bombsiteStats.Where(x => x.Bombsite == 'B').FirstOrDefault();

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
                                SaveImagePngObjective(overviewInfo, allOutputDataList, bmp, pointsDataASite, aSiteOutputFilepath, aSiteOutputOverviewFilepath);
                            }
                            else
                            {
                                if (allOutputDataList.FirstOrDefault().AllStats.mapInfo.GameMode.ToLower() == "defuse")
                                {
                                    var warningMessage = "No data for pointsDataASite even though gamemode is defuse";
                                    consoleMessageStyler.PrintWarningMessage(warningMessage);
                                }
                            }

                            if (pointsDataBSite.DataForPoint1X != null || pointsDataBSite.DataForPoint1Y != null || pointsDataBSite.DataForPoint2X != null || pointsDataBSite.DataForPoint2Y != null)
                            {
                                SaveImagePngObjective(overviewInfo, allOutputDataList, bmp, pointsDataBSite, bSiteOutputFilepath, bSiteOutputOverviewFilepath);
                            }
                            else
                            {
                                if (allOutputDataList.FirstOrDefault().AllStats.mapInfo.GameMode.ToLower() == "defuse")
                                {
                                    var warningMessage = "No data for pointsDataBSite even though gamemode is defuse";
                                    consoleMessageStyler.PrintWarningMessage(warningMessage);
                                }
                            }

                            /*outputFilepath += ".png";
                            SaveImagePng(bmp, outputFilepath);*/
                        }
                        else if (heatmapType == HeatmapTypeNames.HostageRescueLocations)
                        {
                            var rescueZoneOutputFilepath = outputFilepath + "-rescue-zone.png";
                            var rescueZoneOutputOverviewFilepath = outputFilepath + "-rescue-zone-overview.png";

                            var rescueZone = allOutputDataList.FirstOrDefault().AllStats.rescueZoneStats.FirstOrDefault();

                            PointsData pointsDataRescueZone = new PointsData()
                            {
                                DataForPoint1X = rescueZone.XPositionMin,
                                DataForPoint1Y = rescueZone.YPositionMin,
                                DataForPoint2X = rescueZone.XPositionMax,
                                DataForPoint2Y = rescueZone.YPositionMax,
                            };

                            SaveImagePngObjective(overviewInfo, allOutputDataList, bmp, pointsDataRescueZone, rescueZoneOutputFilepath, rescueZoneOutputOverviewFilepath);

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

        private static void GenerateHeatmapDataByType(string heatmapType, OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Graphics graphics)
        {
            heatmapTypeDataGatherer.GenerateByHeatmapType(heatmapType.ToLower(), overviewInfo, allOutputDataList, graphics);
        }

        /// <summary>
        /// Saves a heatmap image as a png
        /// </summary>
        /// <param name="img"></param>
        /// <param name="filepath"></param>
        private static void SaveImagePng(Image img, string filepath)
        {
            var canSave = false;

            // check if the files are locked
            if (File.Exists(filepath))
            {
                var fileAccessible = CheckFileIsNotLocked(filepath, true, true, true);

                if (fileAccessible)
                {
                    canSave = true;
                }
            }
            else
            {
                canSave = true;
            }

            // only create the heatmaps if the files are not locked
            if (canSave)
            {
                img.Save(filepath, ImageFormat.Png);
            }
        }

        /// <summary>
        /// Saves an objective heatmap (BombsiteLocations / HostageRescueLocations) image as a png
        /// </summary>
        /// <param name="overviewInfo"></param>
        /// <param name="allOutputDataList"></param>
        /// <param name="img"></param>
        /// <param name="pointsData"></param>
        /// <param name="filepathObjective"></param>
        /// <param name="filepathObjectiveOverview"></param>
        private static void SaveImagePngObjective(OverviewInfo overviewInfo, List<AllOutputData> allOutputDataList, Image img, PointsData pointsData, string filepathObjective, string filepathObjectiveOverview)
        {
            var canSave = false;

            var overviewFilepath = string.Concat(overviewFilesDirectory, allOutputDataList.FirstOrDefault().AllStats.mapInfo.MapName, "_radar.png");

            // check the overview file exists, cannot do the heatmaps without it
            if (File.Exists(overviewFilepath))
            {
                // check if the files are locked
                if (File.Exists(filepathObjective) || File.Exists(filepathObjectiveOverview))
                {
                    var fileAccessible = CheckFileIsNotLocked(filepathObjective, true, true, true);
                    var fileOverviewAccessible = CheckFileIsNotLocked(filepathObjectiveOverview, true, true, true);

                    if (fileAccessible && fileOverviewAccessible)
                    {
                        canSave = true;
                    }
                }
                else
                {
                    canSave = true;
                }

                // only create the heatmaps if the files are not locked
                if (canSave)
                {
                    Rectangle cropObjective = new Rectangle();

                    try
                    {
                        Bitmap overviewImage = new Bitmap(overviewFilepath);

                        var marginMultiplier = 10;
                        cropObjective = heatmapLogicCenter.CreateRectangleObjectiveSquarePadding(overviewInfo, pointsData, overviewImage, marginMultiplier);

                        Image bmpCropObjective = ((Bitmap)img).Clone(cropObjective, img.PixelFormat);
                        Image bmpCropObjectiveOverview = overviewImage.Clone(cropObjective, img.PixelFormat);

                        bmpCropObjective = ResizeImage(bmpCropObjective, 256, 256);
                        bmpCropObjectiveOverview = ResizeImage(bmpCropObjectiveOverview, 256, 256);

                        SaveImagePng(bmpCropObjective, filepathObjective);
                        SaveImagePng(bmpCropObjectiveOverview, filepathObjectiveOverview);
                    }
                    catch
                    {
                        var errorMessage = string.Concat("There was an issue copping and saving images in SaveImagePngObjective(). cropObjective: ", cropObjective);
                        consoleMessageStyler.PrintErrorMessage(errorMessage);
                    }
                }
            }
            else
            {
                var errorMessage = string.Concat("Overview .png file not found, cannot create objective heatmaps. Filepath: ", overviewFilepath);
                consoleMessageStyler.PrintErrorMessage(errorMessage);
            }
        }

        /// <summary>
        /// Resizes the image provided to a specified width and height
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
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

        private static OverviewInfo GetOverviewInfo(List<AllOutputData> allOutputDataList)
        {
            return ReadOverviewTxtFile(string.Concat(overviewFilesDirectory, allOutputDataList.FirstOrDefault().AllStats.mapInfo.MapName, ".txt"));
        }
    }
}
