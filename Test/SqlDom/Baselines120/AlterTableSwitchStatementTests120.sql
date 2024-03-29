﻿ALTER TABLE t1 SWITCH TO t2 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 0, ABORT_AFTER_WAIT = NONE));

ALTER TABLE t1 SWITCH PARTITION 1 TO t2 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = SELF));

ALTER TABLE t1 SWITCH PARTITION 1 TO t2 PARTITION 1 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1, ABORT_AFTER_WAIT = BLOCKERS));

ALTER TABLE t1 SWITCH TO t2 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 1440 MINUTES, ABORT_AFTER_WAIT = NONE));

ALTER TABLE t1 SWITCH PARTITION 1 TO t2 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 71582 MINUTES, ABORT_AFTER_WAIT = SELF));

ALTER TABLE t1 SWITCH PARTITION 1 TO t2 PARTITION 1 WITH (WAIT_AT_LOW_PRIORITY (MAX_DURATION = 0 MINUTES, ABORT_AFTER_WAIT = BLOCKERS));
