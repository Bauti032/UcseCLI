using Spectre.Console;

namespace UcseCLI;

public static class AbstractClassGenerator
{
    public static void CreateAbstractClasses(
        string path,
        string[] classes)
    {
        Directory.CreateDirectory(path);

        foreach (var className in classes)
        {
            var filePath = Path.Combine(
                path,
                $"{className}.cs"
            );

            File.WriteAllText(
                filePath,
$@"public abstract class {className}
{{
}}"
            );

            AnsiConsole.MarkupLine(
                $"[green]Creada:[/] {filePath}"
            );
        }
    }
}