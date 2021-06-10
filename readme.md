# Random-VMC

Random-VMC is a tool to generate random MSFS2020 weather presets suitable for VFR flights at low altitudes. No strong winds, and a good visibility for pleasant flights in light General Aviation aircrafts.

The weather should be realistic, provided that you choose a suitable preset (ie: not *Tropics* for EDDF in January).

## Requirements

* .NET framework 4.7.2
* Microsoft Flight Simulator 2020 (well, you don't *technically* need it until you want to use the generated presets, you might want to use *random-vmc* on another machine)

## Installation

Just run the installer. 

## Configuration

Set the directory where you want your presets to be generated ( *"%APPDATA%\Roaming\Microsoft Flight Simulator\Weather\Presets"* on my Steam version ) if *Random-VMC* doesn't automatically detect yours. Advice: you usually don't want to give the path to the *Community* folder, presets go in their own separate place.

*Random-VMC* will remember the path you set once you have generated your first preset.

## Usage

Check the climate(s) you want, the number of presets (for each climate, this is not a total) and click on the *generate presets* button.
**Caution**: New presets will overwrite the older ones. Be sure to rename the ones you'd like to keep if the need arises.

You can click on the logo to the see "About..." box.

### Climates available

* *Snow*:  Winter in temperate regions or anytime beyond the Arctic or Antartic circle. 
* *Cold*:  Autumn or spring in temperate regions. Don't expect blue skies, you will often get many clouds with this preset.
* *Summer*: Warmer weather with usually calmer winds and few clouds.
* *Tropics*: Usually hot and damp. Better suited for jungle than deserts.

## License

*Random-VMC* is licensed under the GPLv3 License. See the COPYING file for details.
