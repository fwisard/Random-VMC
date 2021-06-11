using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

/*
 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>. 
*/

namespace random_vmc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int NumPresets = 5;
        private readonly FolderBrowserDialog browse = new FolderBrowserDialog();
        private readonly Random Rnd = new Random();
        private Version version;
        public MainWindow()
        {
            InitializeComponent();

            tbPath.Text = Properties.Settings.Default.PresetsPath;
            if (tbPath.Text == "")
            {
                var TempPath = System.IO.Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), "Microsoft Flight Simulator");

                if (Directory.Exists(TempPath))
                {
                    tbPath.Text = System.IO.Path.Combine(TempPath, "Weather\\Presets");
                }
                else
                {
                    TempPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Packages\\Microsoft.FlightSimulator_8wekyb3d8bbwe\\LocalState");
                    if (Directory.Exists(TempPath))
                    {
                        tbPath.Text = System.IO.Path.Combine(TempPath, "Weather\\Presets");
                    }
                    else
                    {
                        tbPath.Text = "Unable to detect your Presets path, please set it manually";
                    }
                }
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
           /* DateTime buildDate = new DateTime(2000, 1, 1)
                                    .AddDays(version.Build).AddSeconds(version.Revision * 2);*/


        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            
            browse.ShowDialog();
            tbPath.Text = browse.SelectedPath;
        }

        private int CreatePreset(string Climate, string Path, int Number)
        {
            string f = $"{Path}\\a-random-vmc-{Climate}{Number:D2}.WPR";

            var CoverMax = 1000;
            var CoverMin = 0;
            var GustDir = 0.0;
            var GustInterval = 0.0;
            var GustDuration = 0.0;
            var GustSpeed = 0.0;
            var GustProb = 0.1;
            var Aerosol = 0.0;
            var SnowCover = 0.0;
            var Temperature = 273.15;
            var Precipitations = 0.0;

            switch (Climate)
            {
                case "summer":
                    if (Rnd.NextDouble() > 0.5)
                    {
                        CoverMax = 200;
                        CoverMin = 0;
                        GustProb = 0.01;
                    }
                    GustProb = 0.02; // we want a nice weather
                    Temperature += Rnd.Next(22, 37); 
                    if (CoverMax == 200)
                    {
                        Aerosol = Rnd.NextDouble();
                    }
                    else
                    {
                        Aerosol = Rnd.NextDouble() * 6;
                    }
                    break;

                case "snow":
                   if (Rnd.NextDouble() > 0.7)
                   {
                       CoverMax = 200;
                       CoverMin = 0;
                   }
                   Temperature = Rnd.Next(250, 280);
                    SnowCover = 0.1 + (Rnd.NextDouble() * 2.5);
                    Aerosol = Rnd.NextDouble() * 4;
                    break;

                case "cold":
                 if (Rnd.NextDouble() > 0.5)
                    {
                        CoverMin = 200;
                        CoverMax = 1000;
                    }
                    Temperature += Rnd.Next(5, 19);
                    Aerosol = Rnd.NextDouble() * 13;
                    break;

                case "tropics":
                    //tropical weather
                    Temperature += Rnd.Next(25, 45);
                    if (Rnd.NextDouble() > 0.3)
                    {
                        Aerosol = 2 + Rnd.NextDouble() * 11;
                    }
                    else
                    {
                        Aerosol = Rnd.NextDouble() * 2;
                    }
                    break;

                default:
                    break;
            }

            var Density1 = Rnd.Next(CoverMin, CoverMax) / 1000.0;
            var Density2 = Rnd.Next(CoverMin, CoverMax) / 1000.0;
            var Density3 = Rnd.Next(CoverMin, CoverMax) / 1000.0;
            var Cloud1Bot = Rnd.Next(700, 2000);
            var Cloud1Top = Cloud1Bot + Rnd.Next(200, 2000);
            var Scattering1 = Rnd.NextDouble() * 0.8;

            if (Density1 > 0.5 && Rnd.NextDouble() > 0.7)
            {
               
                Precipitations = Rnd.NextDouble() * 0.4;
               
            }
            if (Climate == "tropics" && (Density1 + Density2) > 0.4)
            {
                Precipitations = 0.2 + (Rnd.NextDouble() / 2.0);
                Aerosol = Rnd.NextDouble() * 2.0;
            }

            var Cloud2Bot = Rnd.Next(1500, 6500);
            var Cloud2Top = Cloud2Bot + Rnd.Next(1000, 5000);
            var Scattering2 = Rnd.NextDouble();

            var Cloud3Bot = Rnd.Next(5000, 13000);
            var Cloud3Top = Cloud3Bot + Rnd.Next(100, 2900);
            var Scattering3 = Rnd.NextDouble();

            var Wind1Dir = Rnd.NextDouble() * 360;
            var Wind1Speed = Rnd.NextDouble() * 12;
            if (GustProb > Rnd.NextDouble())
            {
                GustDir = (Wind1Dir - 60 + Rnd.Next(120)) % 360;
                GustInterval = 20 + (Rnd.NextDouble() * 120);
                GustDuration = Rnd.Next(1, 10);
                GustSpeed = 0.1 + (Rnd.NextDouble() * 0.8 * Wind1Speed);
            }
            else
            {
                GustDir = Wind1Dir;
                GustInterval = 0;
                GustDuration = 0;
                GustSpeed = 0;
            }
            var Wind2Alt = Cloud3Bot;
            var Wind2Dir = Rnd.NextDouble() * 360;
            var Wind2Speed = Wind1Speed * (Rnd.NextDouble() + 1.5);

            var Pressure = 104000.0 - (300 * Wind1Speed); // Yeah I know, this is backwards...

            try
            {
                if (File.Exists(f))
                {
                    File.Delete(f);
                }
                using (StreamWriter SW = File.CreateText(f))
                {
                    SW.WriteLine($@"<?xml version=""1.0"" encoding=""UTF-8""?>
<SimBase.Document Type=""WeatherPreset"" version=""1,3"">
    <Descr>AceXML Document</Descr>
    <!-- Created by Random-VMC {version} -->
    <WeatherPreset.Preset> 
        <Name>a-random-VMC-{Climate}-{Number:D2}</Name>
        <Image>weather/presets/live.jpg</Image>
        <Icon>weather/presets/live.svg</Icon>
        <LayeredImage>weather/presets/live_layer.png</LayeredImage>
        <IsAltitudeAMGL>True</IsAltitudeAMGL>
        <CloudLayer>
            <CloudLayerDensity Value=""{Density1:F3}"" Unit=""(0 - 1)"">
            </CloudLayerDensity>
            <CloudLayerAltitudeBot Value=""{Cloud1Bot:F3}"" Unit=""m"">
            </CloudLayerAltitudeBot>
            <CloudLayerAltitudeTop Value=""{Cloud1Top:F3}"" Unit=""m"">
            </CloudLayerAltitudeTop>
            <CloudLayerScattering Value=""{Scattering1:F3}"" Unit=""(0 - 1)"">
            </CloudLayerScattering>
        </CloudLayer>
        <CloudLayer>
            <CloudLayerDensity Value=""{Density2:F3}"" Unit=""(0 - 1)"">
            </CloudLayerDensity>
            <CloudLayerAltitudeBot Value=""{Cloud2Bot:F3}"" Unit=""m"">
            </CloudLayerAltitudeBot>
            <CloudLayerAltitudeTop Value=""{Cloud2Top:F3}"" Unit=""m"">
            </CloudLayerAltitudeTop>
            <CloudLayerScattering Value=""{Scattering2:F3}"" Unit=""(0 - 1)"">
            </CloudLayerScattering>
        </CloudLayer>
        <CloudLayer>
            <CloudLayerDensity Value=""{Density3:F3}"" Unit=""(0 - 1)"">
            </CloudLayerDensity>
            <CloudLayerAltitudeBot Value=""{Cloud3Bot:F3}"" Unit=""m"">
            </CloudLayerAltitudeBot>
            <CloudLayerAltitudeTop Value=""{Cloud3Top:F3}"" Unit=""m"">
            </CloudLayerAltitudeTop>
            <CloudLayerScattering Value=""{Scattering3:F3}"" Unit=""(0 - 1)"">
            </CloudLayerScattering>
        </CloudLayer>
        <WindLayer>
            <WindLayerAltitude Value=""0.000"" Unit=""m"">
            </WindLayerAltitude>
            <WindLayerAngle Value=""{Wind1Dir:F3}"" Unit=""degrees"">
            </WindLayerAngle>
            <WindLayerSpeed Value=""{Wind1Speed:F3}"" Unit=""knts"">
            </WindLayerSpeed>
                <GustWave>
                <GustWaveInterval Value=""{GustInterval:F3}"" Unit=""sec""></GustWaveInterval>
                <GustWaveDuration Value=""{GustDuration:F3}"" Unit=""sec""></GustWaveDuration>
                <GustWaveSpeed Value=""{GustSpeed:F3}"" Unit=""knts""></GustWaveSpeed>
                <GustAngle Value=""{GustDir:F3}"" Unit=""degrees""></GustAngle>
            </GustWave>
        </WindLayer>
        <WindLayer>
            <WindLayerAltitude Value=""{Wind2Alt:F3}"" Unit=""m"">
            </WindLayerAltitude>
            <WindLayerAngle Value=""{Wind2Dir:F3}"" Unit=""degrees"">
            </WindLayerAngle>
            <WindLayerSpeed Value=""{Wind2Speed:F3}"" Unit=""knts"">
            </WindLayerSpeed>
        </WindLayer>
        <MSLPressure Value=""{Pressure:F3}"" Unit=""pa"">
        </MSLPressure>
        <MSLTemperature Value=""{Temperature:F3}"" Unit=""k"">
        </MSLTemperature>
        <AerosolDensity Value=""{Aerosol:F3}"" Unit=""m"">
        </AerosolDensity>
        <Precipitations Value=""{Precipitations:F3}"" Unit=""mm/h"">
        </Precipitations>
        <SnowCover Value=""{SnowCover:F3}"" Unit=""m"">
        </SnowCover>
        <ThunderstormIntensity Value=""0.000"" Unit=""(0 - 1)"">
        </ThunderstormIntensity>
    </WeatherPreset.Preset>
</SimBase.Document>");

                }
                return 1;

            }
            catch (Exception Ex)
            {
                System.Windows.MessageBox.Show(Ex.Message);
                return 0;
            }
        }
        
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            int CreatedFiles = 0;
            NumPresets = (int)sldNum.Value;
            if (Directory.Exists(tbPath.Text))
            {
                Properties.Settings.Default.PresetsPath = tbPath.Text;
                Properties.Settings.Default.Save();
                for (var i = 1; i <= NumPresets; i++)
                {
                    if ((bool) cbSnow.IsChecked)
                    {
                        CreatedFiles += CreatePreset("snow", tbPath.Text, i);
                    }
                    if ((bool) cbCold.IsChecked)
                    {
                        CreatedFiles += CreatePreset("cold", tbPath.Text, i);
                    }
                    if ((bool)cbSummer.IsChecked)
                    {
                        CreatedFiles += CreatePreset("summer", tbPath.Text, i);
                    }
                    if ((bool)cbTropics.IsChecked)
                    {
                        CreatedFiles += CreatePreset("tropics", tbPath.Text, i);
                    }
                }
                if (CreatedFiles > 0)
                {
                    System.Windows.MessageBox.Show($"Successfully created {CreatedFiles} presets in {tbPath.Text}", "Success", MessageBoxButton.OK,MessageBoxImage.Asterisk);
                }
                else
                {
                    System.Windows.MessageBox.Show($"No files created", "Fail", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                }
            }
            else
            {
                System.Windows.MessageBox.Show("Oops, directory doesn't exist! Please set the correct path to your MSFS Weather Presets","Fail", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ImgLogo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show($"Random-VMC v{version}\nCopyright 2021 fwisard\n   This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.\n   This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.\n   You should have received a copy of the GNU General Public License along with this program.  If not, see https://www.gnu.org/licenses/.", 
                "About",MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}
