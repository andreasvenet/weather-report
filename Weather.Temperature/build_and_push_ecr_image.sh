#!/bin/bash
set -e

aws ecr get-login-password --region us-east-1 --profile weather-ecr-agent | docker login --username AWS --password-stdin 748840328402.dkr.ecr.us-east-1.amazonaws.com
docker build -f ./Dockerfile -t weather-temperature:latest .
docker tag weather-temperature:latest 748840328402.dkr.ecr.us-east-1.amazonaws.com/weather-temperature:latest
docker push 748840328402.dkr.ecr.us-east-1.amazonaws.com/weather-temperature:latest