# TODO
## Engine
- MainMenu: implement functionality for Credits Button
- MainMenu: LobbyMenu visuell an andere Menus anpassen
- Options Menu: implement options for Camera Controls
- Act 2: fix Light Probes
- Act 3_4: Kollisionen mit Deko sind akward (Spieler wird hochgedrückt)
- Act 5: Tiles neu platzieren (sind alle krumm und schief)
- Act 5: Gastank Wänden fehlt tlw. Kollision (außerdem kein Prefab)
- viel mehr Objekte interagierbar machen (vor allem Grabable)
- Prefabs überprüfen und unnötige Collider (vor allem in Deko) löschen
- Outlines for Interactables (ObjectContent) after Act 1

## Programming
- fix Error bei Dialogen
- replace InputSettings with Unity build in solution
- Interaktion überarbeiten
	- Interaktion Prompt
	- nicht an ArmTool knüpfen (bzw. permanent aktiv)
	- sollte über Proximity funktionieren
- ForceTool Prompts bei Benutzung einfügen
- Cut Scenes generell skipbar machen (aktuell nur schön bei Act 1 Ende)
	- UI für Skip einbauen

## Art
- Sprites für UI Buttons im Hauptmenu

## Postprocessing
- Postprocess Layer on camera should not be set to layermask "Everything"
	- check all PP elements throughout the game in order to reduce required layermask to specific layers
- Tweak camera PP volume effects to reduce visual bombardment (f.e. bloom 2 -> 1)

## Music
- Normalize all music tracks (stark differences in volume)
- Reduce volume of background drones in most tracks (heavily used, drowns out instrumentation, example: GulianTheme_01_TL)

## SFX
- Sound für Tür wird aufgeschlossen finden und einbauen

# Backlog
- mehr Dekoobjekte platzieren
- Resonance3 scheint nicht verwendet zu werden (Audio und Csv sind da), könnte noch implementiert werden
	- in Akt3_4 gab es eine ungenutzte Resonance, vlt gehört die dahin
- es gibt Dialoge nach Resonanzen, die afaik aktuell nicht verwendet werden