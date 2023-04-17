# We don't want wrapper exceptions - IOException is totally fine!
$content = [System.IO.File]::ReadAllText($args[0]).Replace("using TokenStreamIOException   = antlr.TokenStreamIOException;","")
[System.IO.File]::WriteAllText($args[1], $content)