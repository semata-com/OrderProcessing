use strict;

open(TXT, "countersunkscrews.txt");
my @lines = <TXT>;
my $code = "10001";
my $desc;
for (my $i; $i < @lines; $i++)
{
    my $l = $lines[$i];
    if ($l =~ /TurboGold/)
    { 
        $l =~ / *\w* *(\w*) *(\w*) *([0-9.]*) *x *(\d*)mm *(\w*) *(\w*) *(\d*)/;
  
        my $dia = $3;
        my $len = $4;
        my $pack = $7;
        $desc = "$1 $2 $3 x $4mm $5 $6 $7";
#        my $c0 = $dia . ".0";
#        my $c1 = "0" . $len;
#        my $c2 = "0" . $pack;
#        $c0 =~ s/(.).(.).*/$1$2/;
#        $c1 =~ s/.*(...)/$1/;
#        $c2 =~ s/.*(..)./$1/;
#        $code = "$c0$c1$c2";
        $code++;
        $i++;
    }
    elsif ($l =~ /^From *£([0-9.]*)/)
    {
        my $price = $1 * 1.2;
        $price =~ s/(\d*\...).*/$1/;
        my $stockLevel = ($i * 143 / 15) % 20;
        print "            CreateProduct(orderProcessing,\"$code\", \"$desc\", $stockLevel, ${price}m, countersunkScrews);\n";
    }
}