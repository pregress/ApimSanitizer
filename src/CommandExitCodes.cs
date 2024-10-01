namespace ApimSanitizer;

internal enum CommandExitCodes
{
    Success = 0,
    InvalidFile = 1,
    FileNotExists = 2,
    InvalidExtension = 3,

    Exception = int.MaxValue,
}