# Todo API

## Build and run

docker build -t todo-api .
docker run -p 5014:80 -e ASPNETCORE_ENVIRONMENT=Development todo-api