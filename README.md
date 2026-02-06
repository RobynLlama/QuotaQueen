# QuotaQueen

Quota Queen for YAPYAP is a mod designed to allow the game host to modify various aspects of the game's difficulty and progression.

Note: Only the host needs to run the mod, the host controls all the quota and dungeon related variables. Quota Queen will shutdown automatically when run in client mode to avoid desyncing from the host.

## Tweakables

The following properties can be adjusted in the mod's configuration file. All of the defaults are set to match the game's defaults.

### Round Settings

| Property | Default | Description |
| --- | --- | --- |
| **RoundDuration** | `720` | The length of each day in seconds. |

### Quota Settings

| Property | Default | Description |
| --- | --- | --- |
| **QuotaDays** | `3` | The number of days allowed to meet each quota. |
| **GoldReward** | `25` | The base gold reward for meeting a quota. Additional gold is awarded for exceeding the target. |

### Death Settings

| Property | Default | Description |
| --- | --- | --- |
| **UsePCTPenalty** | `true` | If enabled, the game uses a percentage-based penalty. If disabled, it uses a flat value penalty. |
| **DeathPenaltyFlat** | `100` | The amount of score removed per dead player when using flat penalty mode. |
| **DeathPenaltyPCT** | `0.1` (10%) | The percentage of total score removed per dead player when using percentage penalty mode. |

## Credits

Author: Robyn

Laurel Image by [Freepik](https://www.flaticon.com/free-icon/laurel-wreath_2502739)
