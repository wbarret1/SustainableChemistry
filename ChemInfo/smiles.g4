/* Converted from PEG Grammar by http://bottlecaps.de/convert/  */

grammar smiles;

smiles   : atom ( chain | branch )*;
chain    : ( bond? ( atom | ringClosure ) )+;
branch   : '(' bond? smiles+ ')';
atom     : organicSymbol
           | aromaticSymbol
           | atomSpec
           | wildcard;
bond     : '-'
           | '='
           | '#'
           | '$'
           | ':'
           | '/'
           | '\''
           | '.';
atomSpec : '[' isotope? ( 'se' | 'as' | aromaticSymbol | elementsymbol | wildcard ) chiralClass? hCount? charge? class? ']';
organicSymbol
         : 'B' 'r'?
           | 'C' 'l'?
           | 'N'
           | 'O'
           | 'P'
           | 'S'
           | 'F'
           | 'I';
aromaticSymbol
         : 'b'
           | 'c'
           | 'n'
           | 'o'
           | 'p'
           | 's';
wildcard : '*';

/*
<?TOKENS?>

tokens{ 
  elementsymbol, 
  ringClosure, 
  chiralClass, 
  charge, 
  hCount, 
  class, 
  isotope
}
*/
elementsymbol : [A-Z][a-z]?;
ringClosure : '%' [1-9][0-9]
           | [0-9];
chiralClass : ( '@' ( '@' | 'TH' [1-2] | 'AL' [1-2] | 'SP' [1-3] | 'TB' ( '1' [0-9]? | '2' '0'? | [3-9] ) | 'OH' ( '1' [0-9]? | '2' [0-9]? | '3' '0'? | [4-9] ) )? )?;
charge   : '-' ( '-' | '0' | '1' [0-5]? | [2-9] )?
           | '+' ( '+' | '0' | '1' [0-5]? | [2-9] )?;
hCount   : 'H' [0-9]?;
class    : ':' [0-9]+;
isotope  : [1-9] [0-9]? [0-9]?;