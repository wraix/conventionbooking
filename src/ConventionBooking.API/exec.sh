#!/usr/bin/env bash
docker build -t convention-booking-api .
docker run --init -p 3000:3000 -p 3001:3001 -it convention-booking-api
