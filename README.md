# SourceEngine.Heatmap
Outputs heatmap overlay images and JSON information files from CS:GO parsed demos

### Usage

```
	"-inputdatadirectory            [path]                              The folder location of the input data json files (parsed demo data).
	"-heatmapjsondirectory          [path]                              The folder location of the json for the previously created heatmap files.
	"-outputheatmapdirectory        [path]                              The folder location to output the generated heatmaps.
	"-heatmapstogenerate            [heatmap types (space seperated)]           A list of heatmap key names to generate.
```

Heatmap Types
```
	// Kills - Team Sides
	tkills
	ctkills
	
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
	
	// Kills - random
	wallbangkills
	
	// Positions - players by team
	playerpositionsbyteam
	campingspotsbyteam
	firstkillpositionsbyteam
```

Example:

```
-inputdatadirectory "foldername" -heatmapjsondirectory "foldername" -outputheatmapdirectory "foldername" -heatmapstogenerate ctkills tkills assaultriflekills
```