SELECT AI_CLASSIFY('text', 'spam', 'ham');

SELECT AI_CLASSIFY(t.text_col, 'spam', 'ham')
FROM dbo.Texts AS t;

DECLARE @s AS NVARCHAR (MAX) = N'Hello world';

SELECT AI_CLASSIFY(@s, 'topic', 'sentiment', 'intent');

SELECT AI_CLASSIFY('b' + 'a', 'topic', 'sentiment', 'intent');

SELECT AI_CLASSIFY(@s + 'a', 'topic', 'sentiment', 'intent');

SELECT *
FROM AI_CLASSIFY((SELECT t.text_col
                  FROM dbo.Texts AS t), 'topic', 'sentiment', 'intent');


GO
CREATE OR ALTER FUNCTION dbo.fx
(@s NVARCHAR (MAX))
RETURNS NVARCHAR (MAX)
AS
BEGIN
    RETURN (AI_CLASSIFY(@s, 'labelA', 'labelB'));
END
