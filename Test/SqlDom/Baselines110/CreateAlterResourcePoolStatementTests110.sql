CREATE RESOURCE POOL p
    WITH  (
            CAP_CPU_PERCENT = 40,
            TARGET_MEMORY_PERCENT = 4,
            MIN_IO_PERCENT = 17,
            MAX_IO_PERCENT = 18,
            CAP_IO_PERCENT = 80
          );

CREATE RESOURCE POOL p
    WITH  (
            AFFINITY SCHEDULER = AUTO,
            AFFINITY SCHEDULER = (50),
            AFFINITY SCHEDULER = (50 TO 60),
            AFFINITY SCHEDULER = (4, 5 TO 6, 70 TO 80),
            AFFINITY NUMANODE = (50),
            AFFINITY NUMANODE = (50 TO 60),
            AFFINITY NUMANODE = (4, 5 TO 6, 70 TO 80),
            AFFINITY NUMANODE = (4)
          );

ALTER RESOURCE POOL p
    WITH  (
            CAP_CPU_PERCENT = 40,
            TARGET_MEMORY_PERCENT = 4,
            MIN_IO_PERCENT = 17,
            MAX_IO_PERCENT = 18,
            CAP_IO_PERCENT = 80
          );

ALTER RESOURCE POOL p
    WITH  (
            AFFINITY SCHEDULER = AUTO,
            AFFINITY SCHEDULER = (50),
            AFFINITY SCHEDULER = (50 TO 60),
            AFFINITY SCHEDULER = (4, 5 TO 6, 70 TO 80),
            AFFINITY NUMANODE = (50),
            AFFINITY NUMANODE = (50 TO 60),
            AFFINITY NUMANODE = (4, 5 TO 6, 70 TO 80),
            AFFINITY NUMANODE = (4)
          );