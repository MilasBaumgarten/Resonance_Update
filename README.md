# TODO
## Engine
- Options Menu: implement options for Camera Controls
- viel mehr Objekte interagierbar machen (vor allem Grabable)
- InGame Optionsmenu im Logbuch

## Programming

## Art

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
- Ladebildschirme einbauen
- Gangelemente anpassen (mittlere Streifen sind bei T und L Stücken nicht vorhanden oder unschön)
- Prefabs überprüfen und unnötige Collider (vor allem in Deko) löschen
- mehr Dekoobjekte platzieren
- Resonance3 scheint nicht verwendet zu werden (Audio und Csv sind da), könnte noch implementiert werden
	- in Akt3_4 gab es eine ungenutzte Resonance, vlt gehört die dahin
- es gibt Dialoge nach Resonanzen, die afaik aktuell nicht verwendet werden
