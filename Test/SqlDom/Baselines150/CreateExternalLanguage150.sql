CREATE EXTERNAL LANGUAGE language1
    AUTHORIZATION dbo
    FROM (CONTENT = 0x504b03041400000000006d8f3c4ed58e, FILE_NAME = 'WindowsExtension.dll', ENVIRONMENT_VARIABLES = N'{"TEST":"C:\\Python37"}');

CREATE EXTERNAL LANGUAGE language2
    FROM (CONTENT = 0x1f8b, FILE_NAME = 'LinuxExtension.dll', PLATFORM = LINUX, PARAMETERS = N'{"Param2":"ParameterValue2"}', ENVIRONMENT_VARIABLES = N'{"Var":"TestVariable"}');

CREATE EXTERNAL LANGUAGE language3
    FROM (CONTENT = 0x504b03041400000000006d8f3c4ed58e, FILE_NAME = 'WindowsExtension.dll', PLATFORM = WINDOWS),(CONTENT = 0x1f8b, FILE_NAME = 'LinuxExtension.dll', PLATFORM = LINUX);

CREATE EXTERNAL LANGUAGE language4
    FROM (CONTENT = '\\machine-name\dir1\dir2\windowsextension.zip', FILE_NAME = 'WindowsExtension.dll', PLATFORM = WINDOWS);