UPDATE t1
SET @a += NULL;


GO
UPDATE t1
SET @a -= 1;


GO
UPDATE t1
SET @a *= 1 + 1;


GO
UPDATE t1
SET @a /= 1;


GO
UPDATE t1
SET @a %= 1;


GO
UPDATE t1
SET @a &= 1;


GO
UPDATE t1
SET @a |= 1;


GO
UPDATE t1
SET @a ^= 1;


GO
UPDATE t1
SET @a = c1 += NULL,
    @a = c1 -= DEFAULT,
    @a = c1 *= 1 + 1,
    @a = c1 /= 1,
    @a = c1 %= 1,
    @a = c1 &= 1,
    @a = c1 |= 1,
    @a = c1 ^= 1;

UPDATE t1
SET c1 += NULL,
    c1 -= DEFAULT,
    c1 *= 1 + 1,
    c1 /= 1,
    c1 %= 1,
    c1 &= 1,
    c1 |= 1,
    c1 ^= 1;


GO
SET @a += 1 + 1;


GO
SET @a -= 1;


GO
SET @a *= 1;


GO
SET @a /= 1;


GO
SET @a %= 1;


GO
SET @a &= 1;


GO
SET @a |= 1;


GO
SET @a ^= 1;


GO
SELECT @a += 1;


GO
SELECT @a -= 1;


GO
SELECT @a *= 1;


GO
SELECT @a /= 1;


GO
SELECT @a %= 1;


GO
SELECT @a &= 1;


GO
SELECT @a |= 1;


GO
SELECT @a ^= 1;