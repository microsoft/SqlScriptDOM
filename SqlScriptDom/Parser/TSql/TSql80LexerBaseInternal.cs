using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal partial class TSql80LexerInternal 
    {
        public override int testLiteralsTable(int ttype)
        {
            string tokenText = text.ToString();
            if ((tokenText == null) || (tokenText == string.Empty))
                return ttype;
            else
            {
                //In TSql80, allow colons to be specified after keywords
                //
                if (tokenText.Substring(tokenText.Length - 1, 1) == ":")
                {
                    tokenText = tokenText.Substring(0, tokenText.Length - 1);
                }

                return testLiteralsTable(tokenText, ttype);
            }
        }
    }
}
