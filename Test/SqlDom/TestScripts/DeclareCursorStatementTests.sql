-- basic case
DECLARE t1_cursor CURSOR FOR SELECT * FROM t1

-- old syntax
DECLARE c1 INSENSITIVE SCROLL CURSOR FOR SELECT * FROM t1

-- checking all options...
DECLARE c1 CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY TYPE_WARNING
   FOR SELECT * FROM t1

DECLARE c1 CURSOR GLOBAL SCROLL KEYSET SCROLL_LOCKS FOR SELECT * FROM t1

DECLARE c1 CURSOR DYNAMIC OPTIMISTIC FOR SELECT * FROM t1

DECLARE c1 CURSOR FAST_FORWARD FOR SELECT * FROM t1
