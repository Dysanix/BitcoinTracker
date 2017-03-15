using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Application = System.Windows.Application;

namespace BitcoinTracker
{
    public partial class App : Application
    {
        public App()
        {
            var configFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> <configuration> <configSections> <sectionGroup name=\"userSettings\" type=\"System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" > <section name=\"BitcoinTracker.Properties.Settings\" type=\"System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\" allowExeDefinition=\"MachineToLocalUser\" requirePermission=\"false\" /> </sectionGroup> </configSections> <system.net> <settings> <httpWebRequest useUnsafeHeaderParsing=\"true\" /> </settings> </system.net> <startup> <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.5.2\" /> </startup> <userSettings> <BitcoinTracker.Properties.Settings> <setting name=\"currentBitcoins\" serializeAs=\"String\"> <value>0</value> </setting> <setting name=\"updateInterval\" serializeAs=\"String\"> <value>1500</value> </setting> <setting name=\"currencyTag\" serializeAs=\"String\"> <value>USD</value> </setting> <setting name=\"inForeground\" serializeAs=\"String\"> <value>True</value> </setting> <setting name=\"inTaskbar\" serializeAs=\"String\"> <value>False</value> </setting> <setting name=\"API\" serializeAs=\"String\"> <value>GDax</value> </setting> <setting name=\"checkForUpdates\" serializeAs=\"String\"> <value>True</value> </setting> <setting name=\"muted\" serializeAs=\"String\"> <value>False</value> </setting> <setting name=\"alertHigh\" serializeAs=\"String\"> <value>0</value> </setting> <setting name=\"alertLow\" serializeAs=\"String\"> <value>0</value> </setting> </BitcoinTracker.Properties.Settings> </userSettings> </configuration>";

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + System.AppDomain.CurrentDomain.FriendlyName + ".config"))
            {
                File.WriteAllText(
                    AppDomain.CurrentDomain.BaseDirectory + "\\" + System.AppDomain.CurrentDomain.FriendlyName +
                    ".config", configFile);
            }
        }
    }
}
