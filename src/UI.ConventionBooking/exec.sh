#!/usr/bin/env bash
docker build -t ui-convention-booking .
docker run --init -p 3000:3000 -p 3001:3001 -it ui-convention-booking
