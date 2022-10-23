#!/usr/bin/env bash
docker build -t convention-booking-api .
docker run --init -p 8081:80 -it convention-booking-api
