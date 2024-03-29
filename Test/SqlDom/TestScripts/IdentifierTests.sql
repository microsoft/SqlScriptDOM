-- sacaglar: Unicode, square brackets, double quotes

-- TODO sacaglar, uncomment this when it is supported
set QUOTED_IDENTIFIER on

-- Tests plain, both quoted identifiers and unicode chars
create table tempdb."table".[name] (Gökhan int);

-- Tests unicode chars with a space in between
create table [name] ([Gökhan Çağlar] int);

-- Tests the escaped characters
create table "temp""db"."table"""."""""name" (c1 int);

-- Tests the escaped characters
create table [temp]]db].[table]]].[]]]]name] (c1 int);
create table [[temp]]db].[table]]].[]]]]name] (c1 int);
