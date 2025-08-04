using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiLang
{
    public enum TokenType
    {
        VAR, FN, FOR, PRINT,
        COLON, EQUAL, PLUS, MINUS, STAR, SLASH,
        LPAREN, RPAREN, LBRACE, RBRACE, SEMICOLON,
        EOF,
    }

}
