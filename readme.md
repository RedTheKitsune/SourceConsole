# SourceConsole

![20190510202635_1](https://user-images.githubusercontent.com/15838111/57545124-10f52000-7362-11e9-9a44-9deb3a7aaac5.jpg)
*The source console being used in my game Galactic Lander*

SourceConsole is an in-game developer console for Unity.
It's designed to be compatible with most game types, and is modeled after the classic Source engine console.

## Installation

SourceConsole is distributed as a Unity Package Manager (UPM) package.

In Unity:
- **Window → Package Manager**
- Click **+** (top left) → **Add package from git URL...**
- Paste one of the following:
```
https://github.com/RedTheKitsune/SourceConsole.git
```

After the package is installed, the console is spawned automatically on game start and persists across scene loads.  
Toggle the console with the **backquote** key (<kbd>`</kbd>). (Works with both the legacy Input Manager and the new Input System when enabled.)

## Usage

### Creating commands

Add the `ConCommand` attribute to **static methods**. The method name becomes the command name unless you provide a custom name.

```csharp
[ConCommand("memeCube_setColor", "Sets the MemeCube's color!")]
public static void SetColor(float r, float g, float b) { ... }
```

Supported argument casting is based on the input string and includes `bool`, `int`, `float`, and `string`. Parameters can be **optional** via C# default values.

### Creating variables (convars)

Add the `ConVar` attribute to a **static property** or **static field**.

```csharp
[ConVar("memeCube_spinRate", "Sets the MemeCube's spinrate!")]
public static int SpinRate { get; set; }
```

Typing a convar name with no value prints the current value; providing a value sets it.

### Full example

```csharp
using SourceConsole;
using UnityEngine;

public class MemeCube : MonoBehaviour {
    private static MemeCube Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(SpinRate, 0, SpinRate);
    }

    [ConCommand("memeCube_setColor", "Sets the MemeCube's color!")]
    public static void SetColor(float r, float g, float b)
    {
        if (Singleton == null) return;
        Singleton.GetComponent<Renderer>().material.color = new Color(r, g, b);
    }

    [ConCommand("memeCube_otherTest", "Testing the optional parameters")]
    public static int OtherTest(float meme = 2.65f)
    {
        SourceConsole.SourceConsole.print($"Test meme received float: {meme}");
        return 2;
    }

    [ConVar("memeCube_spinRate", "Sets the MemeCube's spinrate!")]
    public static int SpinRate { get; set; }
}
```

Example console input:
- `memeCube_setColor 1 0 0`
- `memeCube_otherTest`
- `memeCube_otherTest 4.2`
- `memeCube_spinRate 25`
- `memeCube_spinRate`

## Requirements

TextMeshPro is required for the console UI to work. This package declares a UPM dependency on TextMeshPro (`com.unity.textmeshpro`).
If Unity prompts you to import TMP resources in a fresh project, accept the import.

## Why?

Many Unity projects end up needing a small developer console for debugging and tuning at runtime (especially during iteration).
SourceConsole exists to provide a lightweight, Source-engine-style console with commands and variables defined directly in C#.

## Other developer consoles

There are many developer console assets available for Unity, with different UX and feature sets. If SourceConsole doesn't fit your needs,
use whatever tool works best for your project.

## Contributing

Find a bug or improvement? Open an issue or send a pull request.
