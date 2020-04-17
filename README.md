# SourceEngine.Heatmap
Outputs heatmap overlay images and JSON information files from CS:GO parsed demos

### Usage

```
	"-inputdatadirectory            [path]                              		The folder location of the input json data files (parsed demo data). (Optional if -inputdatafilepathsfile provided)
	"-inputdatafilepathsfile        [path]                              		The file location of the text file containing a list of filepaths that contain input json data (parsed demo data). (Optional if -inputdatadirectory provided)
	"-heatmapjsondirectory          [path]                              		The folder location of the json for the previously created heatmap files.
	"-outputheatmapdirectory        [path]                              		The folder location to output the generated heatmaps.
	"-heatmapstogenerate            [heatmap types (space seperated)]           A list of heatmap key names to generate.
```

Heatmap Types
```
	// Generate All Heatmaps
	all

	// Kills - Team Sides
	tkills
	tkillsbeforebombplant
	tkillsafterbombplant
    tkillsbeforehostagetaken
    tkillsafterhostagetaken
	ctkills
	ctkillsbeforebombplant
	ctkillsafterbombplant
    ctkillsbeforehostagetaken
    ctkillsafterhostagetaken
	
	// Kills - Weapon Types
	pistolkills
	smgkills
	lmgkills
	shotgunkills
	assaultriflekills
	sniperkills
	grenadekills
	zeuskills
	knifekills
	equipmentkills
	
	// Kills - Random
	wallbangkills
	
	// Positions - Players By Team
	playerpositionsbyteam
	campingspotsbyteam
	firstkillpositionsbyteam
	
	// locations - Objectives
	bombplantlocations
    hostagerescuelocations
```

Example:

```
-inputdatadirectory "foldername" -inputdatafilepathsfile "filepathsTxtFile" -heatmapjsondirectory "foldername" -outputheatmapdirectory "foldername" -heatmapstogenerate ctkills tkills assaultriflekills
```