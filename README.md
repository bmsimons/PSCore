# PSCore
CSCore bindings optimized for PowerShell use.

## Getting Started

[TBD]

### Prerequisites

1. PowerShell .NET 2 or greater.

### Installing

[TBD]

## Running the tests

[TBD]

## Deployment

Copy the DLL to a local folder and call it from PowerShell using something like this:

```csharp
Add-Type -Path PSCore.dll
$r = [PSCore.LoopbackRecorder]

$r::StartRecording("X:\test1.mp3")
```

(This will lock the DLL until the PowerShell IDE is closed.)

Or this:

```csharp
[System.Reflection.Assembly]::Load( [System.IO.File]::ReadAllBytes("pscore.dll") )

$r = [PSCore.LoopbackRecorder]
$r::StartRecording("X:\test1.mp3")
```

(The above will not lock the DLL file. It works because we do not require explicit bindings when loading the DLL.)

## Built With

* [Visual Studio Community Edition 2017](https://www.visualstudio.com/downloads/) - Visual Studio Community Edition 2017

## Contributing

[TBD]

## Versioning

[TBD]

## Authors

* **bmsimons** - *Initial work* - [bmsimons](https://github.com/bmsimons)

See also the list of [contributors](https://github.com/bmsimons/PSCore/contributors) who participated in this project.

## License

[TBD]

## Acknowledgments

* My cat
