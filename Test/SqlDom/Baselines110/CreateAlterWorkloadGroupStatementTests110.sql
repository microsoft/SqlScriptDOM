CREATE WORKLOAD GROUP g
    WITH  (
            GROUP_MIN_MEMORY_PERCENT = 42
          );

CREATE WORKLOAD GROUP g
    WITH  (
            GROUP_MIN_MEMORY_PERCENT = 42,
            MAX_DOP = 52
          );

ALTER WORKLOAD GROUP g
    WITH  (
            GROUP_MIN_MEMORY_PERCENT = 42
          );

ALTER WORKLOAD GROUP g
    WITH  (
            GROUP_MIN_MEMORY_PERCENT = 42,
            MAX_DOP = 52
          );