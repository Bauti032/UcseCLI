using Spectre.Console;
using System.Text.RegularExpressions;

public static class FileGenerator
{
    // Identificador C# válido: empieza con letra o _, sin separadores de path.
    private static readonly Regex ValidFileName = new(@"^[a-zA-Z_][a-zA-Z0-9_]*$");

    public static void CreateFiles(
        string path,
        string[] files)
    {
        var basePath = Directory.GetCurrentDirectory();
        var resolvedPath = Path.GetFullPath(Path.Combine(basePath, path));

        // Prevenir path traversal: el destino debe estar dentro del directorio actual.
        if (!resolvedPath.StartsWith(basePath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(resolvedPath, basePath, StringComparison.OrdinalIgnoreCase))
        {
            AnsiConsole.MarkupLine(
                "[red]El directorio especificado está fuera del directorio actual.[/]"
            );
            return;
        }

        // Validar nombres de archivo antes de crear nada.
        foreach (var file in files)
        {
            if (!ValidFileName.IsMatch(file))
            {
                AnsiConsole.MarkupLine(
                    $"[red]Nombre de archivo inválido: '{Markup.Escape(file)}'. Usá solo letras, números y guiones bajos.[/]"
                );
                return;
            }
        }

        Directory.CreateDirectory(resolvedPath);

        int created = 0;
        foreach (var file in files)
        {
            var fullPath =
                Path.Combine(
                    resolvedPath,
                    $"{file}.cs"
                );

            File.WriteAllText(
                fullPath,
            $@"public class {file}
{{
}}
");
            created++;
        }

        AnsiConsole.MarkupLine(
            $"[green]{created} archivos creados[/]"
        );
    }
}