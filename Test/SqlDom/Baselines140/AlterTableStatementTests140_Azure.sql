﻿ALTER TABLE T SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE=T_HST, DATA_CONSISTENCY_CHECK=ON, HISTORY_RETENTION_PERIOD=2 MONTHS));


GO
ALTER TABLE T SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE=T_HST, HISTORY_RETENTION_PERIOD=INFINITE));