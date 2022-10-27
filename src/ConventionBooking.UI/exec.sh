#!/usr/bin/env bash
docker build -t convention-booking-ui .
docker run --init -p 3000:3000 -it convention-booking-ui
