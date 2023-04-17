# String comparison
$content = [System.IO.File]::ReadAllText($args[0]).Replace("CaseInsensitiveHashCodeProvider.Default,","StringComparer.OrdinalIgnoreCase")
$content = $content.Replace("CaseInsensitiveComparer.Default","")
# We don't want wrapper exceptions - IOException is totally fine!
$content = $content.Replace("using TokenStreamIOException          = antlr.TokenStreamIOException;","")
$content = $content.Replace("using CharStreamException             = antlr.CharStreamException;","")
$content = $content.Replace("using CharStreamIOException           = antlr.CharStreamIOException;","")
# We don't want exceptions wrapping logic!
$content = $content.Replace("catch (CharStreamException cse) {","finally {")
$content = $content.Replace("if ( cse is CharStreamIOException ) {","if (false) {")
$content = $content.Replace("throw new TokenStreamIOException(((CharStreamIOException)cse).io);","")
$content = $content.Replace("throw new TokenStreamException(cse.Message);","")
[System.IO.File]::WriteAllText($args[1], $content)