-- sacaglar, the first create token could cause infinite recursion
-- if we didn't skip over it after figuring out that it can't
-- be parsed.  Because one of the tokens we recover from errors
-- are CREATE.

abcde 

create )

create table t1 (c1 int)