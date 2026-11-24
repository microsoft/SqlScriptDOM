# We don't want wrapper exceptions - IOException is totally fine!
$content = [System.IO.File]::ReadAllText($args[0]).Replace("using TokenStreamIOException   = antlr.TokenStreamIOException;","")
# Every rule method checks the stack first, so that deep nesting throws InsufficientExecutionStackException
# instead of overflowing the stack. A rule signature ends with the "//throws" comment and its body opens on the next line.
$rules = [regex]::Matches($content, '//throws RecognitionException').Count
$content = [regex]::Replace($content, '(?m)^([^\r\n]*\) *//throws RecognitionException[^\r\n]*\r?\n\{)(\r?\n)', '${1}${2}' + "`t`t" + 'global::System.Runtime.CompilerServices.RuntimeHelpers.EnsureSufficientExecutionStack();${2}')
$checks = [regex]::Matches($content, 'RuntimeHelpers\.EnsureSufficientExecutionStack\(\);').Count
if ($rules -eq 0 -or $checks -ne $rules) { throw "Stack check inserted into $checks of $rules rule methods in $($args[0])" }
[System.IO.File]::WriteAllText($args[1], $content)