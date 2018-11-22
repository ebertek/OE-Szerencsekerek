# Szerencsekerék
A basic version of [Wheel of Fortune](https://en.wikipedia.org/wiki/Wheel_of_Fortune_(U.S._game_show)#Gameplay) written in C#. Homework assignment for NIXSF1HBEE, 2018.

## Building
* `Szerencsekerek.sln` uses .NET Core, and generates a (cross-platform) `.dll`
* `Szerencsekerek-Win.sln` uses .NET Framework 4.6.1, and generates a (Windows-specific) `.exe`

## Binaries (if you don't want to build the app yourself)
* You can run the cross-platform version in `Release/Core/` like this: `dotnet Szerencsekerek.dll`
* You can run the Windows version in `Release/Windows/` by opening `Szerencsekerek.exe`

## Usage
By default, the app loads `kozmondasok.txt`. If you want to use your own list of puzzles, you can do so by launching the app with an argument like this: `dotnet Szerencsekerek.dll doctor_who.txt` (or `Szerencsekerek doctor_who.txt` if you're using the Windows version)
