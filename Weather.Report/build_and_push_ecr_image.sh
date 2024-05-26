#!/bin/bash
set -e

aws ecr get-login-password --region us-east-1 --profile weather-ecr-agent | docker login --username AWS --password-stdin 058264310282.dkr.ecr.eu-north-1.amazonaws.com
docker build -f ./Dockerfile -t weather-report:latest .
docker tag weather-report:latest 058264310282.dkr.ecr.eu-north-1.amazonaws.com/weather-report:latest
docker push 058264310282.dkr.ecr.eu-north-1.amazonaws.com/weather-report:latest