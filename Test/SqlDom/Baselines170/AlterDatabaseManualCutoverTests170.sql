ALTER DATABASE db
    MODIFY (SERVICE_OBJECTIVE = 'HS_Gen5_16') WITH MANUAL_CUTOVER;

ALTER DATABASE db
    MODIFY (EDITION = 'Hyperscale') WITH MANUAL_CUTOVER;

ALTER DATABASE db
    MODIFY (SERVICE_OBJECTIVE = ELASTIC_POOL (NAME = [hspool])) WITH MANUAL_CUTOVER;

ALTER DATABASE db PERFORM_CUTOVER;
