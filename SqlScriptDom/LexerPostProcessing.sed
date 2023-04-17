# String comparison
s/CaseInsensitiveHashCodeProvider.Default,/StringComparer.OrdinalIgnoreCase/
s/CaseInsensitiveComparer.Default//
# We don't want wrapper exceptions - IOException is totally fine!
s/using TokenStreamIOException          = antlr.TokenStreamIOException;//
s/using CharStreamException             = antlr.CharStreamException;//
s/using CharStreamIOException           = antlr.CharStreamIOException;//
# We don't want exceptions wrapping logic!
s/catch (CharStreamException cse) {/finally {/
s/if ( cse is CharStreamIOException ) {/if (false) {/
s/throw new TokenStreamIOException(((CharStreamIOException)cse).io);//
s/throw new TokenStreamException(cse.Message);//