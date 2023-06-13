# PangGame

Extra credits I did and did not implement:

1. Architecture designed with MVC paradigm +++ - Not implemented. The Version of MVC demonstrated in the linked article is, in my opinion,
a bad way of doing things and I didn't want to use a different definition because I've seen the definition of "MVC" vary quite widely

2. Three or more distinct consecutive levels, with increasing difficulty +++ - Implemented.
I used scriptable objects for many things in this project, this makes adding levels and hazard types quite trivial.
See Assets/Settings/LevelSettings to see the existing level definitions

3. Custom visuals and shaders ++ - Semi implemented. I used an asset I found on the asset store
but didn't bother with custom shaders given lack of time.
The asset pack I used:
https://assetstore.unity.com/packages/2d/environments/free-platform-game-assets-85838

4. Custom soundtracks and SFX + - Not implemented. Mainly due to lack of time.

5. Two-player, same screen local multiplayer ++ - Implemented. This was a great way to demonstrate the value of a properly designed input system.
See InputSystem.cs

6. Leaderboards with player names supplied at the end of the game + - Semi implemented. The scores are session specific and don't include names.
