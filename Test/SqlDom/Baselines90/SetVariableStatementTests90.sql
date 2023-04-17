SET @a.b = 12 / 34;


GO
SET @a::b = 12 / 34;


GO
SET @a.b ();


GO
SET @a.b (1, DEFAULT, a.b::func());


GO
SET @a::b ();


GO
SET @a::b (1, DEFAULT, a.b::func());

