CREATE EXTERNAL RESOURCE POOL p;

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_CPU_PERCENT = 1
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_MEMORY_PERCENT = 100
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_PROCESSES = 10
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_MEMORY_PERCENT = 50,
            MAX_CPU_PERCENT = 20
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_MEMORY_PERCENT = 50,
            MAX_PROCESSES = 1
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_PROCESSES = 50,
            MAX_CPU_PERCENT = 1
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_CPU_PERCENT = 30,
            MAX_PROCESSES = 1,
            MAX_MEMORY_PERCENT = 50
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_CPU_PERCENT = 30,
            MAX_PROCESSES = 165463,
            MAX_MEMORY_PERCENT = 50
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            AFFINITY CPU = AUTO
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            AFFINITY CPU = (1)
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            AFFINITY CPU = (1 TO 5, 6 TO 7)
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            AFFINITY NUMANODE = (1)
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            AFFINITY NUMANODE = (1 TO 5, 6 TO 7)
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_CPU_PERCENT = 30,
            MAX_PROCESSES = 165463,
            MAX_MEMORY_PERCENT = 50,
            AFFINITY NUMANODE = (1 TO 5)
          );

CREATE EXTERNAL RESOURCE POOL p
    WITH  (
            MAX_CPU_PERCENT = 30,
            MAX_PROCESSES = 165463,
            MAX_MEMORY_PERCENT = 50,
            AFFINITY CPU = AUTO
          );