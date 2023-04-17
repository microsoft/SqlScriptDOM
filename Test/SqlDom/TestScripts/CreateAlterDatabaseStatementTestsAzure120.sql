-- Azure options
create database d1 (service_objective = 'basic')
create database d1 (maxsize = 100 mb)
create database d1 (edition = 'business', service_objective = 'shared')
create database d1 (maxsize = 1 gb, service_objective = 'shared')
create database d1 (maxsize = 100 mb, edition = 'web', service_objective = 'shared')
go

alter database d1 modify (service_objective = 'basic')
alter database d1 modify (maxsize = 100 mb)
alter database d1 modify (edition = 'basic', service_objective = 'basic')
alter database d1 modify (service_objective = 'basic', maxsize = 100 mb)
alter database d1 modify (maxsize = 1 gb, edition = 'basic', service_objective = 'basic')
go
