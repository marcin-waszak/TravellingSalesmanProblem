while IFS=';' read col1_1 col2_1
do
	S1NAME=$col2_1
	S1ID=$col1_1
	while IFS=';' read col1_2 col2_2
	do
		S2NAME=$col2_2
		S2ID=$col1_2
		echo "Distance from $S1ID to $S2ID"
		# calculate using distance.sh
	done < settlements-list.csv
done < settlements-list.csv