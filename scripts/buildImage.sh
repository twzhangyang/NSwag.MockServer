#!/bin/bash

docker build -f ./src/NSwag.MockServer/Dockerfile -t nswag.mockserver:v1 .

# run with swagger url
# docker run -it -e swaggerUrl="http://*.json" -p 60020:80 nswag.mockserver:v1

# run with volumn
# docker run -it -v $(pwd):/app/swagger -p 60020:80 nswag.mockserver:v1