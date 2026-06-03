using Spectre.Console;

namespace UcseCLI;

public static class InterfaceGenerator
{
    public static void CreateInterfaces(
        string path,
        string[] interfaces)
    {
        Directory.CreateDirectory(path);

        foreach (var interfaceName in interfaces)
        {
            var filePath = Path.Combine(
                path,
                $"{interfaceName}.cs"
            );

            File.WriteAllText(
                filePath,
$@"public interface {interfaceName}
{{
}}"
            );

            AnsiConsole.MarkupLine(
                $"[green]Creada:[/] {filePath}"
            );
        }
    }
}