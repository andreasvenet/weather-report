#!/bin/bash
set -e

aws ecr get-login-password --region us-east-1 --profile weather-ecr-agent | docker login --username AWS --password-stdin 058264310282.dkr.ecr.eu-north-1.amazonaws.com
docker build -f ./Dockerfile -t weather-precipitation:latest .
docker tag weather-precipitation:latest 058264310282.dkr.ecr.eu-north-1.amazonaws.com/weather-precipitation:latest
docker push 058264310282.dkr.ecr.eu-north-1.amazonaws.com/weather-precipitation:latest