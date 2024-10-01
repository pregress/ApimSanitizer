# ApimSanitizer as a Global Tool

To see a list of options, run:

```bash
ApimSanitizer --help
```

The current options are (output of `ApimSanitizer --help`):

```text
Description:
  Apim sanitizier, sanitize open api definitions to import them into Azure APIM

Usage:
  ApimSanitizer [options]

Options:
  -f, --file <file> (REQUIRED)  The input open api definition file in yaml format.
  --version                     Show version information
  -?, -h, --help                Show help and usage information
```



## Exit Codes

Coverlet outputs specific exit codes to better support build automation systems for determining the kind of failure so the appropriate action can be taken.

```bash
0 - Success.
1 - Invalid input file
2 - Input file doesn't exist
3 - Invalid extension, must be of type .yml
2147483647 - General exception occurred during process.
```