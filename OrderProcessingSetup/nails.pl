use strict;

open(TXT, "nails.txt") or die "cannot open roundheadscrews.txt $?\n"; 
my @lines = <TXT>;
my $code = "30001";
my $desc;
for (my $i; $i < @lines; $i++)
{
    my $l = $lines[$i];
    if ($l =~ /Bright/)
    { 
        $l =~ / *\w* *(\w*) *(\w*) *(\w*) *([0-9.]*) *x *(\d*)mm *(\w*) *(\w*) *(\d*)/;
  
        my $dia = $4;
        my $len = $5;
        my $pack = $8;
        $desc = "$3 $4 x $5mm $6 $7 $8";
#        my $c0 = $dia . ".0";
#        my $c1 = "0" . $len;
#        my $c2 = "0" . $pack;
#        $c0 =~ s/(.).(.).*/$1$2/;
#        $c1 =~ s/.*(...)/$1/;
#        $c2 =~ s/.*(...)/$1/;
#        $code = "$c0$c1$c2";
        $code++;
        $i++;
    }
    elsif ($l =~ /From *£([0-9.]*)/)
    {
        my $price = $1 * 1.2;
        $price =~ s/(\d*\...).*/$1/;
        my $stockLevel = ($i * 143 / 15) % 20;
        print "            CreateProduct(orderProcessing,\"$code\", \"$desc\", $stockLevel, ${price}m, nails);\n";
    }
}