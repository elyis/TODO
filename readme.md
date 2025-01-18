# Todo API

## Build and run

``` bash
docker build -t todo-api .
```
``` bash
docker run -p 5014:80 -e ASPNETCORE_ENVIRONMENT=Development todo-api
```