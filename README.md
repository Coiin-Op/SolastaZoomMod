# Solasta Zoom Out Plus

This Unity Mod Manager mod increases the maximum camera orbit radius to 40 (about 67% beyond the 24 radius used by Wider Camera) while you are in a game location. It tilts the camera about 12 degrees farther downward than Wider Camera's original angle while keeping the same zoom distance. It updates Solasta's configured orbit defaults as well as each camera's initial orbit, so the game's own orbit adjustment preserves the expanded range without repeated camera writes.

## Build

The project references the Solasta assemblies in the game installation path used when this mod was prepared. If Solasta is installed elsewhere, update the four `HintPath` values in `SolastaZoomMod.csproj`.

Build with:

```powershell
dotnet build -c Release
```

## Install

Zip `Info.json` and the built `SolastaZoomMod.dll` together at the root of the archive, then drop the zip onto Unity Mod Manager's Mods tab. Disable **Wider Camera** to avoid two mods writing camera orbit settings.

The maximum zoom out is increased by changing the camera's outer orbit radius. Terrain and room visibility remain controlled by the game.
