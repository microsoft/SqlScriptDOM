# Converting TSqlTokenTypes class to enum
$content = [System.IO.File]::ReadAllText($args[0]).Replace("class TSqlTokenTypes","enum TSqlTokenType")
$content = ($content).Replace("public const int","")
$content = ($content).Replace(";",",")
$content = ($content).Replace("NULL_TREE_LOOKAHEAD = 3","None = 0")
$content = ($content).Replace("EOF","EndOfFile")
[System.IO.File]::WriteAllText($args[1], $content)