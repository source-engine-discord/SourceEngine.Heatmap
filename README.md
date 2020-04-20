# SourceEngine.Heatmap
Outputs heatmap overlay images and JSON information files from CS:GO parsed demos

### Usage

```
	-inputdatadirectory            [path]                              		The folder location of the input json data files (parsed demo data). (Optional if -inputdatafilepathsfile provided)
	-inputdatafilepathsfile        [path]                              		The file location of the text file containing a list of filepaths that contain input json data (parsed demo data). (Optional if -inputdatadirectory provided)
	-overviewfilesdirectory        [path]                              		The folder location of the overview files (required if generating BombplantLocations or HostageRescueLocations heatmaps)
	-heatmapjsondirectory          [path]                              		The folder location of the json for the previously created heatmap files.
	-outputheatmapdirectory        [path]                              		The folder location to output the generated heatmaps.
	-heatmapstogenerate            [heatmap types (space seperated)]		A list of heatmap key names to generate.
```

Heatmap Types
```
    // Generate All Heatmaps
    All

    // Kills - Team Sides
    TKills
    TKillsBeforeBombplant
    TKillsAfterBombplant
    TKillsBeforeHostageTaken
    TKillsAfterHostageTaken
    CTKills
    CTKillsBeforeBombplant
    CTKillsAfterBombplant
    CTKillsBeforeHostageTaken
    CTKillsAfterHostageTaken

    // Kills - Weapon Types
    PistolKills
    SmgKills
    LmgKills
    ShotgunKills
    AssaultrifleKills
    SniperKills
    GrenadeKills
    ZeusKills
    KnifeKills
    EquipmentKills

    // Kills - Random
    WallbangKills

    // Positions - Players By Team
    PlayerPositionsByTeam
    CampingSpotsByTeam
    FirstKillPositionsByTeam

    // Locations - Objectives
    BombplantLocations
    HostageRescueLocations

    // Locations - Grenades
    SmokeGrenadeLocations
    FlashGrenadeLocations
    HeGrenadeLocations
    IncendiaryGrenadeLocations
    DecoyGrenadeLocations
```

Example:

```
SourceEngine.Heatmap.Generator -inputdatadirectory "foldername" -inputdatafilepathsfile "filepathsTxtFile" -overviewfilesdirectory "foldername" -heatmapjsondirectory "foldername" -outputheatmapdirectory "foldername" -heatmapstogenerate ctkills tkills assaultriflekills
```