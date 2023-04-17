-- sacaglar, the first create token could cause infinite loop
-- if we didn't skip over it after figuring out that it can't
-- be parsed.  Because one of the tokens we recover from errors
-- are CREATE.

select * from

create )

create table t1 (c1 int)
