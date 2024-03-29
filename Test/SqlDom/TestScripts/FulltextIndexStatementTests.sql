-- sacaglar:  comments inline

-- basic version
CREATE FULLTEXT INDEX ON ...t1 key index [i1]

-- with fulltext catalog name
CREATE FULLTEXT INDEX ON t1 key index [i1] on fc

-- with columns
CREATE FULLTEXT INDEX ON t1(c1) key index [i1] on fc
CREATE FULLTEXT INDEX ON t1(c1 type column [binary]) key index [i1] on fc
CREATE FULLTEXT INDEX ON t1(c1 language 0x413) key index [i1] on fc
CREATE FULLTEXT INDEX ON t1(c1, c2 language 1043, c3 type column varchar language 'english') key index [i1] on fc
CREATE FULLTEXT INDEX ON [dbo].[CatalogA_table]([column1] LANGUAGE [English]) KEY INDEX [PK_CatalogA_table] ON [CatalogA] WITH CHANGE_TRACKING OFF
CREATE FULLTEXT INDEX ON [dbo].[CatalogA_table]([column1] LANGUAGE English) KEY INDEX [PK_CatalogA_table] ON [CatalogA] WITH CHANGE_TRACKING OFF

-- with change tracking options
CREATE FULLTEXT INDEX ON t1(c1 type column [binary]) key index [i1] with change_tracking Manual
CREATE FULLTEXT INDEX ON t1(c1 type column [binary]) key index [i1] with change_tracking Off
CREATE FULLTEXT INDEX ON t1(c1 type column [binary]) key index [i1] on fc with change_tracking Auto
CREATE FULLTEXT INDEX ON t1(c1 type column [binary]) key index [i1] with change_tracking Off, No Population

GO

-- ALTER FULLTEXT INDEX tests
alter fulltext index on dbo.t1 enable
alter fulltext index on t1 disable

alter fulltext index on t1 set change_tracking manual
alter fulltext index on t1 set change_tracking auto
alter fulltext index on t1 set change_tracking off

alter fulltext index on t1 add (c1, c2 type column int, c3 language 1043)
alter fulltext index on t1 add (c1, c2 type column int, c3 language 'THAI')
alter fulltext index on t1 add (c1) with no population

alter fulltext index on t1 drop (c1, c2) with no population
alter fulltext index on t1 drop (c1)

alter fulltext index on t1 start full population
alter fulltext index on t1 start incremental population
alter fulltext index on t1 start update population

alter fulltext index on t1 stop population

alter fulltext index on t1 pause population
alter fulltext index on t1 resume population

GO

drop fulltext index on dbo.t1
drop fulltext index on t1
