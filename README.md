# NSwag.MockServer
A lightweight Mock server by input swagger document

# Why NSwag.MockServer
In API first development, instead of generating the documentation from the code, you need to write the specification first, Usually different programing ecosystem have little differences, for ASP.NET Core I like defining API firstly(request, response, controller) without implementation then using [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) generating Open api document.
The benefits are:
* You can design your API for the consumers and not as a consequence of your implementation.
* You can use the specification file to mock your new server endpoints before they are released so you can more decouple frontend and backend development.
* Open api specification is difficult, write it by hands is a self-challenging work.

# Prerequisite
* Open api document(3.0)
* The example section was document in Open api document for response type

# Getting started
Let's say you have already got Open api document(swagger json/yaml) file, whatever you can reach it in http or it's a file in you disk, you can you NSwag.MockServer to setup a mock server.

NSwag.MockServer is a dock image so you can use it by one line command.
* Setup mock server by Open Api document(swagger json/yaml) url
``` docker
docker run -it -e swaggerUrl="http://*.json" -p 60020:80 nswag.mockserver:v1
```
Then all apis was mocked in http://localhost:60020
* Setup mock server by Open api document(swagger json) file
ensure you current folder have a json named `swagger.json`:
``` docker
docker run -it -v $(pwd):/app/swagger -p 60020:80 nswag.mockserver:v1
```

# Enhancement in future
* Support open api 2.0 document
* Auto generate response even you Open api document didn't have example section

