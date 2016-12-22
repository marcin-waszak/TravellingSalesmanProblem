#!/bin/bash
echo "from;to;distance"
i=1
while IFS=';' read s1id s1name
do
	test $i -eq 1 && ((i=i+1)) && continue
	j=1
	while IFS=';' read s2id s2name
	do
		test $j -eq 1 && ((j=j+1)) && continue
		distance=$(sh ./distance.sh $s1id $s2id)
		echo "$s1id;$s2id;$distance"
	done < little-settlements-list.csv
done < little-settlements-list.csv