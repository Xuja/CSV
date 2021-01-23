# CSP
CSP is an application where .csv and .xml files can be uploaded and validated. 

## Installation
### Docker
Use the command prompt to get the image of the application

```bash
docker pull xwjl92/csp
```

#### Usage
Open command prompt and run 

```bash
docker pull xwjl92/csp
docker image ls
docker run -i -p 8080:80 -t <image id>
```

Or use the Docker Desktop to run the Image with container port 80:/tcp
Open```localhost:8080``` to see the application

## API - Postman
With Postman you can send a POST request to the path localhost:xxxx/api/records/ with the correct file.
In the body pick form-data and enter files as key, select the correct file as value and hit send.

Now with a GET request localhost:xxxx/api/records/ will show the invalid records in JSON
