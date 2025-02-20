//
// Prints a list of all files that the user does not have write access to.
//

using System.Security.AccessControl;
using System.Security.Principal;

string filePath = @"I:\OneDrive";
Console.WriteLine($"Searching within {filePath}.");
var sum = PrintFilesWithoutWriteAccess(new DirectoryInfo(filePath));
Console.WriteLine($"Done. {sum} files.");

static int PrintFilesWithoutWriteAccess(DirectoryInfo parentDir)
{
    var files = FilesWithoutWriteAccess(parentDir).ToList();
    var dirs = DirectoriesWithoutWriteAccess(parentDir).ToList();
    foreach (var file in files)
        Console.WriteLine($"{file.FullName}");
    foreach (var dir in dirs)
        Console.WriteLine($"{dir.FullName}");

    return files.Count
        + dirs.Count
        + parentDir.GetDirectories().Sum(subDir => PrintFilesWithoutWriteAccess(subDir));
}

static IEnumerable<FileInfo> FilesWithoutWriteAccess(DirectoryInfo d)
{
    foreach (var file in d.GetFiles())
    {
        var hasAccess = true;
        try
        {
            hasAccess = HasWriteAccess(file.GetAccessControl());
        }
        catch (UnauthorizedAccessException)
        {
            hasAccess = false;
        }
        if (!hasAccess)
            yield return file;
    };
}

static IEnumerable<DirectoryInfo> DirectoriesWithoutWriteAccess(DirectoryInfo d)
{
    foreach (var dir in d.GetDirectories())
    {
        var hasAccess = true;
        try
        {
            hasAccess = HasWriteAccess(dir.GetAccessControl());
        }
        catch (UnauthorizedAccessException)
        {
            hasAccess = false;
        }
        if (!hasAccess)
            yield return dir;
    };
}

static bool HasWriteAccess(FileSystemSecurity security)
{
    try
    {
        return security
            .GetAccessRules(true, true, typeof(NTAccount))
            .OfType<FileSystemAccessRule>()
            .Any(rule => rule.AccessControlType == AccessControlType.Allow &&
                    (rule.FileSystemRights.HasFlag(FileSystemRights.Write) ||
                     rule.FileSystemRights.HasFlag(FileSystemRights.Modify)));
    }
    catch (UnauthorizedAccessException)
    {
        return false;
    }
    catch (Exception ex)
    {
        throw;
    }
}