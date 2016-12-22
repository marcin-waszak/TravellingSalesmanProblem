#!/bin/bash
MIASTO1=$1
MIASTO2=$2
SITE=$(curl -s 'http://www.odleglosci.pl/odleglosci.php' -H 'User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36' -H 'Content-Type: application/x-www-form-urlencoded' -H 'Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8' -H 'Cache-Control: max-age=0' -H 'Referer: http://www.odleglosci.pl/odleglosci.php' -H 'Connection: keep-alive' --data 'woj1=Wszystkie+wojew%C3%B3dztwa&miasto1='+$MIASTO1+'&woj2=Wszystkie+wojew%C3%B3dztwa&miasto2='+$MIASTO2+'&szukaj=Wyznacz&wyslij=wyslij' --compressed)
echo $SITE | grep -oP ">\K[0-9.]*(?= km<)"