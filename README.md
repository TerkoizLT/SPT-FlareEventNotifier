# Flare Event Notifier

A tiny mod that notifies the player when a green exfil flare was used correctly.

### How to install

1. Download the latest release here: [link](https://dev.sp-tarkov.com/Terkoiz/FlareEventNotifier/releases) -OR- build from source (instructions below)
2. Simply extract the zip file contents into your root SPT-AKI folder (where EscapeFromTarkov.exe is)
3. Your `BepInEx/plugins` folder should now contain a `Terkoiz.FlareEventNotifier.dll` file inside

### How to build from source

1. Download/clone this repository
2. Open your current SPT directory and copy all files from `\EscapeFromTarkov_Data\Managed` into this solution's `\References\EFT_Managed` folder.
3. Rebuild the project in the Release configuration.
4. Grab the `Terkoiz.FlareEventNotifier.dll` file from the `bin/Release` folder and use it wherever. Refer to the "How to install" section if you need help here.