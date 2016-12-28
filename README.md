# README #

This is a small collection of assorted tools that I wrote for use in my own projects.

The code comes without warranty of any kind.

One thing you may find interesting is Exporter that is present in Editor/ExportScripts folder.
The export utility was designed to transfer unity engine scene into Unreal 4.

### using exporter ###

To export scene, objects you want to export must be parented to some sort of "root" object.
To export, right click on the root object in hieararchy window, and select either "export selected objects" or "export current object". This will open "safe file" dialog. Resulting json file should be placed into project's root (i.e. into a folder that contains "Assets" subfolder).

For additional information check readme of "JsonImporter" project. There are couple of gotchas, for example, the script will not automatically convert *.tif textures into png, and you should do it yourself.

### Warnings ###

The code is highly experimental, is subject to change (without warning), and comes with no warranties whatsoever. Use at your own risk and have fun.