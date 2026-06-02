using Spectre.Console;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UcseCLI;

public static class ProjectGenerator
{
    // Solo letras, números y guiones bajos. Debe empezar con letra.
    private static readonly Regex ValidName = new(@"^[a-zA-Z][a-zA-Z0-9_]*$");

    public static void CreateSolution(
        string projectName)
    {
        if (string.IsNullOrWhiteSpace(projectName) || !ValidName.IsMatch(projectName))
        {
            AnsiConsole.MarkupLine(
                "[red]Nombre inválido. Usá solo letras, números y guiones bajos, empezando con una letra.[/]"
            );
            return;
        }

        var root = Path.GetFullPath(
            Path.Combine(
                Environment.CurrentDirectory,
                projectName
            )
        );

        if (Directory.Exists(root))
        {
            AnsiConsole.MarkupLine(
                "[red]La carpeta ya existe[/]"
            );

            return;
        }

        Directory.CreateDirectory(root);

        Execute($"new classlib -n {projectName}.LogicaServicio", root);
        Execute($"new nunit -n {projectName}.LogicaTest", root);
        Execute($"new sln -n SolucionGeneral", root);
        Execute($"sln add \"{projectName}.LogicaServicio/{projectName}.LogicaServicio.csproj\"", root);
        Execute($"sln add \"{projectName}.LogicaTest/{projectName}.LogicaTest.csproj\"", root);
        Execute(
            $"add \"{projectName}.LogicaTest/{projectName}.LogicaTest.csproj\" reference \"{projectName}.LogicaServicio/{projectName}.LogicaServicio.csproj\"",
            root
        );

        AnsiConsole.MarkupLine(
            $"[green]Proyecto '{projectName}' creado correctamente[/]"
        );
    }

    private static void Execute(
        string arguments,
        string workingDirectory)
    {
        var process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        process.Start();

        string output =
            process.StandardOutput.ReadToEnd();

        string error =
            process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            AnsiConsole.MarkupLine(
                $"[red]{Markup.Escape(error)}[/]"
            );

            throw new Exception(
                $"Error ejecutando: dotnet {arguments}"
            );
        }
    }
}