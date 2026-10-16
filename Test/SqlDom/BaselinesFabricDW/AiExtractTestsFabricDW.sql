SELECT AI_EXTRACT('text', 'spam', 'ham');

SELECT AI_EXTRACT(t.text_col, 'spam', 'ham')
FROM dbo.Texts AS t;

DECLARE @s AS NVARCHAR (MAX) = N'Hello world';

SELECT AI_EXTRACT(@s, 'topic', 'sentiment', 'intent');

SELECT AI_EXTRACT('b' + 'a', 'topic', 'sentiment', 'intent');

SELECT AI_EXTRACT(@s + 'a', 'topic', 'sentiment', 'intent');

SELECT *
FROM AI_EXTRACT((SELECT t.text_col
                 FROM dbo.Texts AS t), 'topic', 'sentiment', 'intent');

SELECT AI_EXTRACT('text', 'spam' ERROR ON ERROR);

SELECT AI_EXTRACT('text', 'spam', 'ham' NULL ON ERROR);

SELECT AI_EXTRACT(@s, 'topic', 'sentiment', 'intent' DEFAULT 'Extraction unavailable' ON ERROR);


GO
CREATE OR ALTER FUNCTION dbo.fx
(@s NVARCHAR (MAX))
RETURNS NVARCHAR (MAX)
AS
BEGIN
    RETURN (AI_EXTRACT(@s, 'labelA', 'labelB' DEFAULT N'Extraction unavailable' ON ERROR));
END
