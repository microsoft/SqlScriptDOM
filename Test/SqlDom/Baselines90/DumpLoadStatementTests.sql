BACKUP DATABASE d1
    TO someDevice;

BACKUP DATABASE d1
    TO @deviceName, DISK = 'c:', TAPE = @tapeName;


GO
BACKUP LOG d1
    TO someDevice;


GO
BACKUP LOG d1
    TO someDevice;


GO
RESTORE DATABASE db1;


GO
RESTORE LOG @var2 FROM someDevice;


GO
RESTORE LOG @var2 FROM someDevice;